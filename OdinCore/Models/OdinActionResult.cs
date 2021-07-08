using System;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OdinPlugs.OdinCore.Models.ErrorCode;
using OdinPlugs.OdinMAF.OdinCacheManager;
using OdinPlugs.OdinMvcCore.MvcCore;
using OdinPlugs.OdinMvcCore.OdinErrorCode;
using OdinPlugs.OdinMvcCore.OdinInject;
using OdinPlugs.OdinMvcCore.ViewModelValidate;
using OdinPlugs.OdinUtils.OdinExtensions.BasicExtensions.OdinString;

namespace OdinPlugs.OdinCore.Models
{
    /// <summary>
    /// 统一api传递格式
    /// {
    //     "Data": {},
    //     "ErrorCode": "000000",
    //     "Message": "返回消息"
    //  }
    /// </summary>
    public class OdinActionResult : ActionResult
    {
        public OdinActionResult() { }

        public OdinActionResult(Object data = null, string message = null, int responseCode = 0, string errorCode = "ok")
        {
            Data = data;
            Message = message;
            StatusCode = errorCode;
        }
        public long? SnowFlakeId { get; set; }
        public Object Data { get; set; } = null;
        public string StatusCode { get; set; } = "ok";

        public int ResponseCode { get; set; } = 200;

        public string Handle { get; set; }

        public string Message { get; set; } = "";
        public string ErrorMessage { get; set; } = "";

        public string Token { get; set; } = "";

        public override void ExecuteResult(ActionContext context)
        {
            HttpResponse response = context.HttpContext.Response;
            response.ContentType = "application/json";
            // var options = new JsonSerializerOptions {
            //     PropertyNamingPolicy = new LowerCaseNamingPolicy (),
            // };
            var options =
                (context.HttpContext.RequestServices.GetService(typeof(IOptions<MvcNewtonsoftJsonOptions>)) as dynamic).Value as MvcNewtonsoftJsonOptions;
            // response.WriteAsync (System.Text.Json.JsonSerializer.Serialize (this));
            // response.WriteAsync (System.Text.Json.JsonSerializer.Serialize (this, options).UnicodeToString ());
            response.WriteAsync(JsonConvert.SerializeObject(
                this,
                new JsonSerializerSettings
                {
                    ContractResolver = options.SerializerSettings.ContractResolver
                }
            ));
        }

    }

    public static class OdinActionResultExtensions
    {
        // validateModel return result
        public static OdinActionResult OdinResult(this IModelValidate validate, Object data, int responseCode = 200, string message = "",
            string errorCode = "ok", string token = "", string developerName = "", string developerEmailAddress = "",
            ApiCommentConfig api = null)
        {
            IOdinCacheManager cacheManager = OdinInjectHelper.GetService<IOdinCacheManager>();
            ErrorCode_Model errorModel = cacheManager.Get<ErrorCode_Model>(errorCode);
            return new OdinActionResult
            {
                Data = data,
                Message = message,
                StatusCode = errorCode,
                Token = token,
                Handle = errorModel.Handle,
                ResponseCode = responseCode
            };
        }

        // controller return result
        public static OdinActionResult OdinResultOk(this Controller controller, int responseCode = 200)
        {
            return new OdinActionResult() { ResponseCode = responseCode };
        }
        public static OdinActionResult OdinResult(this Controller controller, Object data, string message = "ok", int responseCode = 200,
            string errorCode = "ok", string token = "", string developerName = "", string developerEmailAddress = "",
            ApiCommentConfig api = null)
        {
            IOdinCacheManager cacheManager = OdinInjectHelper.GetService<IOdinCacheManager>();
            if (errorCode != "ok")
            {
                ErrorCode_Model errorModel = cacheManager.Get<ErrorCode_Model>(errorCode);
            }
            return new OdinActionResult
            {
                ResponseCode = responseCode,
                Data = data,
                Message = message,
                StatusCode = errorCode,
                Token = token,
                Handle = ""
            };
        }

        public static OdinActionResult OdinErrorResult(this IModelValidate validate, string errorCode = "sys-error", MethodBase methosBase = null,
            string developerName = "", string developerEmailAddress = "",
            ApiCommentConfig api = null)
        {
            IOdinCacheManager cacheManager = OdinInjectHelper.GetService<IOdinCacheManager>();
            ErrorCode_Model errorModel = cacheManager.Get<ErrorCode_Model>(errorCode);
            var odinErrorCodeHelper = OdinInjectHelper.GetService<IOdinErrorCode>();
            return new OdinActionResult
            {
                Message = $"{errorModel.ShowMessage}",
                StatusCode = errorCode,
                Handle = errorModel.Handle,
                ErrorMessage = $"{errorModel.ErrorCode} - {methosBase.DeclaringType.FullName}",
                ResponseCode = 200
            };
        }

        // controller return error
        public static OdinActionResult OdinErrorResult(this Controller controller, string errorCode, RequestParamsModel jobjParam, string guid, ApiCommentConfig api)
        {
            IOdinCacheManager cacheManager = OdinInjectHelper.GetService<IOdinCacheManager>();
            ErrorCode_Model errorModel = cacheManager.Get<ErrorCode_Model>(errorCode);
            System.Diagnostics.StackTrace ss = new System.Diagnostics.StackTrace(true);
            System.Reflection.MethodBase mb = ss.GetFrame(1).GetMethod();
            var superMethodName = mb.Name;
            errorModel.ErrorMessage = $"{errorModel.ErrorMessage} - {superMethodName}";
            return new OdinActionResult
            {
                Message = errorModel.ShowMessage,
                StatusCode = errorCode,
                Handle = errorModel.Handle,
                ErrorMessage = errorModel.ErrorMessage,
                ResponseCode = 200
            };
        }

        public static OdinActionResult OdinErrorResult(this Controller controller, string showMessage, string errorMessage = null, string statusCode = "error")
        {
            errorMessage = string.IsNullOrEmpty(errorMessage) ? showMessage : errorMessage;
            return new OdinActionResult
            {
                Message = showMessage,
                StatusCode = statusCode,
                ErrorMessage = errorMessage,
                ResponseCode = 200
            };
        }

        //controller return catchResult
        public static OdinActionResult OdinCatchResult(this Controller controller, System.Exception ex, string errorCode = "sys-catcherror")
        {
            IOdinCacheManager cacheManager = OdinInjectHelper.GetService<IOdinCacheManager>();
            System.Diagnostics.StackTrace ss = new System.Diagnostics.StackTrace(true);
            System.Reflection.MethodBase mb = ss.GetFrame(1).GetMethod();
            var superMethodName = mb.Name;
            ErrorCode_Model errorModel = cacheManager.Get<ErrorCode_Model>(errorCode);
            errorModel.ErrorMessage = $"{errorModel.ErrorMessage} - {superMethodName}";
            // core.SendMail(guid, api, ex, developerName, developerEmailAddress);
            return new OdinActionResult
            {
                Data = JsonConvert.SerializeObject(ex).ToJsonFormatString(),
                ErrorMessage = errorModel.ErrorMessage,
                StatusCode = errorCode,
                ResponseCode = 200
            };
            // return null;
        }
    }
}