using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ydb.Domain.Models
{
    public class RecordHospital
    {
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

    }
}
