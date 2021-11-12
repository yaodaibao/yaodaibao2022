using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ydb.Domain.Models
{

    public class QueryRecordModel
    {
        public string Id { get; set; } 
        public string employeeID { get; set; }
        public string field0009 { get; set; }
        public string field0010 { get; set; }
    }
    public class SaveRecordHospital
    {
        public string Id { get; set; } = "";
        //申请人
        public string field0001 { get; set; }
        //医院
        public string field0004 { get; set; }
        //医院id
        public string field0009 { get; set; }
        //产品
        public string field0005 { get; set; }
        //产品编码
        public string field0010 { get; set; }
        //生效日期
        public string fieldvalid { get; set; }
        //失效日期
        public string fieldinvalid { get; set; }
        //附件
        public string field0036 { get; set; } = "";
        //潜力
        public string field0008 { get; set; }
        //备案状态
        public string field0037 { get; set; }
        //已备案人
        public string field0007 { get; set; }
        //备案人名称
        public string fieldrecordname { get; set; }
        //候选 备案人
        public string fieldbakemployee { get; set; } = "";
        //候选 备案人ID
        public string fieldbakemployeeid { get; set; } = "";
        //变更描述
        public string fieldDes { get; set; } = "";
        public string saveState { get; set; }
    }

    public class RecordHospital
    {
        public string Id { get; set; } = "";
        //医院
        public string field0004 { get; set; }
        //医院id
        public string field0009 { get; set; }
        //产品
        public string field0005 { get; set; }
        //产品编码
        public string field0010 { get; set; }
        //生效日期
        public string fieldvalid { get; set; }
        //失效日期
        public string fieldinvalid { get; set; }
        //附件
        public string field0036 { get; set; }
        //潜力
        public string field0008 { get; set; }
        //备案状态
        public string field0037 { get; set; }
        //已备案人
        public string field0007 { get; set; }
        //备案人名称
        public string fieldrecordname { get; set; }
        //候选 备案人
        public string fieldbakemployee { get; set; }
        //候选备案人id
        public string fieldbakemployeeId { get; set; }
    }
}
