using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OdinPlugs.OdinCore.Models.Aop;
using OdinPlugs.OdinExtensions.BasicExtensions.OdinString;
using OdinPlugs.OdinMAF.OdinMongoDb;
using OdinPlugs.OdinMvcCore;
using OdinPlugs.OdinMvcCore.OdinHttp;
using OdinPlugs.OdinMvcCore.OdinInject;
using OdinPlugs.OdinMvcCore.OdinMiddleware.Utils;
using OdinPlugs.OdinUtils.OdinTime;

namespace OdinPlugs.OdinMiddleware
{
    public class OdinAopMiddleware
    {
        private IWebHostEnvironment environment;
        private readonly Stopwatch stopWatch;
        private readonly IOdinMongo mongoHelper;
        private readonly RequestDelegate _next;
        private static Aop_Invoker_Model apiInvokerModel = null;
        private static Aop_ApiInvokerRecord_Model apiInvokerRecordModel = null;
        private static Aop_ApiInvokerCatch_Model apiInvokerCatchModel = null;
        private static Aop_ApiInvokerThrow_Model apiInvokerThrow_Model = null;

        /// <summary>
        /// 管道执行到该中间件时候下一个中间件的RequestDelegate请求委托，如果有其它参数，也同样通过注入的方式获得
        /// </summary>
        /// <param name="next"></param>
        public OdinAopMiddleware(RequestDelegate next, IWebHostEnvironment environment)
        {
            //通过注入方式获得对象
            _next = next;
            this.environment = environment;
            this.stopWatch = new Stopwatch();
            this.mongoHelper = OdinInjectHelper.GetService<IOdinMongo>();
        }
        /// <summary>
        /// 自定义中间件要执行的逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            System.Console.WriteLine("=========OdinAopMiddleware Request  start==========");
            this.stopWatch.Restart();
            apiInvokerModel = new Aop_Invoker_Model();
            OdinAopMiddlewareHelper.MiddlewareBefore(context, apiInvokerModel);
            // 获取Response.Body内容
            var originalBodyStream = context.Response.Body;
            var responseBody = new MemoryStream();
            context.Response.Body = responseBody;
            System.Console.WriteLine("=========OdinAopMiddleware Request  end==========");
            await _next(context);
            System.Console.WriteLine($"=========OdinAopMiddleware Response    start==========");
            new OdinAopMiddlewareHelper().MiddlewareAfter(context, apiInvokerModel, originalBodyStream, responseBody);
            // 响应完成记录时间和存入日志
            context.Response.OnCompleted(() =>
            {
                System.Console.WriteLine("=========OdinAopMiddleware Response OnCompleted==========");
                new OdinAopMiddlewareHelper().MiddlewareResponseCompleted(context, apiInvokerModel, stopWatch);
                return Task.CompletedTask;
            });
        }
    }
}