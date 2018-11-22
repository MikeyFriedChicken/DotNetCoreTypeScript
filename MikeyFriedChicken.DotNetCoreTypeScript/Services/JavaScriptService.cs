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
        private readonly string _scriptFolder;

        public JavaScriptService([FromServices]INodeServices nodeServices): this(nodeServices,".")
        {
  
        }

        public JavaScriptService([FromServices]INodeServices nodeServices, string scriptFolder)
        {
            _nodeServices = nodeServices;
            _scriptFolder = scriptFolder;
        }

        public async Task<int> AddNumbers(int x, int y)
        {
            string path = Path.Combine(_scriptFolder, "./scripts/addNumbers");
            var result = await _nodeServices.InvokeAsync<int>(path, x, y);
            return result;
        }

        public async Task<string> Hello(string name)
        {
            string path = Path.Combine(_scriptFolder, "./scripts/hello");
            var result = await _nodeServices.InvokeAsync<string>(path, name);
            return result;
        }

        public async Task<string> Goodbye(string name)
        {
            string path = Path.Combine(_scriptFolder, "./scripts/goodbye");
            var result = await _nodeServices.InvokeAsync<string>(path, name);
            return result;
        }

        public async Task<byte[]> MakePDF(string name)
        {
            string path = Path.Combine(_scriptFolder, "./scripts/makePDF");
            JObject result = await _nodeServices.InvokeAsync<JObject>(path, name);
            JValue data = (JValue)result["data"];
            byte[] decodedFromBase64 = Convert.FromBase64String(data.Value.ToString());
            File.WriteAllBytes("test.pdf", decodedFromBase64);
            return decodedFromBase64;
        }
    }
}
