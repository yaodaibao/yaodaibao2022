using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ydb.Domain.Interface;
using iTR.LibCore;
using ydb.Domain.Models;
using ydb.Domain.Interface;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;
using ydb.Domain.Extention;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YRBApp.Controllers
{
    [Route("api/[controller]")]
    [AuthCheck()]
    // [ApiController]
    public class OAHelperInvokeController : ControllerBase
    {
        public readonly IAuthHospitalService _authDataDomainService;
        private readonly ILogger<CompassController> _logger;
        public OAHelperInvokeController(IAuthHospitalService authDataDomainService, ILogger<CompassController> logger)
        {
            _logger = logger;
            _authDataDomainService = authDataDomainService;
        }

        // GET: api/<OAHelperInvokeController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<OAHelperInvokeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<OAHelperInvokeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<OAHelperInvokeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OAHelperInvokeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost]
        [Route("GetYRBAuthData")]
        //王天池，2021-1-09
        public async Task<string> GetYRBAuthData()
        {
            string xmlString = "", result = "";

            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                xmlString = await reader.ReadToEndAsync();
            }

            result = _authDataDomainService.GetYRBAuthData(xmlString);

            return result;
        }
    }
}
