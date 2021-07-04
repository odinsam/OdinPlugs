using System;
using System.Dynamic;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Newtonsoft.Json;
using OdinPlugs.OdinExtensions.BasicExtensions.OdinString;
using OdinPlugs.OdinMvcCore.OdinInject;
using OdinPlugs.OdinNetCore.OdinSnowFlake.SnowFlakeInterface;
using OdinPlugs.OdinNetCore.OdinSnowFlake.Utils;
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
                System.Console.WriteLine($"class:{context.ServiceMethod.DeclaringType.Name} =========== method: {context.ServiceMethod.Name}");
                Log.Information($"拦截前执行");
                System.Console.WriteLine($"  odinlink-up: [ {context.GetHttpContext().Request.Headers["odinlink-Next"].ToString()} ]");
                // 将上游linkid作为当前起始linkid
                context.GetHttpContext().Request.Headers["odinlink-Start"] = context.GetHttpContext().Request.Headers["odinlink-Next"].ToString();
                var odinlinkStart = context.GetHttpContext().Request.Headers["odinlink-Next"].ToString();
                // 生成下游linkid
                var odinlinkNext = OdinSnowFlakeHelper.CreateSnowFlake().ToString();
                context.GetHttpContext().Request.Headers["odinlink-Next"] = odinlinkNext;
                System.Console.WriteLine($"  odinlink-down: [ {context.GetHttpContext().Request.Headers["odinlink-Next"]} ]");
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

                System.Console.WriteLine($"=============OdinAspectCoreInterceptorAttribute  return  start=============");
                System.Console.WriteLine($"class:{context.ServiceMethod.DeclaringType.Name} =========== method: {context.ServiceMethod.Name}");
                if (context.ReturnValue != null)
                {
                    System.Console.WriteLine("========= return value =========");
                    System.Console.WriteLine(JsonConvert.SerializeObject(context.ReturnValue).ToJsonFormatString());
                }
                Log.Information($"拦截后执行");
                if (!context.GetHttpContext().Response.Headers.ContainsKey("odinlink-return"))
                {
                    context.GetHttpContext().Response.Headers["odinlink-returnUp"] = context.GetHttpContext().Request.Headers["odinlink-Next"].ToString();
                    System.Console.WriteLine($"odinlink-returnUp:{context.GetHttpContext().Response.Headers["odinlink-returnUp"].ToString()}");
                    context.GetHttpContext().Response.Headers["odinlink-return"] = OdinSnowFlakeHelper.CreateSnowFlake().ToString();
                    System.Console.WriteLine($"  odinlink-return: [ {context.GetHttpContext().Response.Headers["odinlink-return"]} ]");
                }
                else
                {
                    context.GetHttpContext().Response.Headers["odinlink-returnUp"] = context.GetHttpContext().Response.Headers["odinlink-return"].ToString();
                    System.Console.WriteLine($"odinlink-returnUp: [ {context.GetHttpContext().Response.Headers["odinlink-returnUp"]} ]");
                    context.GetHttpContext().Response.Headers["odinlink-return"] = OdinSnowFlakeHelper.CreateSnowFlake().ToString();
                    System.Console.WriteLine($"odinlink-return: [ {context.GetHttpContext().Response.Headers["odinlink-return"]} ]");
                }
                System.Console.WriteLine($"=============OdinAspectCoreInterceptorAttribute  return  end=============");
            }
        }
    }
}