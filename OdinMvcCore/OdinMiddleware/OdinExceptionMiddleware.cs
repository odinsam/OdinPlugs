using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OdinPlugs.OdinCore.Models;
using OdinPlugs.OdinCore.Models.Aop;
using OdinPlugs.OdinExtensions.BasicExtensions.OdinString;
using OdinPlugs.OdinMvcCore.OdinMiddleware.Utils;

namespace OdinPlugs.OdinMvcCore.OdinMiddleware
{
    public class OdinExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment environment;
        private readonly Stopwatch stopWatch;
        public OdinExceptionMiddleware(RequestDelegate next, IWebHostEnvironment environment)
        {
            //通过注入方式获得对象
            _next = next;
            this.environment = environment;
            this.stopWatch = new Stopwatch();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                System.Console.WriteLine("=========OdinExceptionMiddleware request start==========");
                System.Console.WriteLine("=========OdinExceptionMiddleware request end==========");
                await _next(context);
                System.Console.WriteLine("=========OdinExceptionMiddleware Response start==========");
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("=========OdinExceptionMiddleware Response Exception==========");
                // var apiInvokerModel = new Aop_Invoker_Model();
                // await new OdinAopMiddlewareHelper().MiddlewareException(context, apiInvokerModel, ex);
                System.Console.WriteLine(JsonConvert.SerializeObject(ex).ToJsonFormatString());
                // context.Response.ContentType = "application/json;charset=utf-8;";
                // context.Response.StatusCode = 200;
                var stream = context.Response.Body;
                await System.Text.Json.JsonSerializer.SerializeAsync(stream, new { Name = "m ex" });
            }
        }
    }
}