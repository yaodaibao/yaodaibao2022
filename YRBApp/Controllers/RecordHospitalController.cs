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
        public ResponseModel SaveRecordHospital([FromBody]SaveRecordHospital saveRecordHospital)
        {
            return _recordHospitalService.SaveRecordHospital(saveRecordHospital);
        }
        /// <summary>
        /// 获取开发中备案医院
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetMyRecordingHospital")]
        public ResponseModel GetRecordingHospital([FromBody] QueryRecordModel queryRecordModel)
        {
            return _recordHospitalService.GetMyRecordingHospital(queryRecordModel);
        }

        /// <summary>
        /// 根据医院和产品获取备案医院
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDevelopingHospitalDetail")]
        public ResponseModel CheckRecord([FromBody] QueryRecordModel queryRecordModel)
        {
            return _recordHospitalService.CheckRecord(queryRecordModel);
        }
        /// <summary>
        ///查询备案医院
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("QueryRecordHospital")]
        public ResponseModel QueryRecordHospital([FromBody] QueryRecordModel queryRecordModel)
        {
            return _recordHospitalService.QueryRecordHospital(queryRecordModel);
        }
        

        /// <summary>
        /// 编辑备案医院
        /// </summary>
        /// <param name="queryRecordModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EditRecordHospital")]
        public ResponseModel EditRecordHospital([FromBody] QueryRecordModel queryRecordModel)
        {
            return _recordHospitalService.EditRecordHospital(queryRecordModel);
        }
    }
}
