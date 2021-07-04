using System;
using System.Dynamic;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using OdinPlugs.OdinMvcCore.OdinInject;
using OdinPlugs.OdinNetCore.OdinSnowFlake.SnowFlakeInterface;
using Serilog;

namespace OdinPlugs.OdinMAF.OdinAspectCore
{
    public class OdinAspectCoreInterceptorAttribute : AbstractInterceptorAttribute, IOdinAspectCoreInterceptorAttribute
    {
        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                System.Console.WriteLine($"=============OdinAspectCoreInterceptorAttribute  start=============");
                Log.Information($"拦截前执行");
                System.Console.WriteLine($"     odinlink: [ {context.GetHttpContext().Request.Headers["odinlink"].ToString()} ]");
                System.Console.WriteLine($"odinlink-down: [ {context.GetHttpContext().Request.Headers["odinlink-down"].ToString()} ]");
                System.Console.WriteLine($"=============OdinAspectCoreInterceptorAttribute  end=============");



                await next(context);



                System.Console.WriteLine($"=============OdinAspectCoreInterceptorAttribute  start=============");
                System.Console.WriteLine($"=============OdinAspectCoreInterceptorAttribute  end=============");

            }
            catch
            {
                Log.Information($"执行出错");
                throw;
            }
            finally
            {
                Log.Information($"拦截后执行");
                context.GetHttpContext().Response.Headers["odinlink-return"] = OdinInjectHelper.GetService<IOdinSnowFlake>().NextId().ToString();
                System.Console.WriteLine($"odinlink-down: [ {context.GetHttpContext().Response.Headers["odinlink-return"]} ]");
            }
        }
    }
}