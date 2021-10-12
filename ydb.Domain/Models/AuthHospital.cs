using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ydb.Domain.Models
{
    public class AuthHospital
    {
        public string AuthCode { get; set; }
        public string EmployeeID { get; set; }
        public string Name { get; set; }
        public string PageSize { get; set; }
        public string PageIndex { get; set; }

        public string QueryType { get; set; }
        public string SaveType { get; set; }
        public string Auther { get; set; }

        public string Field0001 { get; set; }
        public string Field0002 { get; set; }
        public string Field0003 { get; set; }
        public string Field0017 { get; set; }
        public string FStatus { get; set; }
        public List<AuthDetail> AuthDetails { get; set; }


    }

    public class AuthDetail
    {
        public string Field0004 { get; set; }
        public string Field0009 { get; set; }
        public string Field0010 { get; set; }
        public string Field0005 { get; set; }
        public string Field0006 { get; set; }
        public string Field0007 { get; set; }
        public string Field0008 { get; set; }
        public string Field0019 { get; set; }
    }
}
