using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ydb.Domain;
using ydb.Domain.Extention;
using ydb.Domain.Interface;
using ydb.Domain.Models;

namespace YRBApp.Controllers
{
    [Route("api/[controller]")]
    [MultiFormatFilter]
    public class RecordHospitalController : Controller
    {
        public IRecordHospitalService _recordHospitalService;

        public RecordHospitalController(IRecordHospitalService recordHospitalService)
        {
            _recordHospitalService = recordHospitalService;
        }

        /// <summary>
        /// 保存要备案医院
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveRecordHospital")]
        public ResponseModel SaveRecordHospital()
        {
            return _recordHospitalService.SaveRecordHospital();
        }
        /// <summary>
        /// 获取开发中备案医院
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMyRecordingHospital")]
        public ResponseModel GetRecordingHospital()
        {
            return _recordHospitalService.GetMyRecordingHospital();
        }

        /// <summary>
        /// 根据医院和产品获取备案医院
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDevelopingHospitalDetail")]
        public ResponseModel CheckRecord()
        {
            return _recordHospitalService.CheckRecord();
        }
        /// <summary>
        ///查询备案医院
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("QueryRecordHospital")]
        public ResponseModel QueryRecordHospital()
        {
            return _recordHospitalService.QueryRecordHospital();
        }

    }
}
