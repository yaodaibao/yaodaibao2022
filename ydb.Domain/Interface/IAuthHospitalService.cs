using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ydb.Domain.Models;

namespace ydb.Domain.Interface
{
    public interface IAuthHospitalService
    {
        public ResponseModel SaveAuth(JObject jsonobj);
        public ResponseModel GetAuthData(string employeeID, string queryID = "", string hospital = "", string auther = "");

        //public ResponseModel SaveAuthMini(AuthHospital authHospital);
        //public ResponseModel GetAuthDataMini(AuthHospital authHospital);

        public ResponseModel SaveAuthMini(JObject jsonobj);
        public ResponseModel GetAuthDataMini(string employeeID, string queryID = "", string hospital = "", string auther = "");
        //王天池,2021-10-21
        public string GetYRBAuthData(string xmlString);
    }
}
