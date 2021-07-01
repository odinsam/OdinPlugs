using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OdinPlugs.OdinCore.Models;
using OdinPlugs.OdinHttpContext.OdinHttpRequest;
using OdinPlugs.OdinNetCore.WebApi.HttpClientHelper;

namespace OdinPlugs.OdinMvcCore.OdinHttp
{
    public class OdinRequestParamasHelper
    {
        public static RequestParamsModel GetRequestParams(Controller controller = null)
        {
            HttpContext context = controller.HttpContext;
            return GetRequestParams(context);
        }

        public static RequestParamsModel GetRequestParams(HttpContext context)
        {
            context.Request.EnableBuffering();
            var request = context.Request;
            JObject jobj = new JObject();
            RequestParamsModel requestParams = new RequestParamsModel();
            var qStr = context.Request.QueryString.ToString();
            if (!string.IsNullOrEmpty(qStr))
            {
                string param = request.QueryString.ToString().Substring(1);
                requestParams.RequestQueryString = param;
            }
            if (request.ContentType != null && request.ContentType.StartsWith("application/json"))
            {
                string param = request.ReadRequestBody();
                requestParams.RequestJson = JsonConvert.DeserializeObject<JObject>(param);
            }
            if (request.ContentType != null &&
                    (request.ContentType.StartsWith("text/plain") || request.ContentType.StartsWith("application/javascript") ||
                    request.ContentType.StartsWith("text/html") || request.ContentType.StartsWith("application/xml")))
            {
                string param = request.ReadRequestBody();
                requestParams.RequestFormDataString = param.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            }
            if (request.ContentType != null && request.ContentType.StartsWith("application/x-www-form-urlencoded"))
            {
                string param = request.ReadRequestBody();
                requestParams.RequestFormData = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(OdinHttpClientFactory.ConvertPostDataToDictionary<string>(param)));
            }
            if (request.ContentType != null && request.ContentType.StartsWith("multipart/form-data"))
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (var kv in request.Form)
                {
                    dic.Add(kv.Key, kv.Value);
                }
                requestParams.RequestFormData = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(dic));
                List<Dictionary<string, UpLoadSmFile>> files = new List<Dictionary<string, UpLoadSmFile>>();
                long filesize = 0;
                foreach (var file in request.Form.Files)
                {
                    filesize += file.Length;
                    if (filesize > 1024 * 1024 * 4)
                        throw new Exception("文件过大无法上传，请联系管理员申请使用大文件上传服务器");
                    var fileBytes = new Byte[file.Length];
                    MemoryStream fileStream = new MemoryStream(fileBytes);
                    file.CopyTo(fileStream);
                    files.Add(new Dictionary<string, UpLoadSmFile>() { { file.Name, new UpLoadSmFile { FileName = file.Name, FileContent = fileStream.ToArray(), FileSize = file.Length } } });
                }
                requestParams.RequestUploadFile = files;
            }
            return requestParams;
        }

    }
}