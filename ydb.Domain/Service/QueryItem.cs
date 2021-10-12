using iTR.LibCore;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ydb.Domain.Interface;

namespace ydb.Domain.Service
{
    /// <summary>
    /// 查询简单的枚举值
    /// </summary>
    public class QueryItem : IQueryItem
    {
        public ResponseModel GetItems(string employeeID, string itemType, string queryValue="",int pageIndex=1,int pageSize=100)
        {
            SQLServerHelper sqlServer = new();
            string sql = "";
            string pageTotal = "0";
            string amount = "0";
            
            DataTable dataTable = new DataTable();
            //获取医院
            if (itemType == "1")
            {
                if (!string.IsNullOrEmpty(queryValue))
                {
                    queryValue = $"where t2.FName like '%{queryValue}%'";
                }
                sql = "select count(*) amount From yaodaibao.dbo.t_Hospital";
                dataTable = sqlServer.ExecuteSql(sql);
                amount = dataTable.Rows[0]["amount"].ToString() ?? "";
                pageTotal = Math.Ceiling(double.Parse(amount) / pageSize).ToString();
                sql = $@"Select t1.FID id,t2.FName name,t2.FNumber,t2.FFullNumber,t2.FClassID,t2.FLevel,t2.FDescription,t2.FLevel,t2.FIsDetail,
                         Isnull(t3.FName, '') As FGrandName, Isnull(t4.FName, '') As FProvinceName, Isnull(t5.FName, '') As FCityName, Isnull(t6.FName, '') As FCountryName,
                                Isnull(t7.FName, '') As FTownName, Isnull(t8.FName, '') As FRevenueLevelName, Isnull(t9.FName, '') As FModeName
                         From yaodaibao.dbo.t_Hospital t1
                         Left Join yaodaibao.dbo.t_Items t2 On t1.FID = t2.FID
                         Left Join yaodaibao.dbo.t_Items t3 On t1.FGrandID = t3.FID
                         Left Join yaodaibao.dbo.t_Items t4 On t1.FProvinceID = t4.FID
                         Left Join yaodaibao.dbo.t_Items t5 On t1.FCityID = t5.FID
                         Left Join yaodaibao.dbo.t_Items t6 On t1.FCountryID = t6.FID
                         Left Join yaodaibao.dbo.t_Items t7 On t1.FTownID = t7.FID
                         Left Join yaodaibao.dbo.t_Items t8 On t1.FRevenueLevelID = t8.FID
                         Left Join yaodaibao.dbo.t_Items t9 On t1.FModeID = t9.FID  {queryValue??""}  order by t1.FSortIndex Asc  offset {(pageIndex - 1 )*100} row fetch next {pageSize} row only ";
            }
            //获取产品
            else if (itemType == "2")
            {
                sql = "select count(*) amount From yaodaibao.dbo.t_Products";
                dataTable = sqlServer.ExecuteSql(sql);
                amount = dataTable.Rows[0]["amount"].ToString() ?? "";
                pageTotal = Math.Ceiling(double.Parse(amount) / pageSize).ToString();
                if (!string.IsNullOrEmpty(queryValue))
                {
                    queryValue = $"  WHERE ti.FName LIKE '%{queryValue}%'";
                }
                sql = $"SELECT ti.FID id , ti.FName name FROM yaodaibao.dbo.t_Products tp LEFT JOIN yaodaibao.dbo.t_Items ti ON tp.FID = ti.FID       {queryValue}  order by ti.FName Asc offset {(pageIndex - 1) * 100} row fetch next {pageSize} row only ";
            }
            //获取责任人的网格编码
            else if (itemType == "5")
            {
                // sql = $"SELECT  field0001  id,field0002 name FROM [yaodaibao].[dbo].[formmain_8861] where field0003 = '{employeeID}'";
                sql = $"select t2.FNumber id, t2.FName name from yaodaibao.dbo.t_Departments t1 left join  yaodaibao.dbo.t_Items  t2 on t1.FID = t2.FID where t1.FID=(select FDeptID from yaodaibao.dbo.t_Employees where  FID = '{employeeID}')";
            }
            //获取责任人及其直接下属
            else if (itemType == "3")
            {
                sql = $"SELECT td.FID FROM yaodaibao.dbo.t_Departments td WHERE td.FSupervisorID = '{employeeID}'";
                dataTable = sqlServer.ExecuteSql(sql);
                if (dataTable.Rows.Count == 0)
                {
                    sql = $"    SELECT ti.FID id,ti.FName name FROM yaodaibao.dbo.t_Employees te LEFT JOIN yaodaibao.dbo.t_Items ti ON te.FID = ti.FID WHERE    ti.FID ='{employeeID}' ";
                }
                else
                {
                    sql = $"SELECT ti.FID id,ti.FName name FROM  yaodaibao.dbo.t_Employees te LEFT JOIN yaodaibao.dbo.t_Items ti ON te.FID = ti.FID WHERE te.FDeptID IN(SELECT td.FID FROM yaodaibao.dbo.t_Departments td WHERE td.FSupervisorID = '{employeeID}') ";
                }
            }
            //获取授权人直接上级的所有下属
            else if (itemType == "4")
            {

                //不过滤
                //SELECT ti.FID id, ti.FName name FROM yaodaibao.dbo.t_Employees te LEFT JOIN yaodaibao.dbo.t_Items ti ON te.FID = ti.FID WHERE te.FDeptID IN(SELECT t2.FParentID
                //From yaodaibao.dbo.t_Departments t1 Left Join yaodaibao.dbo.t_Items t2 On t1.FID = t2.FID

                //Left Join yaodaibao.dbo.t_Items t4 On t4.FID = t2.FParentID WHERE t1.FID IN(SELECT te1.FDeptID FROM yaodaibao.dbo.t_Employees te1 WHERE te1.FID = '3855445455166195066'))
                sql = $"SELECT ti.FID id,ti.FName name FROM yaodaibao.dbo.t_Employees te LEFT JOIN yaodaibao.dbo.t_Items ti ON te.FID = ti.FID";
            }
            //获取销售模式
            else if (itemType == "6")
            {
                sql = $"SELECT  ModeID id,Name  name FROM [yaodaibao].[dbo].[SaleMode]";
            }
            dataTable = sqlServer.ExecuteSql(sql);
            List<string> list = new();
            foreach (DataRow item in dataTable.Rows)
            {
                list.Add($"{{\"name\":\"{item["name"]}\",\"id\":\"{item["id"]}\",\"pageindex\":\"{pageIndex}\",\"pagetotal\":\"{pageTotal}\"}}");
            }
            return new ResponseModel { DataRow = string.Join(',', list.ToArray()) };
        }
    }
}
