using Microsoft.AspNetCore.Mvc;
using ydb.Domain;
using ydb.Domain.Interface;
using ydb.Domain.Models;

namespace YRBApp.Controllers
{
    [Route("api/[controller]")]
    // [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfile;
        public UserProfileController(IUserProfileService userProfile)
        {
            _userProfile = userProfile;
        }
        [HttpPost]
        [Route("SaveUserProfile")]
        public ResponseModel SaveUserProfile([FromBody] UserProfile userProfile)
        {
            ResponseModel responseModel = _userProfile.SaveUserProfile(userProfile);
            return responseModel;
        }

        [HttpPost]
        [Route("GetUserProfile")]
        public ResponseModel GetUserProfile([FromBody] UserProfile userProfile)
        {
            ResponseModel responseModel = _userProfile.GetUserProfile(userProfile);
            return responseModel;
            //if (responseModel.DataRow.ToLower() == "false" || string.IsNullOrEmpty(responseModel.DataRow.ToLower()))
            //{
            //    return "close";
            //}
            //else
            //{
            //    return "open";
            //}
        }

        [HttpPost]
        [Route("GetAutoHis")]
        public ResponseModel GetAutoHis(RouteQuery routeQuery)
        {
            ResponseModel responseModel = _userProfile.GetAutoHis(routeQuery);
            return responseModel;
        }
    }
}
