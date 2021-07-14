using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Serilog;
using OdinPlugs.OdinCore.ConfigModel;
using OdinPlugs.OdinCore.Models.Aop;
using OdinPlugs.OdinMAF.OdinMongoDb;
using OdinPlugs.OdinMvcCore.OdinFilter.FilterUtils;
using OdinPlugs.OdinNetCore.OdinAutoMapper;
using OdinPlugs.OdinMAF.OdinCacheManager;
using OdinPlugs.OdinCore.Models.ErrorCode;
using OdinPlugs.OdinCore.Models;
using OdinPlugs.OdinUtils.Utils.OdinTime;
using OdinPlugs.OdinInject;

namespace OdinPlugs.OdinMvcCore.OdinFilter
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly ConfigOptions options;
        private readonly IOdinMongo mongoHelper;

        public HttpGlobalExceptionFilter()
        {
            this.options = OdinInjectCore.GetService<ConfigOptions>();
            this.mongoHelper = OdinInjectCore.GetService<IOdinMongo>();
        }

        public void OnException(ExceptionContext context)
        {
            var apiInvokerModel = FilterHelper.GetApiInvokerModel(context.HttpContext, context.Result);
            apiInvokerModel.ReturnValue = JsonConvert.SerializeObject(context.Exception);

            #region 保存调用记录到mongodb
            var apiInvokerRecordModel = OdinAutoMapper.DynamicMapper<Aop_ApiInvokerRecord_Model>(apiInvokerModel);
            var mongoHelper = OdinInjectCore.GetService<IOdinMongo>();
            mongoHelper.AddModel<Aop_ApiInvokerRecord_Model>("Aop_ApiInvokerRecord", apiInvokerRecordModel);
            #endregion

            #region    保存异常记录到mongodb
            var apiInvokerThrow = OdinAutoMapper.DynamicMapper<Aop_ApiInvokerThrow_Model>(apiInvokerModel);
            apiInvokerThrow.Ex = context.Exception;
            apiInvokerThrow.ErrorMessage = context.Exception.Message;
            apiInvokerThrow.ErrorTime = UnixTimeHelper.GetUnixDateTimeMS();
            mongoHelper = OdinInjectCore.GetService<IOdinMongo>();
            mongoHelper.AddModel<Aop_ApiInvokerThrow_Model>("Aop_ApiInvokerThrow", apiInvokerThrow);
            #endregion

            var cache = OdinInjectCore.GetService<IOdinCacheManager>();
            var errorCode = cache.Get<ErrorCode_Model>("sys-error");
            var exceptionResult = new OdinActionResult
            {
                SnowFlakeId = apiInvokerModel.Id,
                Data = apiInvokerModel.ReturnValue,
                StatusCode = errorCode.ErrorCode,
                ErrorMessage = errorCode.ErrorMessage,
                Message = errorCode.ShowMessage
            };
            context.Result = exceptionResult;
            context.ExceptionHandled = true;

            // var aopUri = options.Global.Url;
            // var aopApiPath = options.Global.SysApi.AopSysErrorRecord.ApiName;
            // var cad = (context.ActionDescriptor as ControllerActionDescriptor);
            // string actionName = cad.ActionName;
            // string controllerName = cad.ControllerName;
            // string authorType = "OdinPlugs.OdinAttr.AuthorAttribute";
            // string authorName =
            //                     cad.MethodInfo.CustomAttributes
            //                             .SingleOrDefault(c => c.AttributeType.FullName == authorType) != null
            //                     ?
            //                     cad.MethodInfo.CustomAttributes
            //                             .SingleOrDefault(c => c.AttributeType.FullName == authorType)
            //                             .ConstructorArguments[0].Value.ToString()
            //                     :
            //                     (
            //                         cad.ControllerTypeInfo.CustomAttributes
            //                                 .SingleOrDefault(c => c.AttributeType.FullName == authorType) != null
            //                         ?
            //                         cad.ControllerTypeInfo.CustomAttributes
            //                                 .SingleOrDefault(c => c.AttributeType.FullName == authorType)
            //                                 .ConstructorArguments[0].Value.ToString()
            //                         :
            //                         "NoAuthor"
            //                     );
            // string errorCode = "sys-error";
            // string showMessage = string.Empty;
            // string errorMessage = string.Empty;
            // string handle = string.Empty;
            // if (typeof(OdinException).IsInstanceOfType(context.Exception))
            // {
            //     OdinException odinex = context.Exception as OdinException;
            //     errorCode = odinex.ErrorCode;
            //     showMessage = odinex.ShowMessage;
            //     errorMessage = odinex.Message;
            //     handle = odinex.Handle;
            // }
            // else
            // {
            //     showMessage = "未知错误,请重试或联系管理员";
            //     errorMessage = "未知错误";
            // }
            // var errorJson = new { ErrorCode = errorCode, ShowMessage = showMessage, Handle = handle };
            // // // JsonSerializerSettings settings = new JsonSerializerSettings();
            // // // settings.Converters.Add(new IPAddressConverter());
            // // // settings.Converters.Add(new IPEndPointConverter());
            // // // settings.Formatting = Formatting.Indented;
            // // // Log.Error(JsonConvert.SerializeObject(context.Exception).ToJsonFormatString(), settings);
            // // Log.Error(context.Exception.Message);
            // context.Result = new ApplicationErrorResult(errorJson);
            // context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            // string guid = context.HttpContext.Request.Headers.ContainsKey("guid") ? context.HttpContext.Request.Headers["guid"].ToString() : $"not-find-guid-{Guid.NewGuid().ToString("N")}";
            // Aop_SysError_Model errorModel = new Aop_SysError_Model
            // {
            //     Guid = guid,
            //     ControllerName = controllerName,
            //     ActionName = actionName,
            //     ErrorMessage = errorMessage,
            //     ErrorInfo = JsonConvert.SerializeObject(context.ExceptionDispatchInfo),
            //     ErrorTime = UnixTimeHelper.GetUnixDateTime(),
            //     Ex = context.Exception,
            //     Author = authorName
            // };
            // this.mongoHelper.AddModel<Aop_SysError_Model>("Aop_InvokerError", errorModel);
            // Log.Information("=============Aop_InvokerError save in mongo============");
            // var t = ApiHelper.PostSSLWebApiAsync<string>(aopUri, aopApiPath, errorModel);
            // //非 sys-开头的error 或者  catch  不发送邮件
            // if (options.Global.EnableErrorNotifyMail && errorJson.ErrorCode.StartsWith("sys-"))
            //     SendMail(guid, controllerName, actionName,
            //                 context.Exception, authorName, authorName + "@tongfutele.com");

        }

        // public void SendMail(string guid, string controllerName, string actionName, Exception ex, string userName, string userAddress)
        // {
        //     ErrorNotifyMail_Model model = new ErrorNotifyMail_Model();
        //     model.toUsers = new List<MailToUserModel>() { new MailToUserModel { UserName = userName, UserAddress = userAddress } };
        //     model.ccUsers = new List<MailCcUserModel>() { new MailCcUserModel { UserName = "dingjj", UserAddress = "dingjj@tongfutele.com" } };
        //     model.templateName = "errorNotify";
        //     model.subject = "程序异常通知【严重】";
        //     JObject jobj = new JObject();
        //     jobj.Add("$mailProject$", $"程序异常通知【严重】");
        //     jobj.Add("$mailContent$", $"程序 Controller:{controllerName} Action:{actionName} 调用异常，回溯Guid为:{guid},具体异常信息为:<br />{JsonConvert.SerializeObject(ex)}");
        //     jobj.Add("$mailTime$", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //     model.content = jobj;
        //     string aopUri = options.Global.Url;
        //     string apiPath = options.Global.SysApi.AopErrorNotifyMail.ApiName;
        //     var t = ApiHelper.PostSSLWebApiAsync<string>(aopUri, apiPath, model);
        // }
    }

    public class ApplicationErrorResult : ObjectResult
    {
        public ApplicationErrorResult(object value) : base(value)
        {
            StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }

    public class ErrorResponse
    {
        public ErrorResponse(string msg)
        {
            Message = msg;
        }
        public string Message { get; set; }
        public object DeveloperMessage { get; set; }
    }
}