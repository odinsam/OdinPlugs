using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using OdinPlugs.OdinCore.ConfigModel;
using OdinPlugs.OdinCore.Models;
using OdinPlugs.OdinExtensions.BasicExtensions.OdinString;
using OdinPlugs.OdinMAF.OdinMongoDb;
using OdinPlugs.OdinMvcCore.OdinInject;
using OdinPlugs.OdinSecurity.OdinRsa;

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
            this.options = OdinInjectHelper.GetService<ConfigOptions>(); ;
            this.mongoHelper = OdinInjectHelper.GetService<IOdinMongo>();
        }
        public void OnResultExecuted(ResultExecutedContext context) { }

        /// <summary>
        /// 方法执行结束  判断是否需要对返回的结果做安全加密
        /// </summary>
        /// <param name="context"></param>
        public void OnResultExecuting(ResultExecutingContext context)
        {
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
#if DEBUG
            System.Console.WriteLine(JsonConvert.SerializeObject(context.Result).ToJsonFormatString());
#endif

        }
    }
}