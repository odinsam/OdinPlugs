using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace OdinPlugs.OdinMvcCore.OdinExtensions
{
    public class MvcContext
    {
        public static IHttpContextAccessor httpContextAccessor;
        public static HttpContext GetContext()
        {
            HttpContext context = httpContextAccessor.HttpContext;
            return context;
        }

        public static T GetRequiredServices<T>()
        {
            return httpContextAccessor.HttpContext.RequestServices.GetRequiredService<T>();
        }
    }
}