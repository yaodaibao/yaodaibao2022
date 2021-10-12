using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YRB.Infrastructure.CustomValid;

namespace ydb.Domain.Models
{
    /// <summary>
    /// 签入签出实体类
    /// </summary>
    public class RouteDetails
    {
        public string RouteID { get; set; }
        [MyRequired(ErrorMessage: "EmployeeID 必填")]
        public string EmployeeID { get; set; }
        [MyRequired(ErrorMessage:"请填入日期")]
        public string Date { get; set; }
        public string SignInTime { get; set; }
        public string SignInLat { get; set; }
        public string SignInLng { get; set; }
        public string FType { get; set; }
        public string SignOutDate { get; set; }
        public string FSignOutTime { get; set; }
        
        public string InstitutionID { get; set; }
        public string InstitutionName { get; set; }
        public string SignInAddress { get; set; }
        public string SignOutAddress { get; set; }
        public string SignOutLat { get; set; }
        public string SignOutLng { get; set; }
        
        public string SignInPhotoPath { get; set; }
        public string FRemark { get; set; }
        public string BeginDate { get; set; }

        public string EndDate { get; set; }

    }
}

