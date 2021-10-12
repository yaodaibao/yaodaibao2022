using iTR.LibCore;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Server;
using MyService;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using ydb.BLL;
using ydb.Domain.Interface;
using ydb.Domain.Models;

namespace ydb.Domain.Service
{
    public partial class AuthHospitalService : IAuthHospitalService
    {
        private readonly ILogger<AuthHospitalService> _logger;
        public AuthHospitalService(ILogger<AuthHospitalService> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 根据EmployeeID和授权表ID 获取待处理的授权请求和已处理授权请求 业务不区分审批审核，避免以后添加审批审核逻辑，添加判断条件
        /// </summary>
        /// <returns></returns>
        public ResponseModel GetAuthData(string employeeID, string queryState = "", string auther = "", string hospital = "")
        {
            SQLServerHelper serverHelper = new();
            string sql = "";
            DataTable dataTable = new DataTable();
            List<string> authDataList = new();
            //提取审批审核单
            if (queryState == "auth")
            {
                sql = $@"
                        SELECT [FormID]
                              ,ISNULL([HospitalID],'') [HospitalID]
                              ,ISNULL([HospitalName],'') [HospitalName]
                              ,ISNULL([ProductName],'')  [ProductName]
                              ,ISNULL([ProductID],'') [ProductID]
                              ,ISNULL([GridName],'') [GridName]
                              ,ISNULL([GridID],'') [GridID]
                              ,ISNULL([AutherName],'') [AutherName]
                              ,ISNULL([AutherID],'') [AutherID]
                              ,ISNULL([AuthState],'') [AuthState]
                              ,ISNULL([ViewerName],'') [ViewerName]
                              ,ISNULL([ViewerID],'') [ViewerID]
                              ,ISNULL([EmployeeID],'') [EmployeeID]
                              ,ISNULL([NextAuthID],'') [NextAuthID]
                              ,ISNULL([CurrentAuthID],'') [CurrentAuthID]
                              ,[CreateTime]
                              ,[ModifyTime]
                              ,ISNULL([SaleModeName],'') [SaleModeName]
                              ,ISNULL([SaleMode],'') [SaleMode]
                          FROM [yaodaibao].[dbo].[AuthStating] where NextAuthID='{employeeID}' and AuthState <> '2'  and AuthState <> '-1' order by  AutherName";
                dataTable = serverHelper.ExecuteSql(sql);
                //处理多次提交记录，多个表单 
                foreach (DataRow row in dataTable.Rows)
                {
                    for (int i = 0; i < row["FormID"].ToString().Split("|").Length; i++)
                    {
                        //过滤查询责任人
                        if (row["AutherName"].ToString().Split("|")[i].ToString().Contains(auther) && row["HospitalName"].ToString().Split("|")[i].ToString().Contains(hospital))
                        {
                            if (authDataList.Count > 100)
                            {
                                break;
                            }
                            authDataList.Add($"{{\"formid\":\"{row["FormID"].ToString().Split("|")[i]}\",\"hospitalid\":\"{row["HospitalID"].ToString().Split("|")[i]}\",\"hospitalname\":\"{row["HospitalName"].ToString().Split("|")[i]}\",\"productname\":\"{row["ProductName"].ToString().Split("|")[i]}\",\"productid\":\"{row["ProductID"].ToString().Split("|")[i]}\",\"grid\":\"{row["GridID"].ToString().Split("|")[i]}\",\"gridname\":\"{row["GridName"].ToString().Split("|")[i]}\",\"autherid\":\"{row["AutherID"].ToString().Split("|")[i]}\",\"authername\":\"{row["AutherName"].ToString().Split("|")[i]}\",\"applyerid\":\"{row["ViewerID"].ToString().Split("|")[i]}\",\"applyername\":\"{row["ViewerName"].ToString().Split("|")[i]}\",\"salemode\":\"{row["SaleMode"].ToString().Split("|")[i]}\",\"salemodename\":\"{row["SaleModeName"].ToString().Split("|")[i]}\",\"savetype\":\"{row["AuthState"].ToString().Split("|")[i]}\"}}");
                        }
                    }
                }
            }
            //查看
            else
            {
                //过滤责任人和医院
                sql = $"SELECT td.FSupervisorID   FROM yaodaibao.dbo.t_Departments td WHERE td.FID =( SELECT te.FDeptID FROM yaodaibao.dbo.t_Employees te WHERE te.FID = '{employeeID}')";
                dataTable = serverHelper.ExecuteSql(sql);
                WorkShip workShip = new WorkShip();
                string meberID = workShip.GetAllMemberIDsByLeaderID(employeeID);
                List<string> merberList = new List<string>();
                for (int i = 0; i < meberID.Split("|").Length; i++)
                {
                    if (!string.IsNullOrEmpty(meberID.Split("|")[i]))
                    {
                        merberList.Add($"'{meberID.Split("|")[i]}'");
                    }
                }
                sql = @$"SELECT  ad.FInstitutionID,ti.FName InsName,ad.FAuthObjectID,ti1.FName objName FROM yaodaibao.dbo.AuthDatas ad LEFT JOIN yaodaibao.dbo.t_Items ti ON ad.FInstitutionID = ti.FID LEFT JOIN yaodaibao.dbo.t_Items ti1 ON ad.FAuthObjectID = ti1.FID
                  WHERE ad.FAuthObjectID in ({string.Join(",", merberList.ToArray())}) order by  ti.FName";

                dataTable = serverHelper.ExecuteSql(sql);
                foreach (DataRow row in dataTable.Rows)
                {
                    //为空不考虑，过滤查询查询条件 InsName 授权医院 objName 授权人
                    if (row["InsName"].ToString().Contains(hospital) && row["objName"].ToString().Contains(auther))
                    {
                        if (authDataList.Count > 100)
                        {
                            break;
                        }
                        //适应前端显示 调整显示对象名字 真实对应关系
                        //authDataList.Add($"{{\"institutionid\":\"{row["FInstitutionID"].ToString()}\",\"institutionname\":\"{row["InsName"].ToString()}\",\"authobjectid\":\"{row["FAuthObjectID"].ToString()}\",\"authobjectname\":\"{row["objName"].ToString()}\"}}");
                        authDataList.Add($"{{\"institutionid\":\"{row["FAuthObjectID"].ToString()}\",\"institutionname\":\"{row["objName"].ToString()}\",\"authobjectid\":\"{row["FInstitutionID"].ToString()}\",\"authobjectname\":\"{row["InsName"].ToString()}\"}}");
                    }

                }
            }

            return new ResponseModel() { DataRow = string.Join(',', authDataList.ToArray()) };

        }


        /// <summary>
        /// 保存提交的授权数据 前台不设置提交逻辑，后台添加复杂逻辑操作
        /// </summary>
        /// <param name="authData"></param>
        /// <returns></returns>
        public ResponseModel SaveAuth(JObject jsonobj)
        {
            string myEmployeeID = jsonobj["EmployeeID"]?.ToString(),
                formID = jsonobj["formid"]?.ToString(),
                saveType = jsonobj["savetype"]?.ToString(),
                hospitalID = jsonobj["hospitalid"]?.ToString(),
                hospitalName = jsonobj["hospitalname"]?.ToString(),
                productName = jsonobj["productname"]?.ToString(),
                procuctID = jsonobj["productid"]?.ToString(),
                grid = jsonobj["grid"]?.ToString(),
                gridName = jsonobj["gridname"]?.ToString(),
                autherID = jsonobj["autherid"]?.ToString(),
                autherName = jsonobj["authername"]?.ToString(),
                viewerID = jsonobj["applyerid"]?.ToString(),
                viewerName = jsonobj["applyername"]?.ToString(),
                saleMode = jsonobj["salemode"]?.ToString(),
                saleModeName = jsonobj["salemodename"]?.ToString(),
                checkState = jsonobj["checkstate"]?.ToString();
            DataTable dataTable = new();
            SQLServerHelper serverHelper = new();
            //提交直接保存
            if (string.IsNullOrEmpty(formID))
            {
                return Save(myEmployeeID, hospitalID, hospitalName, procuctID, productName, grid, gridName, autherID, autherName, viewerID, viewerName, saleMode, saleModeName);
            }
            else
            {

                for (int j = 0; j < 2; j++)
                {
                    //提取审核和审批表
                    List<string> hospitalIDList = new List<string>();
                    List<string> hospitalNameList = new List<string>();
                    List<string> productNameList = new List<string>();
                    List<string> procuctIDList = new List<string>();
                    List<string> gridList = new List<string>();
                    List<string> gridNameList = new List<string>();
                    List<string> autherIDList = new List<string>();
                    List<string> autherNameList = new List<string>();
                    List<string> viewerIDList = new List<string>();
                    List<string> viewerNameList = new List<string>();
                    List<string> saleModeList = new List<string>();
                    List<string> saleModeNameList = new List<string>();
                    List<string> formIDList = new List<string>();
                    List<string> checkStateList = new List<string>();
                    string sql;

                    for (int i = 0; i < formID.Split('|').Length; i++)
                    {
                        //*************************必须过滤前端传过来多余的|***********************
                        if (string.IsNullOrEmpty(formID.Split('|')[i]))
                        {
                            continue;
                        }

                        sql = $"Select *  From [yaodaibao].[dbo].[AuthStating] where FormID = '{formID.Split('|')[i]}' and AuthState = '{j}'";
                        dataTable = serverHelper.ExecuteSql(sql);

                        foreach (DataRow row in dataTable.Rows)
                        {
                            hospitalIDList.Add(row["HospitalID"].ToString());
                            hospitalNameList.Add(row["HospitalName"].ToString());
                            productNameList.Add(row["ProductName"].ToString());
                            procuctIDList.Add(row["ProductID"].ToString());
                            gridList.Add(row["GridID"].ToString());
                            gridNameList.Add(row["GridName"].ToString());
                            autherIDList.Add(row["AutherID"].ToString());
                            autherNameList.Add(row["AutherName"].ToString());
                            viewerIDList.Add(row["ViewerID"].ToString());
                            viewerNameList.Add(row["ViewerName"].ToString());
                            saleModeList.Add(row["SaleMode"].ToString());
                            saleModeNameList.Add(row["SaleModeName"].ToString());
                        }
                        hospitalID = string.Join('|', hospitalIDList.ToArray());
                        string temo = formID.Split('|')[i];
                        hospitalName = string.Join('|', hospitalNameList.ToArray());
                        temo = formID.Split("|")[i];
                        productName = string.Join('|', productNameList.ToArray());
                        procuctID = string.Join('|', procuctIDList.ToArray());
                        grid = string.Join('|', gridList.ToArray());
                        gridName = string.Join('|', gridNameList.ToArray());
                        autherID = string.Join('|', autherIDList.ToArray());
                        autherName = string.Join('|', autherNameList.ToArray());
                        viewerID = string.Join('|', viewerIDList.ToArray());
                        viewerName = string.Join('|', viewerNameList.ToArray());
                        saleMode = string.Join('|', saleModeList.ToArray());
                        saleModeName = string.Join('|', saleModeNameList.ToArray());

                        if (hospitalIDList.Count == 0)
                        {
                            continue;
                        }
                        checkStateList.Add(checkState.Split('|')[i]);
                        formIDList.Add(formID.Split('|')[i]);
                    }
                    //审核
                    if (j == 0)
                    {
                        //如果下一个审核人是自己 ， 审批结束
                        sql = $"SELECT FormID FROM yaodaibao.dbo.AuthStating WHERE NextAuthID = '{myEmployeeID}'";
                        dataTable = serverHelper.ExecuteSql(sql);
                        if (dataTable.Rows.Count == 0)
                        {
                            Approve(myEmployeeID, string.Join("|", formIDList.ToArray()), hospitalID, procuctID, productName, autherID, viewerID, grid, string.Join("|", checkStateList.ToArray()));
                            continue;
                        }
                        sql = $@"SELECT t2.FParentID 
                               From yaodaibao.dbo.t_Departments t1 Left Join yaodaibao.dbo.t_Items t2 On t1.FID = t2.FID
                               Left Join yaodaibao.dbo.t_Items t4 On t4.FID = t2.FParentID
                               WHERE t1.FID IN(SELECT te1.FDeptID FROM yaodaibao.dbo.t_Employees te1 WHERE te1.FID = '{myEmployeeID}')";
                        dataTable = serverHelper.ExecuteSql(sql);
                        //没有父级节点了， 审批结束
                        if (dataTable.Rows[0]["FParentID"].ToString() == "-1")
                        {
                            Approve(myEmployeeID, string.Join("|", formIDList.ToArray()), hospitalID, procuctID, productName, autherID, viewerID, grid, string.Join("|", checkStateList.ToArray()));
                        }
                        else
                        {
                            Audit(myEmployeeID, string.Join("|", formIDList.ToArray()), string.Join("|", checkStateList.ToArray()));
                        }
                    }
                    //审批
                    else if (j == 1)
                    {
                        Approve(myEmployeeID, string.Join("|", formIDList.ToArray()), hospitalID, procuctID, productName, autherID, viewerID, grid, string.Join("|", checkStateList.ToArray()));
                    }
                }


            }
            return new ResponseModel();
        }
        /// <summary>    
        ///  提交直接保存
        /// </summary>
        /// <param name="myEmployeeID">登陆者提交ID</param>
        /// <param name="hospitalID"></param>
        /// <param name="hospitalName"></param>
        /// <param name="ProcuctID"></param>
        /// <param name="ProductName"></param>
        /// <param name="grid"></param>
        /// <param name="gridName"></param>
        /// <param name="autherID"></param>
        /// <param name="autherName"></param>
        /// <param name="viewerID"></param>
        /// <param name="viewerName"></param>
        /// <param name="saleMode"></param>
        /// <param name="saleModeName"></param>
        /// <param name="checkState"></param>
        /// <returns></returns>
        public ResponseModel Save(string myEmployeeID, string hospitalID, string hospitalName, string ProcuctID, string ProductName, string grid, string gridName, string autherID, string autherName, string viewerID, string viewerName, string saleMode, string saleModeName)
        {
            SQLServerHelper serverHelper = new();

            string sql = "";
            //下一个审批人节点ID
            string nextID = "";



            DataTable dataTable = new();
            sql = $"SELECT  FSupervisorID FROM yaodaibao.dbo.t_Departments td WHERE td.FSupervisorID ='{myEmployeeID}'";
            dataTable = serverHelper.ExecuteSql(sql);

            //是部门管理 让下一个部门管理审批
            if (dataTable.Rows.Count != 0)
            {
                sql = $@"SELECT isnull(td.FSupervisorID,'') authid FROM (SELECT t2.FParentID
               From yaodaibao.dbo.t_Departments t1 Left Join yaodaibao.dbo.t_Items t2 On t1.FID = t2.FID
               Left Join yaodaibao.dbo.t_Items t4 On t4.FID = t2.FParentID
               WHERE t1.FID IN(SELECT te1.FDeptID FROM yaodaibao.dbo.t_Employees te1 WHERE te1.FID = '{myEmployeeID}')) pt LEFT JOIN  yaodaibao.dbo.t_Departments td    ON td.FID = pt.FParentID";
                dataTable = serverHelper.ExecuteSql(sql);
                //没有父级部门，到自己结束
                if (dataTable.Rows.Count == 0)
                {
                    nextID = myEmployeeID;
                }
                else
                {
                    if (string.IsNullOrEmpty(dataTable.Rows[0]["authid"].ToString()))
                    {
                        nextID = myEmployeeID;
                    }
                    else
                    {
                        nextID = dataTable.Rows[0]["authid"].ToString();
                    }
                }
            }
            //如果不是部门管理，查找部门管理人员
            else
            {
                sql = $@"SELECT td.FSupervisorID authId FROM yaodaibao.dbo.t_Departments td LEFT JOIN yaodaibao.dbo.t_Employees  te ON td.FID = te.FDeptID WHERE te.FID='{myEmployeeID}'";
                dataTable = serverHelper.ExecuteSql(sql);
                nextID = dataTable.Rows[0]["authId"].ToString();
            }
            //不插入相同的医院+责任人 
            for (int i = 0; i < hospitalID.Split("|").Length; i++)
            {
                string fromid = Guid.NewGuid().ToString();
                //先根据医院和责任人判断是否有数据变化
                sql = $"select * from [yaodaibao].[dbo].[AuthStating] where HospitalID='{hospitalID.Split('|')[i]}'  and AutherID = '{autherID.Split('|')[i]}'";
                dataTable = serverHelper.ExecuteSql(sql);
                if (dataTable.Rows.Count > 0)
                {
                    //判断是否有相同的授权人，产品
                    foreach (DataRow row in dataTable.Rows)
                    {
                        for (int j = 0; j < viewerID.Split('|')[i].Split(",").Length; j++)
                        {
                            //新增授权人
                            if (!row["ViewerID"].ToString().Contains(viewerID.Split('|')[i].Split(",")[j]))
                            {
                                sql = $"update [yaodaibao].[dbo].[AuthStating]  set ViewerID = '{(string.IsNullOrEmpty(row["ViewerID"].ToString() ?? "") ? "" : row["ViewerID"] + ",") + viewerID.Split('|')[i].Split(",")[j]}' , ViewerName = '{(string.IsNullOrEmpty(row["ViewerName"].ToString() ?? "") ? "" : row["ViewerName"] + ",") + viewerName.Split('|')[i].Split(",")[j] }' , AuthState = '0' where FormID='{row["FormID"]}'";
                                serverHelper.ExecuteSqlNone(sql);
                            }
                        }
                        for (int j = 0; j < ProcuctID.Split('|')[i].Split(",").Length; j++)
                        {
                            //新增产品
                            if (!row["ProductID"].ToString().Contains(ProcuctID.Split('|')[i]))
                            {
                                sql = $"update [yaodaibao].[dbo].[AuthStating]  set ProductID = '{row["ProductID"] + "," + ProcuctID.Split('|')[i]}' , ProductName = '{row["ProductName"] + "," + ProductName.Split('|')[i] }' , AuthState = '0' where FormID='{row["FormID"]}'";
                                serverHelper.ExecuteSqlNone(sql);
                            }
                        }
                    }
                }
                else
                {
                    sql = $"insert into [yaodaibao].[dbo].[AuthStating]([EmployeeID],[FormID],[HospitalID],[HospitalName],[ProductID],[ProductName],[AutherID],[AutherName],[GridID],[GridName],[ViewerID],[ViewerName],[SaleMode],[SaleModeName],[NextAuthID],[AuthState]) values('{myEmployeeID}','{fromid}','{hospitalID.Split('|')[i] ?? ""}','{hospitalName.Split('|')[i] ?? ""}','{ProcuctID.Split('|')[i] ?? ""}','{ProductName.Split('|')[i] ?? ""}','{autherID.Split('|')[i] ?? ""}','{autherName.Split('|')[i] ?? ""}','{grid.Split('|')[i] ?? ""}','{gridName.Split('|')[i] ?? ""}','{viewerID.Split('|')[i] ?? ""}','{viewerName.Split('|')[i] ?? ""}','{saleMode.Split('|')[i] ?? ""}','{saleModeName.Split('|')[i] ?? ""}','{nextID}','0')";
                    serverHelper.ExecuteSqlNone(sql);
                }


            }
            return new ResponseModel();
        }
        /// <summary>
        /// 审核
        /// </summary>
        /// <returns></returns>
        public ResponseModel Audit(string myEmployeeID, string formID, string checkState)
        {
            SQLServerHelper serverHelper = new();
            string sql = $@"SELECT td.FSupervisorID authid FROM (SELECT t2.FParentID
               From yaodaibao.dbo.t_Departments t1 Left Join yaodaibao.dbo.t_Items t2 On t1.FID = t2.FID
               Left Join yaodaibao.dbo.t_Items t4 On t4.FID = t2.FParentID
               WHERE t1.FID IN(SELECT te1.FDeptID FROM yaodaibao.dbo.t_Employees te1 WHERE te1.FID = '{myEmployeeID}')) pt LEFT JOIN  yaodaibao.dbo.t_Departments td    ON td.FID = pt.FParentID";
            DataTable dataTable = new();
            dataTable = serverHelper.ExecuteSql(sql);
            string nextID = "";
            //没有父级部门，到自己结束
            if (dataTable.Rows.Count == 0)
            {
                nextID = myEmployeeID;
            }
            else
            {
                if (string.IsNullOrEmpty(dataTable.Rows[0]["authid"].ToString()))
                {
                    nextID = myEmployeeID;

                }
                else
                {
                    nextID = dataTable.Rows[0]["authid"].ToString();
                }

            }

