using System.Text;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OdinPlugs.OdinCore.ConfigModel;
using OdinPlugs.OdinCore.Models;
using OdinPlugs.OdinCore.Models.Aop;
using OdinPlugs.OdinCore.Models.ErrorCode;
using OdinPlugs.OdinExtensions.BasicExtensions.OdinString;
using OdinPlugs.OdinMAF.OdinCacheManager;
using OdinPlugs.OdinMAF.OdinMongoDb;
using OdinPlugs.OdinMvcCore.OdinHttp;
using OdinPlugs.OdinMvcCore.OdinInject;
using OdinPlugs.OdinNetCore.OdinAutoMapper;
using OdinPlugs.OdinUtils.OdinTime;
using OdinPlugs.OdinMvcCore.OdinAttr;

namespace OdinPlugs.OdinMvcCore.OdinMiddleware.Utils
{
    public class OdinAopMiddlewareHelper
    {
        private static Aop_ApiInvokerRecord_Model apiInvokerRecordModel = null;
        private static Aop_ApiInvokerCatch_Model apiInvokerCatchModel = null;
        private static Aop_ApiInvokerThrow_Model apiInvokerThrow_Model = null;

        /// <summary>
        /// 中间件请求前，尚未进入action方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="apiInvokerModel"></param>
        public static void MiddlewareBefore(HttpContext context, Aop_Invoker_Model apiInvokerModel)
        {
            HttpRequest request = context.Request;
            apiInvokerModel.RequestUrl = request.Path.ToString();
            apiInvokerModel.RequestHeader = request.Headers.ToDictionary(x => x.Key, v => string.Join(";", v.Value.ToList()));
            apiInvokerModel.ApiMethod = request.Method;
            apiInvokerModel.BeginTime = UnixTimeHelper.GetUnixDateTimeMS();
            apiInvokerModel.ApiBeginTime = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            apiInvokerModel.InputParams = JsonConvert.SerializeObject(OdinRequestParamasHelper.GetRequestParams(context));
            apiInvokerModel.Author = request.Headers["guid"].ToString();
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var myAttribute = endpoint.Metadata.GetMetadata<AuthorAttribute>();
                if (myAttribute != null)
                    apiInvokerModel.Author = myAttribute.AuthorName;
            }
        }

        /// <summary>
        /// 中间件请求成功后  action方法已经执行完成
        /// </summary>
        /// <param name="context"></param>
        /// <param name="apiInvokerModel"></param>
        /// <param name="originalBodyStream"></param>
        /// <param name="responseBody"></param>
        /// <returns></returns>
        public void MiddlewareAfter(HttpContext context, Aop_Invoker_Model apiInvokerModel,
            Stream originalBodyStream, MemoryStream responseBody)
        {

            System.Console.WriteLine($"=========OdinAopMiddleware Response       MiddlewareAfter==========");
            HttpRequest request = context.Request;
            apiInvokerModel.ControllerName = request.RouteValues["controller"] != null ? request.RouteValues["controller"].ToString() : "";
            apiInvokerModel.ActionName = request.RouteValues["action"] != null ? request.RouteValues["action"].ToString() : "";
            apiInvokerModel.GUID = request.Headers["guid"].ToString();
            apiInvokerRecordModel = apiInvokerModel as Aop_ApiInvokerRecord_Model;
            apiInvokerRecordModel.ReturnValue = GetResponse(context.Response) != null ? GetResponse(context.Response) : null;
            apiInvokerRecordModel.EndTime = UnixTimeHelper.GetUnixDateTimeMS();
            apiInvokerRecordModel.ApiEndTime = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            responseBody.CopyTo(originalBodyStream);
        }

