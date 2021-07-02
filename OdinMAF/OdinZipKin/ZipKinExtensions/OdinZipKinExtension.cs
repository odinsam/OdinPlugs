using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using zipkin4net;
using zipkin4net.Middleware;
using zipkin4net.Tracers.Zipkin;
using zipkin4net.Transport.Http;

namespace OdinPlugs.OdinMAF.OdinZipKin.ZipKinExtensions
{
    public static class OdinZipKinExtension
    {
        public static void UserZipkinCore(this IApplicationBuilder app, IHostApplicationLifetime applicationLifetime, ILoggerFactory loggerFactory)
        {
            applicationLifetime.ApplicationStarted.Register(() =>
            {
                //记录数据密度，1.0代表全部记录
                TraceManager.SamplingRate = 1.0f;
                //链路日志
                var logger = new TracingLogger(loggerFactory, "zipkin4net");
                //zipkin服务地址和内容类型
                var httpSender = new HttpZipkinSender("http://localhost:9411/", "application/json");
                var tracer = new ZipkinTracer(httpSender, new JSONSpanSerializer(), new Statistics());
                var consoleTracer = new zipkin4net.Tracers.ConsoleTracer();
                TraceManager.RegisterTracer(tracer);
                TraceManager.RegisterTracer(consoleTracer);
                TraceManager.Start(logger);
            });
            //程序停止时停止链路跟踪
            applicationLifetime.ApplicationStopped.Register(() => TraceManager.Stop());
            //引入zipkin中间件，用于跟踪服务请求,这边的名字可自定义代表当前服务名称
            app.UseTracing("OdinCore");
        }

    }
}