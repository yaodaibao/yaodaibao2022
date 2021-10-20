using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Unicode;
using System.Threading.Tasks;
using ydb.BLL;
using ydb.Domain;
using ydb.Domain.Interface;
using ydb.Domain.Models;
using YRB.Infrastructure.CustomValid;
using YRBApp.Middleware;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YRBApp.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class RouteInvokeController : ControllerBase
    {

        private readonly IAutoRouteService _autoRouteService;
        public RouteInvokeController(IAutoRouteService autoRouteService)
        {
            _autoRouteService = autoRouteService;
        }

        /// <summary>
        /// 保存自动签到的数据
        /// </summary>
        /// <param name="autoRoute"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveAutoRouteData")]
        public async Task<ResponseModel> SaveAutoRouteData([FromBody] MiniLocationPoint autoRoute)
        {
            ResponseModel responseModel = await _autoRouteService.SaveAutoRouteDataAsync(autoRoute);
            return responseModel;
        }

        /// <summary>
        /// 小程序签入功能
        /// </summary>
        /// <param name="routeDetails"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SignIn")]
        public string SignIn([FromBody] RouteDetails routeDetails)
        {
            //           < SignIn >
            //< AuthCode > 1d340262 - 52e0 - 413f - b0e7 - fc6efadc2ee5 </ AuthCode >< RouteID ></ RouteID >< EmployeeID > 8536148497677042350 </ EmployeeID >< Date > 2021 - 09 - 02 </ Date >
            //   < SignInTime > 11:03:50 </ SignInTime >< InstitutionID ></ InstitutionID >< FRemark ></ FRemark >< SignInAddress > 宏汇国际广场 </ SignInAddress >< SignInLat >
            //      121.421856 </ SignInLat >< SignInLng > 31.186804 </ SignInLng >< FType > 夜访 </ FType ></ SignIn >
            var xmlstring = $"<SignIn><EmployeeID>{routeDetails.EmployeeID}</EmployeeID><Date>{routeDetails.Date}</Date><SignInTime>{routeDetails.SignInTime}</SignInTime><InstitutionID></InstitutionID><FRemark></FRemark><SignInAddress>{routeDetails.SignInAddress}</SignInAddress><SignInLat>{routeDetails.SignInLat}</SignInLat><SignInLng>{routeDetails.SignInLng}</SignInLng><FType></FType></SignIn>";

            RouteData routeData = new RouteData();
            string result = routeData.SignIn(xmlstring);
            return result;
        }
        /// <summary>
        /// 小程序签出功能
        /// </summary>
        /// <param name="routeDetails"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SignOut")]
        public string SignOut([FromBody] RouteDetails routeDetails)
        {


            string xmlString = $"<SignOut><RouteID>{routeDetails.RouteID}</RouteID><FSignOutDate>{routeDetails.SignOutDate}</FSignOutDate><FSignOutTime>{routeDetails.SignInTime}</FSignOutTime><SignOutAddress>{routeDetails.SignOutAddress}</SignOutAddress><SignOutLat>{routeDetails.SignOutLat}</SignOutLat><SignOutLng>{routeDetails.SignOutLng}</SignOutLng></SignOut>";
            var routeData = new RouteData();
            string result = routeData.SignOut(xmlString);
            return result;
        }

        /// <summary>
        /// 小程序获取签到记录
        /// </summary>
        /// <param name="routeDetails"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetRouteList")]
        public string GetRouteList([FromBody] RouteDetails routeDetails)
        {
            var xmlstring = $"<GetRouteList><BeginDate>{routeDetails.BeginDate}</BeginDate><EndDate>{routeDetails.EndDate}</EndDate><EmployeeIDs>{routeDetails.EmployeeID}</EmployeeIDs></GetRouteList>";
            var routeData = new RouteData();
            string result = routeData.GetDetail(xmlstring);
            return result;
        }
    }
}
