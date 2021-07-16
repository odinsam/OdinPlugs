using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using OdinPlugs.OdinCore.ConfigModel;
using OdinPlugs.OdinCore.Models;
using OdinPlugs.OdinCore.Models.Aop;
using OdinPlugs.OdinInject;
using OdinPlugs.OdinInject.InjectCore;
using OdinPlugs.OdinInject.InjectPlugs.OdinMongoDbInject;
using OdinPlugs.OdinMvcCore.OdinFilter.FilterUtils;
using OdinPlugs.OdinNetCore.OdinAutoMapper;
using OdinPlugs.OdinUtils.Utils.OdinTime;

namespace OdinPlugs.OdinMvcCore.OdinFilter
{
    /// <summary>
    /// 全局  请求拦截  拦截器
    /// </summary>
    public class ApiInvokerFilterAttribute : IActionFilter
    {
        private string aopUri = string.Empty;
        private string aopApiPath = string.Empty;
        private ConfigOptions options { get; set; }
        private OdinFilter odinFilter;
        private IOdinMongo mongoHelper;
        Stopwatch stopWatch;

        public ApiInvokerFilterAttribute()
        {
            this.options = OdinInjectCore.GetService<ConfigOptions>();
            this.mongoHelper = OdinInjectCore.GetService<IOdinMongo>();
        }
        public virtual void OnActionExecuting(ActionExecutingContext context)
        {
            System.Console.WriteLine($"=============ApiInvokerFilterAttribute  OnActionExecuting  start=============");
            stopWatch = Stopwatch.StartNew();
            stopWatch.Restart();
            if (context.ActionDescriptor.FilterDescriptors.Any(a => a.Filter.GetType() == typeof(ApiFilterAttribute)) ||
                (!context.ActionDescriptor.FilterDescriptors.Any(a => a.Filter.GetType() == typeof(NoApiFilterAttribute)) &&
                    context.Controller.GetType().GetCustomAttributes(true).Any(a => a.GetType() == typeof(ApiAttribute))
                )
            )
            {
                // 倒带 以便多次获取  请求参数
                context.HttpContext.Request.EnableBuffering();
                odinFilter = new OdinFilter(options, context);
                // ip 检测
                string ip = odinFilter.Executing_IpValidate();
                context.RouteData.Values.Add("ip", ip);
                string strParams = string.Empty;
                //api链路检测
                odinFilter.Executing_ApiLink();
                //参数签名检测
                odinFilter.Executing_ParamsSign(strParams);
                //token检测 
                //odinFilter.Executing_TokenValidate(apiCallRecordModel, strParams);
            }
            System.Console.WriteLine($"=============ApiInvokerFilterAttribute  OnActionExecuting  end=============");
        }

        public virtual void OnActionExecuted(ActionExecutedContext context)
        {
            System.Console.WriteLine($"=============ApiInvokerFilterAttribute  OnActionExecuted  start=============");
            stopWatch.Stop();
            var elapseTime = stopWatch.ElapsedMilliseconds;
            if (context.Exception == null)
            {
                var apiInvokerModel = FilterHelper.GetApiInvokerModel(context.HttpContext, context.Result);
                var apiInvokerRecordModel = apiInvokerModel as Aop_ApiInvokerRecord_Model;
                apiInvokerRecordModel.EndTime = UnixTimeHelper.GetUnixDateTimeMS();
                apiInvokerRecordModel.ApiEndTime = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

                apiInvokerRecordModel.ElaspedTime = elapseTime;
                var apiResult = context.Result;
                (context.Result as OdinActionResult).SnowFlakeId = apiInvokerModel.Id;
                apiInvokerModel.ReturnValue = JsonConvert.SerializeObject(apiResult);

                var mongoHelper = OdinInjectCore.GetService<IOdinMongo>();

                #region 如果方法调用返回catch异常   保存记录结果到mongodb
                var responseResult = JsonConvert.DeserializeObject<OdinActionResult>(apiInvokerRecordModel.ReturnValue);
                if (responseResult.StatusCode != "ok")
                {
                    var apiInvokerCatchModel = OdinAutoMapper.DynamicMapper<Aop_ApiInvokerCatch_Model>(apiInvokerModel);
                    apiInvokerCatchModel.Ex = responseResult.Data as Exception;
                    apiInvokerCatchModel.ErrorMessage = responseResult.ErrorMessage;
                    apiInvokerCatchModel.ShowMessage = responseResult.Message;
                    apiInvokerCatchModel.ErrorCode = responseResult.StatusCode;
                    apiInvokerCatchModel.ErrorTime = UnixTimeHelper.GetUnixDateTimeMS();
                    mongoHelper.AddModel<Aop_ApiInvokerCatch_Model>("Aop_ApiInvokerCatch", apiInvokerCatchModel);
                }
                #endregion

                #region 保存调用记录到mongodb
                apiInvokerModel.ElaspedTime = stopWatch.ElapsedMilliseconds;
                apiInvokerRecordModel = OdinAutoMapper.DynamicMapper<Aop_ApiInvokerRecord_Model>(apiInvokerModel);
                mongoHelper.AddModel<Aop_ApiInvokerRecord_Model>("Aop_ApiInvokerRecord", apiInvokerRecordModel);
                #endregion
                System.Console.WriteLine($"=============ApiInvokerFilterAttribute  OnActionExecuted  end=============");
            }
            else
            {
                System.Console.WriteLine(JsonConvert.SerializeObject(context.Exception).ToString());
                System.Console.WriteLine($"=============ApiInvokerFilterAttribute  OnActionExecuted  end=============");
                throw context.Exception;
            }

        }
    }
}