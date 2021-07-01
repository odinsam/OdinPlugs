using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OdinPlugs.OdinCore.Models.Aop;
using OdinPlugs.OdinMvcCore.OdinAttr;
using OdinPlugs.OdinUtils.OdinTime;

namespace OdinPlugs.OdinMvcCore.OdinFilter.FilterUtils
{
    public class FilterHelper
    {
        public static Aop_ApiInvokerRecord_Model GetApiInvokerModel(HttpContext context, IActionResult result)
        {
            HttpRequest request = context.Request;
            var apiInvokerModel = new Aop_ApiInvokerRecord_Model();
            apiInvokerModel.Id = Convert.ToInt64(context.Request.Headers["SnowFlakeId"]);
            apiInvokerModel.RequestUrl = request.Path.ToString();
            apiInvokerModel.RequestHeader = request.Headers.ToDictionary(x => x.Key, v => string.Join(";", v.Value.ToList()));
            apiInvokerModel.ApiMethod = request.Method;
            apiInvokerModel.BeginTime = UnixTimeHelper.GetUnixDateTimeMS();
            apiInvokerModel.ApiBeginTime = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            apiInvokerModel.InputParams = JsonConvert.SerializeObject(OdinHttp.OdinRequestParamasHelper.GetRequestParams(context));
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

            return apiInvokerModel;
        }
    }
}