using System.Threading.Tasks;

namespace MikeyFriedChicken.DotNetCoreTypeScript.Services
{
    public interface IJavaScriptService
    {
        Task<int> AddNumbers(int x, int y);
        Task<string> Hello(string name);
        Task<string> Goodbye(string name);
        Task<byte[]> MakePDF(string name);
    }
}