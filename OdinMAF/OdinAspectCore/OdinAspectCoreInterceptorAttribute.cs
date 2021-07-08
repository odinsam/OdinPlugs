using System;
using System.Diagnostics;
using System.Dynamic;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Newtonsoft.Json;
using OdinPlugs.OdinMvcCore.OdinInject;
using OdinPlugs.OdinMvcCore.OdinLinkMonitor.OdinLinkMonitorInterface;
using OdinPlugs.OdinNetCore.OdinSnowFlake.SnowFlakeInterface;
using OdinPlugs.OdinNetCore.OdinSnowFlake.Utils;
using OdinPlugs.OdinUtils.OdinExtensions.BasicExtensions.OdinString;
using Serilog;

namespace OdinPlugs.OdinMAF.OdinAspectCore
{
    public class OdinAspectCoreInterceptorAttribute : AbstractInterceptorAttribute, IOdinAspectCoreInterceptorAttribute
    {
        Stopwatch stopWatch;
        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            stopWatch = Stopwatch.StartNew();
            stopWatch.Restart();
            bool isSuccess = true;
            try
            {
                System.Console.WriteLine($"=============OdinAspectCoreInterceptorAttribute  request  start=============");
                var odinLinkMonitor = OdinInjectHelper.GetService<IOdinLinkMonitor>();
                var linkMonitorId = Convert.ToInt64(context.GetHttpContext().Items["odinlinkId"]);
                var linkMonitor = odinLinkMonitor.ApiInvokerLinkMonitor(context);
                System.Console.WriteLine(JsonConvert.SerializeObject(linkMonitor[linkMonitorId].Peek()).ToJsonFormatString());
                System.Console.WriteLine($"=============OdinAspectCoreInterceptorAttribute  request  end=============");



                await next(context);



                System.Console.WriteLine($"=============OdinAspectCoreInterceptorAttribute  response  start=============");
                System.Console.WriteLine($"=============OdinAspectCoreInterceptorAttribute  response  end=============");

            }
            catch (Exception ex)
            {
                Log.Information($"执行出错");
                isSuccess = false;
                throw ex;
            }
            finally
            {

                System.Console.WriteLine($"=============OdinAspectCoreInterceptorAttribute  return  start=============");
                // stopWatch.Stop();
                var odinLinkMonitor = OdinInjectHelper.GetService<IOdinLinkMonitor>();
                var linkMonitorId = Convert.ToInt64(context.GetHttpContext().Items["odinlinkId"]);
                Console.WriteLine($"isSuccess:{isSuccess}");
                var linkMonitor = odinLinkMonitor.ApiInvokerToEndLinkMonitor(context, isSuccess, stopWatch);
                System.Console.WriteLine(JsonConvert.SerializeObject(linkMonitor[linkMonitorId].Peek()).ToJsonFormatString());
                Log.Information($"拦截后执行");
                // if (!context.GetHttpContext().Response.Headers.ContainsKey("odinlink-return"))
                // {
                //     context.GetHttpContext().Response.Headers["odinlink-returnUp"] = context.GetHttpContext().Request.Headers["odinlink-Next"].ToString();
                //     System.Console.WriteLine($"odinlink-returnUp:{context.GetHttpContext().Response.Headers["odinlink-returnUp"].ToString()}");
                //     context.GetHttpContext().Response.Headers["odinlink-return"] = OdinSnowFlakeHelper.CreateSnowFlakeId().ToString();
                //     System.Console.WriteLine($"  odinlink-return: [ {context.GetHttpContext().Response.Headers["odinlink-return"]} ]");
                // }
                // else
                // {
                //     context.GetHttpContext().Response.Headers["odinlink-returnUp"] = context.GetHttpContext().Response.Headers["odinlink-return"].ToString();
                //     System.Console.WriteLine($"odinlink-returnUp: [ {context.GetHttpContext().Response.Headers["odinlink-returnUp"]} ]");
                //     context.GetHttpContext().Response.Headers["odinlink-return"] = OdinSnowFlakeHelper.CreateSnowFlakeId().ToString();
                //     System.Console.WriteLine($"odinlink-return: [ {context.GetHttpContext().Response.Headers["odinlink-return"]} ]");
                // }
                // string methodId = context.ServiceMethod.DeclaringType.FullName + "-" + context.ServiceMethod.Name;
                // stopWatch.Stop();
                // context.GetHttpContext().Response.Headers[$"odinlink-{ methodId }-handleTime"] = stopWatch.ElapsedMilliseconds.ToString();
                // var handleTime = context.GetHttpContext().Response.Headers[$"odinlink-{ methodId }-handleTime"].ToString();
                // System.Console.WriteLine($"odinlink-{ methodId }-handleTime: [ { handleTime } ]");
                System.Console.WriteLine($"=============OdinAspectCoreInterceptorAttribute  return  end=============");
            }
        }
    }
}