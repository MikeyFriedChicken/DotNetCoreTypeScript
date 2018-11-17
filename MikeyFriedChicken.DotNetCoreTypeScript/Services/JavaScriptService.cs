using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using Newtonsoft.Json.Linq;

namespace MikeyFriedChicken.DotNetCoreTypeScript.Services
{
    public class JavaScriptService:IJavaScriptService
    {
        private readonly INodeServices _nodeServices;

        public JavaScriptService([FromServices]INodeServices nodeServices)
        {
            _nodeServices = nodeServices;
        }

        public async Task<int> AddNumbers(int x, int y)
        {
            var result = await _nodeServices.InvokeAsync<int>("./scripts/addNumbers", x, y);
            return result;
        }

        public async Task<string> Hello(string name)
        {

            var result = await _nodeServices.InvokeAsync<string>("./scripts/hello", name);
            return result;
        }

        public async Task<string> Goodbye(string name)
        {

            var result = await _nodeServices.InvokeAsync<string>("./scripts/goodbye", name);
            return result;
        }

        public async Task<JObject> MakePDF(string name)
        {

            JObject result = await _nodeServices.InvokeAsync<JObject>("./scripts/makePDF", name);
            JValue data = (JValue)result["data"];
            byte[] decodedFromBase64 = Convert.FromBase64String(data.Value.ToString());
            File.WriteAllBytes("test.pdf", decodedFromBase64);
            return result;
        }
    }

    public interface IJavaScriptService
    {
        Task<int> AddNumbers(int x, int y);
        Task<string> Hello(string name);
        Task<string> Goodbye(string name);
        Task<JObject> MakePDF(string name);
    }
}
