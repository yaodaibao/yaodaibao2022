using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ydb.Domain;
using ydb.Domain.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YRBApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        public readonly IAuthHospitalService _authHospital;

        public ReportController(IAuthHospitalService authHospital)
        {
            _authHospital = authHospital;
        }
 
        [HttpPost("GetMenuList")]
        public string GetMenuList()
        {

 
            return "value5";
        }

        // POST api/<ReportDataInvokeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ReportDataInvokeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ReportDataInvokeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
