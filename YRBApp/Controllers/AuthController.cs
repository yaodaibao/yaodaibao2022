using iTR.LibCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ydb.Domain;
using ydb.Domain.Interface;
using ydb.Domain.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YRBApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IQueryItem _queryItem;
        public IAuthHospitalService _authHospitalService;
        public AuthController(IQueryItem queryItem, IAuthHospitalService authHospitalService)
        {
            _queryItem = queryItem;
            _authHospitalService = authHospitalService;
        }

        // GET api/<AuthInvokeController>/5
        [Route("QueryItem")]
        [HttpPost]
        public async Task<ResponseModel> QueryAuthItem()
        {
            string jsonString;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                jsonString = await reader.ReadToEndAsync();
            }
            var jsonobj = JObject.Parse(jsonString);
            ResponseModel result = _queryItem.GetItems(jsonobj["EmployeeID"]?.ToString(), jsonobj["queryType"]?.ToString(), jsonobj["Name"]?.ToString(), int.Parse(jsonobj["pageindex"]?.ToString() ?? "1"), int.Parse(jsonobj["pagesize"]?.ToString() ?? "100"));
            return result;
        }

        // POST api/<AuthInvokeController>
        //[HttpPost]
        //[Route("SaveAuthData")]
        //public async Task<ResponseModel> SaveAuthData()
        //{
        //    string jsonString;
        //    using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
        //    {
        //        jsonString = await reader.ReadToEndAsync();
        //    }
        //    var jsonobj = JObject.Parse(jsonString);
        //    ResponseModel result = _authHospitalService.SaveAuth(jsonobj);
        //    return result;
        //}
        //[HttpPost]
        //[Route("GetAuthData")]
        //public async Task<ResponseModel> GetAuthData()
        //{
        //    string jsonString;
        //    using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
        //    {
        //        jsonString = await reader.ReadToEndAsync();
        //    }
        //    var jsonobj = JObject.Parse(jsonString);
        //    ResponseModel result = _authHospitalService.GetAuthData(jsonobj["EmployeeID"]?.ToString(), "auth", jsonobj["auther"]?.ToString() ?? "", jsonobj["hospital"]?.ToString() ?? "");
        //    return result;
        //}
        //[HttpPost]
        //[Route("GetMyAuthData")]
        //public async Task<ResponseModel> GetMyAuthData()
        //{
        //    string jsonString;
        //    using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
        //    {
        //        jsonString = await reader.ReadToEndAsync();
        //    }
        //    var jsonobj = JObject.Parse(jsonString);
        //    ResponseModel result = _authHospitalService.GetAuthData(jsonobj["EmployeeID"]?.ToString(), jsonobj["savetype"]?.ToString() ?? "", jsonobj["auther"]?.ToString() ?? "", jsonobj["hospital"]?.ToString() ?? "");
        //    return result;
        //}

        [HttpPost]
        [Route("SaveAuthData")]
        public async Task<ResponseModel> GetMyAuthDataMini()
        {
            string jsonString;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                jsonString = await reader.ReadToEndAsync();
            }
            var jsonobj = JObject.Parse(jsonString);
            ResponseModel result = _authHospitalService.GetAuthData(jsonobj["EmployeeID"]?.ToString(), jsonobj["savetype"]?.ToString() ?? "", jsonobj["auther"]?.ToString() ?? "", jsonobj["hospital"]?.ToString() ?? "");
            return result;

        }

        [HttpPost]
        [Route("GetAuthData")]
        public async Task<ResponseModel> GetAuthDataMini()
        {
            string jsonString;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                jsonString = await reader.ReadToEndAsync();
            }
            var jsonobj = JObject.Parse(jsonString);
            ResponseModel result = _authHospitalService.GetAuthData(jsonobj["EmployeeID"]?.ToString(), "auth", jsonobj["auther"]?.ToString() ?? "", jsonobj["hospital"]?.ToString() ?? "");
            return result;
        }

        [HttpPost]
        [Route("SaveAuthData")]
        public async Task<ResponseModel> SaveAuthDataMini()
        {
            string jsonString;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                jsonString = await reader.ReadToEndAsync();
            }
            var jsonobj = JObject.Parse(jsonString);
            ResponseModel result = _authHospitalService.SaveAuth(jsonobj);
            return result;
        }
    }
}
