using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ydb.Domain.Models;

namespace ydb.Domain.Interface
{
    public interface IRecordHospitalService
    {
        public ResponseModel SaveRecordHospital();
        public ResponseModel CheckRecord();
        public ResponseModel QueryRecordHospital();
        public ResponseModel GetMyRecordingHospital();

    }
}