            for (int i = 0; i < formID.Split('|').Length; i++)
            {
                //不同意
                if (checkState.Split("|")[i] == "b")
                {
                    serverHelper.ExecuteSqlNone($"update [yaodaibao].[dbo].[AuthStating] set [AuthState] = '-1',[ModifyTime] = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',[NextAuthID] ='{nextID}',CurrentAuthID='{myEmployeeID}' where formID = '{formID.Split('|')[i]}'");
                }
                else
                {
                    serverHelper.ExecuteSqlNone($"update [yaodaibao].[dbo].[AuthStating] set [AuthState] = '1',[ModifyTime] = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',[NextAuthID] ='{nextID}',CurrentAuthID='{myEmployeeID}' where formID = '{formID.Split('|')[i]}'");
                }

            }

            return new ResponseModel();
        }
        /// <summary>
        /// 审批
        /// </summary>
        /// <returns></returns>
        public ResponseModel Approve(string myEmployeeID, string formID, string hospitalID, string productID, string productName, string autherID, string viewerID, string gridID, string checkState)
        {


            SQLServerHelper serverHelper = new();
            string sql = "";
            //开启事务
            //serverHelper.BeginTran();
            DataTable dataTable = new();
            //string hosInstitutionID = "";
            //string productID = "";
            //string employeeID = "";
            string hosName = "";
            string hosCode = "";
            string employeeCode = "";

            for (int i = 0; i < formID.Split('|').Length; i++)
            {

                try
                {
                    //不同意
                    if (checkState.Split('|')[i] == "b")
                    {
                        serverHelper.ExecuteSqlNone($"update [yaodaibao].[dbo].[AuthStating] set [AuthState] = '-1',[ModifyTime] = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where formID = '{formID.Split('|')[i]}'");
                        continue;
                    }
                    List<string> transList = new List<string>();

                    //先生成OA经营授权表的ID
                    byte[] gb = Guid.NewGuid().ToByteArray();
                    long id = BitConverter.ToInt64(gb, 0);
                    #region  产品-责任人,医院-产品授权,多产品时要求插入多条记录
                    for (int j = 0; j < productID.Split('|')[i].Split(',').Length; j++)
                    {
                        sql = $"SELECT  ti.FName hosName ,ti.FNumber hosCode FROM yaodaibao.dbo.t_Items ti WHERE ti.FID ='{hospitalID.Split('|')[i]}'";
                        dataTable = serverHelper.ExecuteTransSql(sql);
                        if (dataTable.Rows.Count == 0)
                        {
                            break;
                            throw new Exception("没有此医院");
                        }
                        hosName = dataTable.Rows[0]["hosName"].ToString();
                        hosCode = dataTable.Rows[0]["hosCode"].ToString();
                        string gid = gridID.Split('|')[i] ?? "";
                        string productID1 = productID.Split('|')[i].Split(',')[j];
                        string productName2 = productName.Split('|')[i].Split(',')[j];

                        sql = $"SELECT * FROM   yaodaibao.dbo.formmain_8862 WHERE field0001='{gridID.Split('|')[i] ?? ""}' and field0004='{hosCode}' and field0012='{hosName}'    and    field0014='{productID.Split('|')[i]?.Split(',')[j] ?? ""}'  and   field0015='{productName.Split('|')[i]?.Split(',')[j] ?? ""}'";
                        dataTable = serverHelper.ExecuteTransSql(sql);
                        //没有记录
                        if (dataTable.Rows.Count == 0)
                        {
                            string temp = productID.Split('|')[i].Split(',')[j];
                            //先插入医院网格授权表
                            sql = $"INSERT INTO yaodaibao.dbo.formmain_8862(field0001,field0004,field0012,field0014,field0015 ) VALUES('{gridID.Split('|')[i] ?? ""}','{hosCode}','{hosName}','{productID.Split('|')[i]?.Split(',')[j] ?? ""}','{productName.Split('|')[i]?.Split(',')[j] ?? ""}')";
                            //放进事务操作
                            transList.Add(sql);
                            //serverHelper.ExecuteTransSqlNone(sql);
                        }
                        sql = $"Select FID from yaodaibao.dbo.AuthDatas Where FInstitutionID='{hospitalID.Split('|')[i]}' and FDeleted=0 and FAuthObjectID='{productID.Split('|')[i].Split(',')[j]}'";
                        dataTable = serverHelper.ExecuteTransSql(sql);
                        if (dataTable.Rows.Count == 0)
                        {
                            //医院-产品
                            sql = $"Insert into yaodaibao.dbo.AuthDatas(FCompanyID,FInstitutionID,FInstitutionType,FAuthObjectID,FAuthType,FBeginDate) Values('-1','{productID.Split('|')[i].Split(',')[j]}','1','{hospitalID.Split('|')[i]}','1','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')";
                            transList.Add(sql);
                            //serverHelper.ExecuteTransSqlNone(sql);
                        }
                    }
                    #endregion

                    #region 医院-责任人授权
                    //该院是否已授权于其他代理商？
                    sql = $"Select FModeID From yaodaibao.dbo.t_Hospital Where FIsdeleted =0 And FID= '{hospitalID.Split('|')[i]}'";
                    dataTable = serverHelper.ExecuteTransSql(sql);
                    if (dataTable.Rows.Count == 0)
                    {
                        break;
                        throw new Exception("拟授权的医院不存在");
                    }
                    else
                    {
                        sql = $"Select FID from yaodaibao.dbo.AuthDatas Where FInstitutionID='{autherID.Split('|')[i]}' and FDeleted=0 and FAuthObjectID='{hospitalID.Split('|')[i]}'";
                        dataTable = serverHelper.ExecuteTransSql(sql);
                        if (dataTable.Rows.Count == 0)//授权
                        {
                            sql = $"Insert into yaodaibao.dbo.AuthDatas(FCompanyID,FInstitutionID,FInstitutionType,FAuthObjectID,FAuthType,FBeginDate) Values('-1','{hospitalID.Split('|')[i]}','1','{autherID.Split('|')[i]}','4','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')";
                            transList.Add(sql);
                            //serverHelper.ExecuteTransSqlNone(sql);
                        }
                    }

                    #endregion

                    #region 回写OA经营授权表，责任人-授权人 调用webservice

                    //OWLAppServiceSoapClient authService = new OWLAppServiceSoapClient(OWLAppServiceSoapClient.EndpointConfiguration.OWLAppServiceSoap);
                    //authService.SaveAuthDataAsync("");
                    sql = $"select FNumber from yaodaibao.dbo.t_items where FID='{autherID.Split('|')[i]}'";
                    dataTable = serverHelper.ExecuteTransSql(sql);
                    if (dataTable.Rows.Count == 0)
                    {
                        break;
                        throw new Exception("审批失败，没有此责任人");
                    }
                    #region 调用接口数据

                    employeeCode = dataTable.Rows[0]["FNumber"].ToString();
                    //获取责任人部门
                    dataTable = serverHelper.ExecuteSql($"SELECT  FDeptID FROM yaodaibao.dbo.t_Employees where FID = '{autherID.Split('|')[i]}'");
                    string sendOAData = $"{{\"AuthCode\":\"0uyV7aK3cvAWcDRZowdyP3AMH27pvw0dmuvqfsJy5+XO/vl6Wdi9Cg==\",\"EmployeeID\":\"{myEmployeeID}\",\"ID\":\"{id}\",\"field0001\":\"{hosCode}\",\"field0002\":\"{hosName}\",\"field0003\":\"{employeeCode}\",\"field0004\":\"{autherID.Split('|')[i]}\",\"field0005\":\"{viewerID.Split('|')[i] ?? ""}\",\"field0006\":\"医院\",\"field0012\":\"{dataTable.Rows[0]["FDeptID"] ?? ""}\"}}";
                    MyService.OWLAppServiceSoapClient appServiceSoapClient = new MyService.OWLAppServiceSoapClient(OWLAppServiceSoapClient.EndpointConfiguration.OWLAppServiceSoap);
                    string oaResult = appServiceSoapClient.SaveAuthData(sendOAData);
                    if (oaResult == "500")
                    {
                        throw new Exception("提交审批失败");
                    }
                    else
                    {
                        if (transList.Count > 0)
                        {
                            int flag = serverHelper.ExecuteSqlTran(transList);
                            //提交事务失败 OA经营授权表
                            if (flag == 0)
                            {
                                //todo:提示OA授权表删除插入的记录
                                throw new Exception("提交审批失败");
                            }
                        }

                        //审批完成
                        serverHelper.ExecuteTransSqlNone($"update [yaodaibao].[dbo].[AuthStating] set [AuthState] = '2',[ModifyTime] = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where formID = '{formID.Split('|')[i]}'");
                    }
                    #endregion
                    #endregion
                    _logger.LogInformation("回调请求Json:" + sendOAData);
                    //审批完成
                    serverHelper.ExecuteTransSqlNone($"update [yaodaibao].[dbo].[AuthStating] set [AuthState] = '2',[ModifyTime] = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where formID = '{formID.Split('|')[i]}'");
                    ////提交事务
                    //serverHelper.CommitTran();
                }
                catch (Exception ex)
                {
                    //回滚事务
                    //serverHelper.RollbackTran();
                    throw new Exception(ex.Message);
                }
            }
            return new ResponseModel();
        }


    }
}
