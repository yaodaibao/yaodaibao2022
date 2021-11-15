using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTR.LibCore;
using Newtonsoft.Json;
using ydb.BLL;
using ydb.Domain.Interface;
using ydb.Domain.Models;

namespace ydb.Domain.Service
{
    public class RecordHospitalService : IRecordHospitalService
    {
        public ResponseModel CheckRecord(QueryRecordModel recordModel)
        {
            SQLServerHelper sqlServerHelper = new SQLServerHelper();
            List<RecordHospital> recordHospitalList = new List<RecordHospital>();
            DataTable dataTable = null;
            string sql = $"select  CONVERT(varchar(100), t1.fieldvalid, 23) fieldvalid, CONVERT(varchar(100), t1.fieldinvalid, 23) fieldinvalid,  t1.FID ,  t1.field0004 , t1.field0009 , t1.field0005 , t1.field0010 , t1.field0036 , t1.field0008 , t1.field0007 , t1.fieldbakemployee ,t1.fieldbakemployeeid ,t2.FName fieldrecordname  from [yaodaibao].[dbo].[formson_8927] t1 left join   [yaodaibao].[dbo].[t_items] t2 on t1.field0007 = t2.FID where t1.field0009='{recordModel.field0009}' and t1.field0010='{recordModel.field0010}'";
            dataTable = sqlServerHelper.ExecuteSql(sql);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    recordHospitalList.Add(new RecordHospital()
                    {
                        Id = row["FID"].ToString(),
                        field0004 = row["field0004"].ToString(),
                        field0009 = row["field0009"].ToString(),
                        field0005 = row["field0005"].ToString(),
                        field0010 = row["field0010"].ToString(),
                        field0036 = row["field0036"].ToString(),
                        field0008 = row["field0008"].ToString(),
                        field0037 = row["field0037"].ToString(),
                        field0007 = row["field0007"].ToString(),
                        fieldvalid = row["fieldvalid"].ToString(),
                        fieldinvalid = row["fieldinvalid"].ToString(),
                        fieldbakemployee = row["fieldbakemployee"].ToString(),
                        fieldrecordname = row["fieldrecordname"].ToString(),
                        fieldbakemployeeId = row["fieldbakemployeeid"].ToString()
                    });
                }
            }
            else
            {
                recordHospitalList.Add(new RecordHospital()
                {
                    Id = "",
                    field0004 = "",
                    field0009 = "",
                    field0005 = "",
                    field0010 = "",
                    field0036 = "",
                    field0008 = "",
                    field0037 = "开发中",
                    field0007 = "",
                    fieldvalid = "",
                    fieldinvalid = "",
                    fieldbakemployee = "",
                    fieldbakemployeeId = "",
                    fieldrecordname = ""
                });
            }
            return new ResponseModel() { DataRow = JsonConvert.SerializeObject(recordHospitalList).Replace("[", "").Replace("]", "") };
        }

        public ResponseModel EditRecordHospital(QueryRecordModel recordModel)
        {
            List<RecordHospital> recordHospitalList = new List<RecordHospital>();
            SQLServerHelper sqlServerHelper = new SQLServerHelper();
            DataTable dataTable = null;
            string sql = $"select  CONVERT(varchar(100), t1.fieldvalid, 23) fieldvalid, CONVERT(varchar(100), t1.fieldinvalid, 23) fieldinvalid,  t1.FID ,  t1.field0004 , t1.field0009 , t1.field0005 , t1.field0010 , t1.field0036 , t1.field0008 , t1.field0037 ,t1.field0007 , t1.fieldbakemployee ,t1.fieldbakemployeeid ,t2.FName fieldrecordname from [yaodaibao].[dbo].[formson_8927] t1 left join   [yaodaibao].[dbo].[t_items] t2 on t1.field0007 = t2.FID where t1.FID='{recordModel.Id}'";
            dataTable = sqlServerHelper.ExecuteSql(sql);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    recordHospitalList.Add(new RecordHospital()
                    {
                        Id = row["FID"].ToString(),
                        field0004 = row["field0004"].ToString(),
                        field0009 = row["field0009"].ToString(),
                        field0005 = row["field0005"].ToString(),
                        field0010 = row["field0010"].ToString(),
                        field0036 = row["field0036"].ToString(),
                        field0008 = row["field0008"].ToString(),
                        field0037 = row["field0037"].ToString(),
                        field0007 = row["field0007"].ToString(),
                        fieldvalid = row["fieldvalid"].ToString(),
                        fieldinvalid = row["fieldinvalid"].ToString(),
                        fieldbakemployee = row["fieldbakemployee"].ToString(),
                        fieldrecordname = row["fieldrecordname"].ToString(),
                        fieldbakemployeeId = row["fieldbakemployeeid"].ToString()

                    });
                }
            }
            return new ResponseModel() { DataRow = JsonConvert.SerializeObject(recordHospitalList).Replace("[", "").Replace("]", "") };
        }

        public ResponseModel GetMyRecordingHospital(QueryRecordModel recordModel)
        {
            List<RecordHospital> recordHospitalList = new List<RecordHospital>();
            SQLServerHelper sqlServerHelper = new SQLServerHelper();
            DataTable dataTable = null;
            string sql = $"select   CONVERT(varchar(100), t1.fieldvalid, 23) fieldvalid, CONVERT(varchar(100), t1.fieldinvalid, 23) fieldinvalid,  t1.FID ,  t1.field0004 , t1.field0009 , t1.field0005 , t1.field0010 , t1.field0036 , t1.field0008 , t1.field0037 , t1.field0007 , t1.fieldbakemployee ,t1.fieldbakemployeeid ,t3.FName fieldrecordname from [yaodaibao].[dbo].[formson_8927] t1 left join [yaodaibao].[dbo].[formmain_8926] t2 on t1.FID=t2.FID left join [yaodaibao].[dbo].[t_items] t3 on t1.field0007 = t3.FID where t2.field0001='{recordModel.employeeID}' and t1.field0037='开发中'";
            dataTable = sqlServerHelper.ExecuteSql(sql);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    recordHospitalList.Add(new RecordHospital()
                    {
                        Id = row["FID"].ToString(),
                        field0004 = row["field0004"].ToString(),
                        field0009 = row["field0009"].ToString(),
                        field0005 = row["field0005"].ToString(),
                        field0010 = row["field0010"].ToString(),
                        field0036 = row["field0036"].ToString(),
                        field0008 = row["field0008"].ToString(),
                        field0037 = row["field0037"].ToString(),
                        field0007 = row["field0007"].ToString(),
                        fieldvalid = row["fieldvalid"].ToString(),
                        fieldinvalid = row["fieldinvalid"].ToString(),
                        fieldbakemployee = row["fieldbakemployee"].ToString(),
                        fieldrecordname = row["fieldrecordname"].ToString(),
                        fieldbakemployeeId = row["fieldbakemployeeid"].ToString()
                    });
                }
            }
            return new ResponseModel() { DataRow = JsonConvert.SerializeObject(recordHospitalList).Replace("[", "").Replace("]", "") };
        }

        public ResponseModel QueryRecordHospital(QueryRecordModel recordModel)
        {
            List<RecordHospital> recordHospitalList = new List<RecordHospital>();
            SQLServerHelper sqlServerHelper = new SQLServerHelper();
            WorkShip workShip = new WorkShip();
            string subids = workShip.GetAllMemberIDsByLeaderID("", true).Replace("|", "','");
            DataTable dataTable = null;
            string sql = $"select  CONVERT(varchar(100), t1.fieldvalid, 23) fieldvalid, CONVERT(varchar(100), t1.fieldinvalid, 23) fieldinvalid,  t1.FID ,  t1.field0004 , t1.field0009 , t1.field0005 , t1.field0010 , t1.field0036 , t1.field0008 , t1.field0037 , t1.field0007 , t1.fieldbakemployee ,t1.fieldbakemployeeid , t3.FName fieldrecordname from [yaodaibao].[dbo].[formson_8927] t1 left join [yaodaibao].[dbo].[formmain_8926] t2 on t1.FID=t2.FID  left join [yaodaibao].[dbo].[t_items] t3 on t1.field0007 = t3.FID where t2.field0001 in('{subids}')";
            dataTable = sqlServerHelper.ExecuteSql(sql);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    recordHospitalList.Add(new RecordHospital()
                    {
                        Id = row["FID"].ToString(),
                        field0004 = row["field0004"].ToString(),
                        field0009 = row["field0009"].ToString(),
                        field0005 = row["field0005"].ToString(),
                        field0010 = row["field0010"].ToString(),
                        field0036 = row["field0036"].ToString(),
                        field0008 = row["field0008"].ToString(),
                        field0037 = row["field0037"].ToString(),
                        field0007 = row["field0007"].ToString(),
                        fieldvalid = row["fieldvalid"].ToString(),
                        fieldinvalid = row["fieldinvalid"].ToString(),
                        fieldbakemployee = row["fieldbakemployee"].ToString(),
                        fieldrecordname = row["fieldrecordname"].ToString(),
                        fieldbakemployeeId = row["fieldbakemployeeid"].ToString()
                    });
                }
            }
            return new ResponseModel() { DataRow = JsonConvert.SerializeObject(recordHospitalList).Replace("[", "").Replace("]", "") };
        }

        public ResponseModel SaveRecordHospital(SaveRecordHospital saveRecordHospital)
        {
            SQLServerHelper sqlServerHelper = new SQLServerHelper();
            string sql = "", guid = "";
            int result = 0;
            for (int i = 0; i < saveRecordHospital.field0004.Split("|").Length; i++)
            {
                guid = Guid.NewGuid().ToString();
                sql = "";
                //新增
                if (saveRecordHospital.saveState.Split("|")[i > saveRecordHospital.saveState.Split("|").Length ? saveRecordHospital.saveState.Split("|").Length : i] == "0")
                {
                    sql = $"insert into [yaodaibao].[dbo].[formmain_8926](FID,field0001) values('{guid}','{saveRecordHospital.field0001.Split("|")[i > saveRecordHospital.field0001.Split("|").Length ? saveRecordHospital.field0001.Split("|").Length : i]}')";
                    result = sqlServerHelper.ExecuteSqlNone(sql);
                    if (result > 0)
                    {
                        sql = $"insert into [yaodaibao].[dbo].[formson_8927](FID,field0004,field0009,field0010,field0005,  field0007,field0008, field0037,field0036,fieldvalid,fieldinvalid,fieldbakemployee,fieldbakemployeeid,fielddes) values('{guid}','{saveRecordHospital.field0004.Split("|")[i > saveRecordHospital.field0004.Split("|").Length ? saveRecordHospital.field0004.Split("|").Length : i]}','{saveRecordHospital.field0009.Split("|")[i > saveRecordHospital.field0009.Split("|").Length ? saveRecordHospital.field0009.Split("|").Length : i]}','{saveRecordHospital.field0010.Split("|")[i > saveRecordHospital.field0010.Split("|").Length ? saveRecordHospital.field0010.Split("|").Length : i]}','{saveRecordHospital.field0005.Split("|")[i > saveRecordHospital.field0005.Split("|").Length ? saveRecordHospital.field0005.Split("|").Length : i] }','{saveRecordHospital.field0007.Split("|")[i > saveRecordHospital.field0007.Split("|").Length ? saveRecordHospital.field0007.Split("|").Length : i]}','{saveRecordHospital.field0008.Split("|")[i > saveRecordHospital.field0008.Split("|").Length ? saveRecordHospital.field0008.Split("|").Length : i] }','{saveRecordHospital.field0037.Split("|")[i > saveRecordHospital.field0037.Split("|").Length ? saveRecordHospital.field0037.Split("|").Length : i]}','{saveRecordHospital.field0036.Split("|")[i > saveRecordHospital.field0036.Split("|").Length ? saveRecordHospital.field0036.Split("|").Length : i] }' ,'{saveRecordHospital.fieldvalid.Split("|")[i > saveRecordHospital.fieldvalid.Split("|").Length ? saveRecordHospital.fieldvalid.Split("|").Length : i] }','{saveRecordHospital.fieldinvalid.Split("|")[i > saveRecordHospital.fieldinvalid.Split("|").Length ? saveRecordHospital.fieldinvalid.Split("|").Length : i]}','{saveRecordHospital.fieldbakemployee.Split("|")[i > saveRecordHospital.fieldbakemployee.Split("|").Length ? saveRecordHospital.fieldbakemployee.Split("|").Length : i] }','{saveRecordHospital.fieldbakemployeeid.Split("|")[i > saveRecordHospital.fieldbakemployeeid.Split("|").Length ? saveRecordHospital.fieldbakemployeeid.Split("|").Length : i] }','{saveRecordHospital.fieldDes.Split("|")[i > saveRecordHospital.fieldDes.Split("|").Length ? saveRecordHospital.fieldDes.Split("|").Length : i] }')";
                        sqlServerHelper.ExecuteSql(sql);
                    }
                }
                //变更
                else
                {
                    sql = $"select t1.field0007 , t2.FName,t1.fieldbakemployee , t1.fieldbakemployeeid from [yaodaibao].[dbo].[formson_8927] t1 left join  [yaodaibao].[dbo].[t_items] on t1.field0007 = t2.FID where t1.field0009 =  '{saveRecordHospital.field0009.Split("|")[i > saveRecordHospital.field0009.Split("|").Length ? saveRecordHospital.field0009.Split("|").Length : i]}' and t1.field0010='{saveRecordHospital.field0010.Split("|")[i > saveRecordHospital.field0010.Split("|").Length ? saveRecordHospital.field0010.Split("|").Length : i]}' ";

                    DataTable dt = sqlServerHelper.ExecuteSql(sql);

                    string bakname = "", bakid = "";
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["field0007"].ToString() != saveRecordHospital.field0007.Split("|")[i > saveRecordHospital.field0007.Split("|").Length ? saveRecordHospital.field0007.Split("|").Length : i])
                        {
                            if (!string.IsNullOrEmpty(saveRecordHospital.fieldbakemployee.Split("|")[i > saveRecordHospital.fieldbakemployee.Split("|").Length ? saveRecordHospital.fieldbakemployee.Split("|").Length : i]))
                            {
                                bakname = row["fieldbakemployee"] + "," + saveRecordHospital.fieldbakemployee.Split("|")[
                                    i > saveRecordHospital.fieldbakemployee.Split("|").Length
                                        ? saveRecordHospital.fieldbakemployee.Split("|").Length
                                        : i];
                                bakid = row["fieldbakemployeeid"] + "," + saveRecordHospital.fieldbakemployee.Split("|")[
                                    i > saveRecordHospital.fieldbakemployee.Split("|").Length
                                        ? saveRecordHospital.fieldbakemployee.Split("|").Length
                                        : i];
                            }
                        }
                    }

                    sql = $"update [yaodaibao].[dbo].[formson_8927] set field0004='{saveRecordHospital.field0004.Split("|")[i > saveRecordHospital.field0004.Split("|").Length ? saveRecordHospital.field0004.Split("|").Length : i]}' , field0009='{saveRecordHospital.field0009.Split("|")[i > saveRecordHospital.field0009.Split("|").Length ? saveRecordHospital.field0009.Split("|").Length : i] }' ,  field0010='{saveRecordHospital.field0010.Split("|")[i > saveRecordHospital.field0010.Split("|").Length ? saveRecordHospital.field0010.Split("|").Length : i]}' ,  field0005='{saveRecordHospital.field0005.Split("|")[i > saveRecordHospital.field0005.Split("|").Length ? saveRecordHospital.field0005.Split("|").Length : i]}' ,  field0007='{saveRecordHospital.field0007.Split("|")[i > saveRecordHospital.field0007.Split("|").Length ? saveRecordHospital.field0007.Split("|").Length : i]}' ,  field0008='{saveRecordHospital.field0008.Split("|")[i > saveRecordHospital.field0008.Split("|").Length ? saveRecordHospital.field0008.Split("|").Length : i]}' ,  field0037='{saveRecordHospital.field0037.Split("|")[i > saveRecordHospital.field0037.Split("|").Length ? saveRecordHospital.field0037.Split("|").Length : i]}' ,  field0036='{saveRecordHospital.field0036.Split("|")[i > saveRecordHospital.field0036.Split("|").Length ? saveRecordHospital.field0036.Split("|").Length : i]}' ,  fieldvalid='{saveRecordHospital.fieldvalid.Split("|")[i > saveRecordHospital.fieldvalid.Split("|").Length ? saveRecordHospital.fieldvalid.Split("|").Length : i]}' ,  fieldinvalid='{saveRecordHospital.fieldinvalid.Split("|")[i > saveRecordHospital.fieldinvalid.Split("|").Length ? saveRecordHospital.fieldinvalid.Split("|").Length : i]}' ,  fielddes='{saveRecordHospital.fieldDes.Split("|")[i > saveRecordHospital.fieldDes.Split("|").Length ? saveRecordHospital.fieldDes.Split("|").Length : i]}' ,  fieldbakemployee='{bakname}',  fieldbakemployeeid='{bakid}' where FID='{saveRecordHospital.Id.Split("|")[i]}'";
                    sqlServerHelper.ExecuteSql(sql);
                }
            }
            return new ResponseModel() { };
        }
    }
}
