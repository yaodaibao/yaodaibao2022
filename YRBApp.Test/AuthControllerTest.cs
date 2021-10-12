using Microsoft.Extensions.Logging;
using System;
using Xunit;
using ydb.Domain.Interface;
using ydb.Domain.Service;
using YRBApp.Controllers;

namespace YRBApp.Test
{
    public class AuthControllerTest
    {
        AuthController _authController;
        IQueryItem _queryItem;
        IAuthHospitalService _authHospitalService;
        ILogger<AuthHospitalService> _logger;
        public AuthControllerTest()
        {
            _queryItem = new QueryItem();
            _authController = new AuthController(_queryItem, _authHospitalService);
        }

        [Fact]
        public void GetAuthDataTest()
        {
            var result = _authController.GetAuthData();
            Assert.Equal(1, 1);
        }

    }
}
