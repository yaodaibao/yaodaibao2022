using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ydb.Domain;
using ydb.Domain.Interface;
using ydb.Domain.Models;
namespace YRBApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IJwtService _jwtService;
        public LoginController(ILoginService loginService, IJwtService jwtService)
        {
            _loginService = loginService;
            _jwtService = jwtService;
        }
        [HttpPost]
        [Route("Login")]
        public ResponseModel Login([FromBody] Login login)
        {
            ResponseModel responseModel = _loginService.Login(login);
            //刪除token功能
            //if (responseModel.Result)
            //{
            //    //为了更安全使用jwt，为了兼容药瑞宝后台同时返回authcode
            //    string token = _jwtService.GenerateToken(login.UserName, login.Password);
            //    responseModel.Description = responseModel.Description + "|" + token;
            //}
            return responseModel;
        }
        [HttpPost]
        [Route("GetRegStatusByMobile")]
        public ResponseModel GetRegStatusByMobile([FromBody] Login login)
        {
            ResponseModel responseModel = _loginService.GetRegStatusByMobile(login);

            return responseModel;
        }
        [HttpPost]
        [Route("ChangePwd")]
        public ResponseModel ChangePwd([FromBody] Login login)
        {
            ResponseModel responseModel = _loginService.ChangePwd(login);

            return responseModel;
        }
        [HttpPost]
        [Route("SetPwd")]
        public ResponseModel SetPwd([FromBody] Login login)
        {
            ResponseModel responseModel = _loginService.SetPwd(login);

            return responseModel;
        }
        [HttpPost]
        [Route("ChangePwd")]
        public ResponseModel SendVCode([FromBody] Login login)
        {
            ResponseModel responseModel = _loginService.SendVCode(login);

            return responseModel;
        }
        [HttpPost]
        [Route("CheckVCode")]
        public ResponseModel CheckVCode([FromBody] Login login)
        {
            ResponseModel responseModel = _loginService.CheckVCode(login);

            return responseModel;
        }
    }
}
