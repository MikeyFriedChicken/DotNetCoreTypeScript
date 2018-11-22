using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikeyFriedChicken.DotNetCoreTypeScript.Services;

namespace MikeyFriedChicken.DotNetCoreTypeScript.Tests
{
    [TestClass]
    public class JavaScriptServiceTests
    {

        private string SCRIPT_FOLDER_TS =  ".";  // Works for TS
        private string SCRIPT_FOLDER_JS = "./bin/Debug/netcoreapp2.1";  // Works for JS folder

        [TestMethod]
        public void CanAddTwoNumbers()
        {
            var services = new ServiceCollection();
            services.AddNodeServices(options => {
                // Set any properties that you want on 'options' here
                options.ProjectPath = "../../../";
            });
            var serviceProvider = services.BuildServiceProvider();
            var nodeServices = serviceProvider.GetRequiredService<INodeServices>();
            JavaScriptService javaScriptService = new JavaScriptService(nodeServices, SCRIPT_FOLDER_JS);
            Task<int> result = javaScriptService.AddNumbers(1, 2);
            Assert.AreEqual(3,result.Result);
        }

        [TestMethod]
        public void CanCallTypeScriptHello()
        {
            var services = new ServiceCollection();
            services.AddNodeServices(options => {
                // Set any properties that you want on 'options' here
            });
            var serviceProvider = services.BuildServiceProvider();
            var nodeServices = serviceProvider.GetRequiredService<INodeServices>();
            JavaScriptService javaScriptService = new JavaScriptService(nodeServices, SCRIPT_FOLDER_TS);
            Task<string> result = javaScriptService.Hello("Mike");
            Assert.AreEqual("Hello Mike", result.Result);
        }

        [TestMethod]
        public void CanCallTypeScriptGoodbye()
        {
            var services = new ServiceCollection();
            services.AddNodeServices(options => {
                // Set any properties that you want on 'options' here
            });
            var serviceProvider = services.BuildServiceProvider();
            var nodeServices = serviceProvider.GetRequiredService<INodeServices>();
            JavaScriptService javaScriptService = new JavaScriptService(nodeServices, SCRIPT_FOLDER_TS);
            Task<string> result = javaScriptService.Goodbye("Mike");
            Assert.AreEqual("Goodbye Mike", result.Result);
        }


        [TestMethod]
        public void CanCreatePDF()
        {
            var services = new ServiceCollection();
            services.AddNodeServices(options => {
                // Set any properties that you want on 'options' here
            });
            var serviceProvider = services.BuildServiceProvider();
            var nodeServices = serviceProvider.GetRequiredService<INodeServices>();
            JavaScriptService javaScriptService = new JavaScriptService(nodeServices, SCRIPT_FOLDER_TS);
            var result = javaScriptService.MakePDF("Hello Mike").Result;
            Assert.IsNotNull(result);

            if (File.Exists("test.pdf"))
            {
                File.Delete("test.pdf");
            }

            File.WriteAllBytes("test.pdf",result);

            Assert.IsTrue(File.Exists("test.pdf"));
        }
    }
}
