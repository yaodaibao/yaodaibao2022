using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTR.LibCore;
using Newtonsoft.Json;
using ydb.Domain.Interface;
using ydb.Domain.Models;

namespace ydb.Domain.Service
{
    public class RecordHospitalService : IRecordHospitalService
    {
        public ResponseModel CheckRecord()
        {
            SQLServerHelper sqlServerHelper = new SQLServerHelper();
            string sql = "select  field0004 ,field0005 field008 ,field0037,field0007 ,field0036    from formson_8927";
            //DataTable dt = sqlServerHelper.ExecuteSql(sql);
            //List<RecordHospital> recordHospitalList = new List<RecordHospital>();
            //foreach (DataRow dataRow in dt.Rows)
            //{
            //    recordHospitalList.Add(new RecordHospital()
            //    {
            //        field0004 = dataRow["field0004"].ToString(),
            //        field0007 = dataRow["field0007"].ToString(),
            //        field0036 = dataRow["field0036"].ToString(),
            //        field0037 = dataRow["field0037"].ToString(),
            //        field0005 = dataRow["field0005"].ToString(),
            //        field0008 = dataRow["field0008"].ToString()
            //    });
            //}
            List<RecordHospital> recordHospitalList = new List<RecordHospital>();
            //foreach (DataRow dataRow in dt.Rows)
            //{
            //recordHospitalList.Add(new RecordHospital()
            //{
            //    field0004 = @"dataRow[""field0004""].ToString(),
            //    field0007 = dataRow[""field0007""].ToString(),
            //    field0036 = dataRow[""field0036""].ToString(),
            //    field0037 = dataRow[""field0037""].ToString(),
            //    field0005 = dataRow[""field0005""].ToString(),
            //    field0008 = dataRow[""field0008""].ToString()"
            //});
            recordHospitalList.Add(new RecordHospital()
            {
                field0004 = "医院",
                field0009 = "医院id",
                field0005 = "产品",
                field0010 = "产品编码",
                field0036 = "相关附件",
                field0008 = "潜力量",
                field0037 = "备案状态",
                field0007 = "备案人",
                fieldvalid = "生效日期",
                fieldinvalid = "失效日期",
            });
            return new ResponseModel() { DataRow = JsonConvert.SerializeObject(recordHospitalList).Replace("[", "").Replace("]", "") };
        }

        public ResponseModel GetMyRecordingHospital()
        {
            return new ResponseModel();
        }

        public ResponseModel QueryRecordHospital()
        {
            return new ResponseModel();
        }

        public ResponseModel SaveRecordHospital()
        {
            FormMain_8926 formMain8926 = new FormMain_8926();
            FormSon_8927 formSon8927 = new FormSon_8927();
            SQLServerHelper sqlServerHelper = new SQLServerHelper();
            string sql = $"insert into [dbo].[formson_8927](field0004,field0009,field0010,field0005, field0006,field0007,field0008,field0014,field0019,field0025,field0037,field0036,field0027) values()";
            sqlServerHelper.ExecuteSql(sql);
            sql = $"insert into [dbo].[formson_8927](field0012,field0001,field0002,field0003,field0016,field0017,field0021,field0023,field0029,field0031,field0032,field0033,field0035,field0039) values()";
            sqlServerHelper.ExecuteSql(sql);
            return new ResponseModel() { };
        }
    }
}
