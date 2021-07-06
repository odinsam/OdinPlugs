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
using OdinPlugs.OdinCore.Models.ApiLinkModels;
using OdinPlugs.OdinExtensions.BasicExtensions.OdinString;
using OdinPlugs.OdinMAF.OdinMongoDb;
using OdinPlugs.OdinMvcCore.OdinFilter.FilterUtils;
using OdinPlugs.OdinMvcCore.OdinInject;
using OdinPlugs.OdinMvcCore.OdinLinkMonitor.OdinLinkMonitorInterface;
using OdinPlugs.OdinNetCore.OdinAutoMapper;
using OdinPlugs.OdinNetCore.OdinSnowFlake.Utils;
using OdinPlugs.OdinUtils.OdinTime;

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
            this.options = OdinInjectHelper.GetService<ConfigOptions>();
            this.mongoHelper = OdinInjectHelper.GetService<IOdinMongo>();
        }
        public virtual void OnActionExecuting(ActionExecutingContext context)
        {
            System.Console.WriteLine($"=============ApiInvokerFilterAttribute  OnActionExecuting  start=============");
            stopWatch = Stopwatch.StartNew();
            stopWatch.Restart();
            var odinLinkMonitor = OdinInjectHelper.GetService<IOdinLinkMonitor>();
            var snowFlakeId = OdinSnowFlakeHelper.CreateSnowFlakeId();
            // 创建链路，并生成起始节点
            var linkMonitor = odinLinkMonitor.CreateOdinLinkMonitor(snowFlakeId);
            context.HttpContext.Items.Add("odinlinkId", snowFlakeId);
            context.HttpContext.Items.Add("odinlink", linkMonitor);
            System.Console.WriteLine(JsonConvert.SerializeObject(linkMonitor[snowFlakeId].Peek()).ToJsonFormatString());

            #region 保存link记录到mongodb--链路Start

            #endregion


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
        }

        public virtual void OnActionExecuted(ActionExecutedContext context)
        {
            System.Console.WriteLine($"=============ApiInvokerFilterAttribute  OnActionExecuted  end=============");
            var odinLinkMonitor = OdinInjectHelper.GetService<IOdinLinkMonitor>();
            stopWatch.Stop();
            var elapseTime = stopWatch.ElapsedMilliseconds;
            Dictionary<long, Stack<OdinApiLinkModel>> linkMonitor = null;
            if (context.Exception == null)
            {
                // 生成结束链路
                linkMonitor = odinLinkMonitor.ApiInvokerOverLinkMonitor(context, elapseTime);

                var apiInvokerModel = FilterHelper.GetApiInvokerModel(context.HttpContext, context.Result);
                var apiInvokerRecordModel = apiInvokerModel as Aop_ApiInvokerRecord_Model;
                apiInvokerRecordModel.EndTime = UnixTimeHelper.GetUnixDateTimeMS();
                apiInvokerRecordModel.ApiEndTime = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

                apiInvokerRecordModel.ElaspedTime = elapseTime;
                var apiResult = context.Result;
                (context.Result as OdinActionResult).SnowFlakeId = apiInvokerModel.Id;
                apiInvokerModel.ReturnValue = JsonConvert.SerializeObject(apiResult);

                var mongoHelper = OdinInjectHelper.GetService<IOdinMongo>();

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
            }
            else
            {
                linkMonitor = odinLinkMonitor.ApiInvokerOverLinkMonitor(context, elapseTime, false);
                System.Console.WriteLine(JsonConvert.SerializeObject(context.Exception).ToString());
            }
            var linkMonitorId = Convert.ToInt64(context.HttpContext.Items["odinlinkId"]);
            System.Console.WriteLine(JsonConvert.SerializeObject(linkMonitor[linkMonitorId].Peek()).ToJsonFormatString());
        }
    }
}