        public void MiddlewareResponseCompleted(HttpContext context, Aop_Invoker_Model apiInvokerModel, Stopwatch stopWatch)
        {
            stopWatch.Stop();
            apiInvokerModel.ElaspedTime = stopWatch.ElapsedMilliseconds;
            apiInvokerRecordModel = OdinAutoMapper.DynamicMapper<Aop_ApiInvokerRecord_Model>(apiInvokerModel);
            var mongoHelper = OdinInjectHelper.GetService<IOdinMongo>();
            mongoHelper.AddModel<Aop_ApiInvokerRecord_Model>("Aop_ApiInvokerRecord", apiInvokerRecordModel);

            var responseResult = JsonConvert.DeserializeObject<OdinActionResult>(apiInvokerRecordModel.ReturnValue);
            if (responseResult.StatusCode != "ok")
            {
                apiInvokerCatchModel = OdinAutoMapper.DynamicMapper<Aop_ApiInvokerCatch_Model>(apiInvokerModel);
                apiInvokerCatchModel.Ex = responseResult.Data as Exception;
                apiInvokerCatchModel.ErrorMessage = responseResult.ErrorMessage;
                apiInvokerCatchModel.ShowMessage = responseResult.Message;
                apiInvokerCatchModel.ErrorCode = responseResult.StatusCode;
                apiInvokerCatchModel.ErrorTime = UnixTimeHelper.GetUnixDateTimeMS();
                mongoHelper.AddModel<Aop_ApiInvokerCatch_Model>("Aop_ApiInvokerCatch", apiInvokerCatchModel);
            }
        }

        public async Task MiddlewareException(HttpContext context, Aop_Invoker_Model apiInvokerModel, Exception ex)
        {
            HttpRequest request = context.Request;



            apiInvokerModel.RequestUrl = request.Path.ToString();
            apiInvokerModel.RequestHeader = request.Headers.ToDictionary(x => x.Key, v => string.Join(";", v.Value.ToList()));
            apiInvokerModel.ApiMethod = request.Method;
            // apiInvokerModel.BeginTime = UnixTimeHelper.GetUnixDateTimeMS();
            // apiInvokerModel.ApiBeginTime = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            apiInvokerModel.InputParams = JsonConvert.SerializeObject(OdinRequestParamasHelper.GetRequestParams(context));
            apiInvokerModel.Author = request.Headers["guid"].ToString();
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var myAttribute = endpoint.Metadata.GetMetadata<AuthorAttribute>();
                if (myAttribute != null)
                    apiInvokerModel.Author = myAttribute.AuthorName;
            }




            apiInvokerModel.ControllerName = request.RouteValues["controller"] != null ? request.RouteValues["controller"].ToString() : "";
            apiInvokerModel.ActionName = request.RouteValues["action"] != null ? request.RouteValues["action"].ToString() : "";
            apiInvokerModel.GUID = request.Headers["guid"].ToString();
            // apiInvokerModel.ElaspedTime = stopWatch.ElapsedMilliseconds;
            // stopWatch.Stop();


            var mongoHelper = OdinInjectHelper.GetService<IOdinMongo>();
            apiInvokerRecordModel = OdinAutoMapper.DynamicMapper<Aop_ApiInvokerRecord_Model>(apiInvokerModel);
            mongoHelper.AddModel<Aop_ApiInvokerRecord_Model>("Aop_ApiInvokerRecord", apiInvokerRecordModel);

            apiInvokerThrow_Model = OdinAutoMapper.DynamicMapper<Aop_ApiInvokerThrow_Model>(apiInvokerModel);
            apiInvokerThrow_Model.Ex = ex;
            apiInvokerThrow_Model.ErrorMessage = ex.Message;
            apiInvokerThrow_Model.ErrorTime = UnixTimeHelper.GetUnixDateTimeMS();
            mongoHelper.AddModel<Aop_ApiInvokerThrow_Model>("Aop_ApiInvokerThrow", apiInvokerThrow_Model);

            var cache = OdinInjectHelper.GetService<IOdinCacheManager>();
            var errorCode = cache.Get<ErrorCode_Model>("sys-error");

            context.Response.ContentType = "application/json;charset=utf-8;";
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync(
                JsonConvert.SerializeObject(new OdinActionResult
                {
                    Data = null,
                    StatusCode = errorCode.ErrorCode,
                    ErrorMessage = errorCode.ErrorMessage,
                    Message = errorCode.ShowMessage
                }), Encoding.UTF8
            );
        }

        /// <summary>
        /// 获取响应内容
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static string GetResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = new StreamReader(response.Body).ReadToEnd();
            response.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }
    }
}