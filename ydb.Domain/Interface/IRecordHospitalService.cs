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
        public ResponseModel SaveRecordHospital(SaveRecordHospital saveRecordHospital);
        public ResponseModel CheckRecord(QueryRecordModel recordModel);
        public ResponseModel QueryRecordHospital(QueryRecordModel recordModel);
        public ResponseModel GetMyRecordingHospital(QueryRecordModel recordModel);

        public ResponseModel EditRecordHospital(QueryRecordModel recordModel);

    }
}
