using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ydb.Domain;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;
using ydb.Domain.Extention;

namespace YRBApp.Controllers
{
    [Route("api/[controller]")]
    [MultiFormatFilter()]
    //[ApiController]

    public class CompassController : ControllerBase
    {
        public readonly IRewardDomainService _rewardDomainService;
        private readonly ILogger<CompassController> _logger;
        public CompassController(IRewardDomainService rewardDomainService, ILogger<CompassController> logger)
        {
            _logger = logger;
            _rewardDomainService = rewardDomainService;
        }

        [HttpPost]
        [Route("GetReward")]
        public async Task<ResponseModel> GetReward()
        {
            string jsonString;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                jsonString = await reader.ReadToEndAsync();
            }
            var jsonobj = JObject.Parse(jsonString);
            ResponseModel result = _rewardDomainService.GetReward(jsonobj["EmployeeID"]?.ToString(), jsonobj["startmonth"]?.ToString(), jsonobj["endmonth"]?.ToString()).GetAwaiter().GetResult();
            return result;
        }

        [HttpPost]
        [Route("GetTotal")]
        public async Task<ResponseModel> GetTotal()
        {
            string jsonString;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                jsonString = await reader.ReadToEndAsync();
            }
            var jsonobj = JObject.Parse(jsonString);
            ResponseModel result = _rewardDomainService.GetTotal(jsonobj["EmployeeID"]?.ToString()).GetAwaiter().GetResult();
            return result;
        }


        /// <summary>
        /// 获取各种产品的销量
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetSaleProducts")]
        public async Task<ResponseModel> GetSaleProducts()
        {
            string jsonString;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                jsonString = await reader.ReadToEndAsync();
            }
            var jsonobj = JObject.Parse(jsonString);
            ResponseModel result = _rewardDomainService.GetSaleProducts(jsonobj["EmployeeID"]?.ToString(), jsonobj["startmonth"]?.ToString(), jsonobj["endmonth"]?.ToString(), jsonobj["product"]?.ToString()).GetAwaiter().GetResult();
            return result;
        }
        /// <summary>
        /// 获取医院的销量
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetSalesHospitals")]
        public async Task<ResponseModel> GetSalesHospitals()
        {
            string jsonString;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                jsonString = await reader.ReadToEndAsync();
            }
            var jsonobj = JObject.Parse(jsonString);
            ResponseModel result = _rewardDomainService.GetSalesHospitals(jsonobj["EmployeeID"]?.ToString(), jsonobj["product"]?.ToString() ?? "", jsonobj["startmonth"]?.ToString(), jsonobj["endmonth"]?.ToString()).GetAwaiter().GetResult();
            return result;
        }
    }
}
