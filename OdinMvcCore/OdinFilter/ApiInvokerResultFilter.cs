using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using OdinPlugs.OdinCore.ConfigModel;
using OdinPlugs.OdinCore.Models;
using OdinPlugs.OdinInject;
using OdinPlugs.OdinInject.InjectCore;
using OdinPlugs.OdinInject.InjectPlugs.OdinMongoDbInject;
using OdinPlugs.OdinUtils.OdinSecurity.OdinRsa;

namespace OdinPlugs.OdinMvcCore.OdinFilter
{
    /// <summary>
    /// 全局 返回结果 拦截器
    /// </summary>
    public class ApiInvokerResultFilter : IResultFilter
    {
        private readonly ConfigOptions options;
        private readonly IOdinMongo mongoHelper;
        public ApiInvokerResultFilter()
        {
            this.options = OdinInjectCore.GetService<ConfigOptions>(); ;
            this.mongoHelper = OdinInjectCore.GetService<IOdinMongo>();
        }
        public void OnResultExecuted(ResultExecutedContext context) { }

        /// <summary>
        /// 方法执行结束  判断是否需要对返回的结果做安全加密
        /// </summary>
        /// <param name="context"></param>
        public void OnResultExecuting(ResultExecutingContext context)
        {
            System.Console.WriteLine($"=============ApiInvokerResultFilter  OnResultExecuting  start=============");
            var result = context.Result as OdinActionResult;
            result.ResponseCode = 200;
            if (result != null)
            {
                if (result.Data != null)
                {
                    if (options.FrameworkConfig.ApiSecurity)
                    {
                        if (context.ActionDescriptor.FilterDescriptors.Any(a => a.Filter.GetType() == typeof(ApiSecurityFilterAttribute)) ||
                            (!context.ActionDescriptor.FilterDescriptors.Any(a => a.Filter.GetType() == typeof(NoApiSecurityFilterAttribute)) &&
                                context.Controller.GetType().GetCustomAttributes(true).Any(a => a.GetType() == typeof(ApiSecurityAttribute))
                            )
                        )
                        {
                            string rsaData = RsaHelper.RsaEncrypt(JsonConvert.SerializeObject(result.Data), options.Security.Rsa.RsaPublicKey);
                            result.Data = rsaData;
                        }
                    }
                }
                context.Result = result;
            }
            System.Console.WriteLine($"=============ApiInvokerResultFilter  OnResultExecuting  end=============");
        }
    }
}