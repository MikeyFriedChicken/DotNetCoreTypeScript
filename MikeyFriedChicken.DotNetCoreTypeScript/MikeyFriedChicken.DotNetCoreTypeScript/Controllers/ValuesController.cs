using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MikeyFriedChicken.DotNetCoreTypeScript.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MikeyFriedChicken.DotNetCoreTypeScript.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IJavaScriptService _javaScriptService;

        public ValuesController(IJavaScriptService javaScriptService)
        {
            _javaScriptService = javaScriptService;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // GET api/values/addNumbers
        [HttpGet("addNumbers")]
        public async Task<IActionResult> AddNumbers()
        {
            int ret = await _javaScriptService.AddNumbers(5, 2);
            return Ok(ret);
        }

        // GET api/values/hello
        [HttpGet("hello")]
        public async Task<IActionResult> Hello()
        {
            string ret = await _javaScriptService.Hello("Michael");
            return Ok(ret);
        }

        // GET api/values/hello
        [HttpGet("goodbye")]
        public async Task<IActionResult> Goodbye()
        {
            string ret = await _javaScriptService.Goodbye("Michael");
            return Ok(ret);
        }

        // GET api/values/hello
        [HttpGet("makepdf")]
        public async Task<IActionResult> MakePDF()
        {
            JObject ret = await _javaScriptService.MakePDF("Michael");
            return Ok(ret.ToString(Formatting.Indented));
        }
    }
}
