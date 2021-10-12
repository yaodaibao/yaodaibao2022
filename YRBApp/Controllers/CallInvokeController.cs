using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YRBApp.Filters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YRBApp.Controllers
{
    [ServiceFilter(typeof(RedisCacheFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class CallInvokeController : ControllerBase
    {
        // GET: api/<CallInvokeController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CallInvokeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CallInvokeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CallInvokeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CallInvokeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
