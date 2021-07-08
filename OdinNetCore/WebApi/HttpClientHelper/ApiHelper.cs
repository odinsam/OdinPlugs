using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OdinPlugs.OdinBasicDataType.OdinEnum;
using OdinPlugs.OdinNetCore.WebApi.HttpClientHelper.HttpClientInterface;
using OdinPlugs.OdinUtils.OdinExtensions.BasicExtensions.OdinString;

namespace OdinPlugs.OdinNetCore.WebApi.HttpClientHelper
{
    public class ApiHelper : IApiHelper
    {
        HttpClient _httpClient;

        public ApiHelper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// ~ 通过sslCer 重构 HttpClient
        /// </summary>
        /// <param name="sslCer">SslCer 对象</param>
        public void CreateHttpClientBySslCer(SslCer sslCer)
        {
            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip,
                SslProtocols = SslProtocols.Tls,
                UseDefaultCredentials = true,
                ClientCertificateOptions = ClientCertificateOption.Manual
            };
            if (!string.IsNullOrEmpty(sslCer.CerPassword))
                handler.ClientCertificates.Add(new X509Certificate2(sslCer.CerPath, sslCer.CerPassword));
            if (!string.IsNullOrEmpty(sslCer.CerPath))
                handler.ClientCertificates.Add(new X509Certificate2(sslCer.CerPath));
            _httpClient = new HttpClient(handler);
        }

        /// <summary>
        /// ~ 异步调用api方法
        /// </summary>
        /// <param name="method">enumMethod类型 Get，Post，Put，Delete</param>
        /// <param name="webApiUri">请求地址 e.g https://xxx.xxx.xxx/ or http://xxx.xxx.xxx/</param>
        /// <param name="webApiPath">求方法 e.g api/xxx/xxx</param>
        /// <param name="obj">请求数据</param>
        /// <param name="customHeaders">customHeaders</param>
        /// <param name="mediaType">默认 application/json</param>
        /// <param name="encoder">字符编码 默认为 UTF8</param>
        /// <typeparam name="T">返回类型</typeparam>
        /// <returns>返回Task结果</returns>
        public async Task<T> InvokeByMethodAsync<T>(EnumMethod method, string webApiUri, string webApiPath, Object obj = null, Dictionary<string, string> customHeaders = null, string mediaType = "application/json", Encoding encoder = null)
        {
            if (method == EnumMethod.Get)
                return await GetSSLWebApiAsync<T>(webApiUri, webApiPath, customHeaders, mediaType);
            else
                return await SSLWebApiAsync<T>(webApiUri, webApiPath, obj, method, customHeaders, mediaType, encoder);
        }

        /// <summary>
        /// ~ 调用api方法 
        /// </summary>
        /// <param name="method">enumMethod类型 Get，Post，Put，Delete</param>
        /// <param name="webApiUri">请求地址 e.g https://xxx.xxx.xxx/ or http://xxx.xxx.xxx/</param>
        /// <param name="webApiPath">求方法 e.g api/xxx/xxx</param>
        /// <param name="obj">请求数据</param>
        /// <param name="customHeaders">customHeaders</param>
        /// <param name="mediaType">默认 application/json</param>
        /// <param name="encoder">字符编码 默认为 UTF8</param>
        /// <typeparam name="T">返回类型</typeparam>
        /// <returns>返回Task结果</returns>
        public T InvokeByMethod<T>(EnumMethod method, string webApiUri, string webApiPath, Object obj = null, Dictionary<string, string> customHeaders = null, string mediaType = "application/json", Encoding encoder = null)
        {
            if (method == EnumMethod.Get)
                return GetSSLWebApi<T>(webApiUri, webApiPath, customHeaders, mediaType);
            else
                return SSLWebApi<T>(webApiUri, webApiPath, obj, method, customHeaders, mediaType, encoder);
        }


        /// <summary>
        /// ~ 调用api方法 ，依据 uri协议会自动判断是
        /// </summary>
        /// <param name="method">enumMethod类型 Get，Post，Put，Delete</param>
        /// <param name="webApiUri">请求地址 e.g https://xxx.xxx.xxx/ or http://xxx.xxx.xxx/</param>
        /// <param name="webApiPath">求方法 e.g api/xxx/xxx</param>
        /// <param name="obj">请求数据</param>
        /// <param name="customHeaders">customHeaders</param>
        /// <param name="mediaType">默认 application/json</param>
        /// <param name="encoder">字符编码 默认为 UTF8</param>
        /// <typeparam name="T">返回类型</typeparam>
        /// <returns>返回Task结果</returns>
        [Obsolete("方法过时，请使用method是枚举类型参数的同名重载方法")]
        public T InvokeByMethod<T>(string method, string webApiUri, string webApiPath, Object obj = null, Dictionary<string, string> customHeaders = null, string mediaType = "application/json", Encoding encoder = null)
        {
            EnumMethod invokeMethod = method.ToEnum<EnumMethod>();
            if (invokeMethod == EnumMethod.Get)
                return GetSSLWebApi<T>(webApiUri, webApiPath, customHeaders, mediaType);
            else
                return SSLWebApi<T>(webApiUri, webApiPath, obj, invokeMethod, customHeaders, mediaType, encoder);
        }

        #region 异步https/http Get请求      Task<T> GetSSLWebApiAsync<T>   
        /// <summary>
        /// 异步https/http Get请求
        /// </summary>
        /// <param name="webApiUri">请求地址 e.g https://xxx.xxx.xxx/ or http://xxx.xxx.xxx/ </param>
        /// <param name="webApiPath">请求方法 e.g api/xxx/xxx</param>
        /// <param name="customHeaders">customHeaders</param>
        /// <param name="mediaType">默认 application/json</param>
        /// <param name="sslCer">https证书 默认为 null</param>
        /// <typeparam name="T">请求返回类型</typeparam>
        /// <returns>返回结果</returns>
        public async Task<T> GetSSLWebApiAsync<T>(string webApiUri, string webApiPath, Dictionary<string, string> customHeaders = null, string mediaType = "application/json")
        {
            if (!webApiUri.StartsWith("https://"))
            {
                return await GetWebApiAsync<T>(webApiUri, webApiPath, customHeaders, mediaType);
            }
            else
            {
                return await Task<T>.Run(() =>
                {
                    return GetSSLWebApi<T>(webApiUri, webApiPath, customHeaders, mediaType);
                });
            }
        }

        #region 内部私有方法    Task<T> GetWebApiAsync<T>  /  T GetWebApi<T> 
        /// <summary>
        /// 异步https Get请求 Task<T> GetWebApiAsync<T>
        /// </summary>
        /// <param name="webApiUri">请求地址 e.g http://xxx.xxx.xxx/</param>
        /// <param name="webApiPath">请求方法 e.g api/xxx/GetValue</param>
        /// <param name="customHeaders">customHeaders</param>
        /// <param name="mediaType">默认 application/json</param>
        /// <typeparam name="T">请求返回类型</typeparam>
        /// <returns>返回Task结果</returns>
        private async Task<T> GetWebApiAsync<T>(string webApiUri, string webApiPath, Dictionary<string, string> customHeaders = null, string mediaType = "application/json")
        {
            return await Task<T>.Run(() =>
            {
                return GetWebApi<T>(webApiUri, webApiPath, customHeaders, mediaType);
            });
        }

        /// <summary>
        /// 同步http Get请求 T GetWebApi<T>
        /// </summary>
        /// <param name="webApiUri">请求地址 e.g http://xxx.xxx.xxx/</param>
        /// <param name="webApiPath">请求方法 e.g api/xxx/GetValue</param>
        /// <param name="customHeaders">customHeaders</param>
        /// <param name="mediaType">默认 application/json</param>
        /// <typeparam name="T">请求返回类型</typeparam>
        /// <returns>返回结果</returns>
        private T GetWebApi<T>(string webApiUri, string webApiPath, Dictionary<string, string> customHeaders = null, string mediaType = "application/json")
        {
            _httpClient.BaseAddress = new Uri(webApiUri);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            if (customHeaders != null)
            {
                foreach (KeyValuePair<string, string> customHeader in customHeaders)
                {
                    _httpClient.DefaultRequestHeaders.Add(customHeader.Key, customHeader.Value);
                }
            }
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            Task<HttpResponseMessage> httpResponseMessage = _httpClient.GetAsync(webApiPath, HttpCompletionOption.ResponseContentRead);
            return GetResult<T>(httpResponseMessage);

        }

        #endregion

        #endregion

        #region 同步https/http Get请求      T GetSSLWebApi<T> 
        /// <summary>
        /// 同步https Get请求
        /// </summary>
        /// <param name="webApiUri">请求地址 e.g http://xxx.xxx.xxx/</param>
        /// <param name="webApiPath">请求方法 e.g api/xxx/GetValue</param>
        /// <param name="customHeaders">customHeaders</param>
        /// <param name="mediaType">默认 application/json</param>
        /// <param name="sslCer">https证书 默认为 null</param>
        /// <typeparam name="T">请求返回类型</typeparam>
        /// <returns>返回结果</returns>
        public T GetSSLWebApi<T>(string webApiUri, string webApiPath,
            Dictionary<string, string> customHeaders = null, string mediaType = "application/json")
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            _httpClient.BaseAddress = new Uri(webApiUri);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            if (customHeaders != null)
            {
                foreach (KeyValuePair<string, string> customHeader in customHeaders)
                {
                    _httpClient.DefaultRequestHeaders.Add(customHeader.Key, customHeader.Value);
                }
            }
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            Task<HttpResponseMessage> httpResponseMessage = _httpClient.GetAsync(webApiPath, HttpCompletionOption.ResponseContentRead);
            return GetResult<T>(httpResponseMessage);
        }
        #endregion

        #region 异步https Post Put Delete请求   Task<T> PostSSLWebApiAsyn<T> 
        /// <summary>
        /// 异步https Post Put Delete请求
        /// </summary>
        /// <param name="webApiUri">请求地址 e.g http://xxx.xxx.xxx/</param>
        /// <param name="webApiPath">请求方法 e.g api/xxx/GetValue</param>
        /// <param name="obj">请求数据</param>
        /// <param name="method">请求方式</param>
        /// <param name="customHeaders">customHeaders</param>
        /// <param name="mediaType">默认 application/json</param>
        /// <param name="encoder">字符编码 默认为 UTF8</param>
        /// <param name="sslCer">https证书</param>
        /// <typeparam name="T">请求返回类型</typeparam>
        /// <returns>返回结果</returns>
        public async Task<T> SSLWebApiAsync<T>(string webApiUri, string webApiPath, object obj, EnumMethod method = EnumMethod.Post, Dictionary<string, string> customHeaders = null, string mediaType = "application/json", Encoding encoder = null)
        {
            if (!webApiUri.StartsWith("https://"))
            {
                return await WebApiAsync<T>(webApiUri, webApiPath, obj, method, customHeaders, mediaType, encoder);
            }
            else
            {
                return await Task.Run(() =>
                {
                    return SSLWebApi<T>(webApiUri, webApiPath, obj, method, customHeaders, mediaType, encoder);
                });
            }
        }

        #region 内部私有方法  Task<T>   Task<T> WebApiAsync<T>  /  T WebApi<T>
        /// <summary>
        /// 异步http Post Put Delete请求
        /// </summary>
        /// <param name="webApiUri">请求地址 e.g http://xxx.xxx.xxx/</param>
        /// <param name="webApiPath">请求方法 e.g api/xxx/GetValue</param>
        /// <param name="obj">请求数据</param>
        /// <param name="method">请求方式</param>
        /// <param name="customHeaders">customHeaders</param>
        /// <param name="mediaType">默认 application/json</param>
        /// <param name="encoder">字符编码 默认为 UTF8</param>
        /// <typeparam name="T">请求返回类型</typeparam>
        /// <returns>返回结果</returns>
        private async Task<T> WebApiAsync<T>(string webApiUri, string webApiPath, object obj, EnumMethod method = EnumMethod.Post, Dictionary<string, string> customHeaders = null, string mediaType = "application/json", Encoding encoder = null)
        {
            return await Task.Run(() =>
            {
                return WebApi<T>(webApiUri, webApiPath, obj, method, customHeaders, mediaType, encoder);
            });
        }

        /// <summary>
        /// 同步http Post Put Delete请求
        /// </summary>
        /// <param name="webApiUri">请求地址 e.g http://xxx.xxx.xxx/</param>
        /// <param name="webApiPath">请求方法 e.g api/xxx/GetValue</param>
        /// <param name="obj">请求数据</param>
        /// <param name="method">请求方式</param>
        /// <param name="customHeaders">customHeaders</param>
        /// <param name="mediaType">默认 application/json</param>
        /// <param name="encoder">字符编码 默认为 UTF8</param>
        /// <typeparam name="T">请求返回类型</typeparam>
        /// <returns>返回结果</returns>
        private T WebApi<T>(string webApiUri, string webApiPath, object obj, EnumMethod method = EnumMethod.Post,
            Dictionary<string, string> customHeaders = null, string mediaType = "application/json", Encoding encoder = null)
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            _httpClient.BaseAddress = new Uri(webApiUri);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            if (customHeaders != null)
            {
                foreach (KeyValuePair<string, string> customHeader in customHeaders)
                {
                    _httpClient.DefaultRequestHeaders.Add(customHeader.Key, customHeader.Value);
                }
            }
            _httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
            StringBuilder jsonContent = new StringBuilder();
            string sendContent = string.Empty;
            if (mediaType == "application/json")
            {
                if (obj != null)
                {
                    if (typeof(String) == obj.GetType())
                        jsonContent.Append(obj);
                    else
                        jsonContent.Append(JsonConvert.SerializeObject(obj));
                    sendContent = jsonContent.ToString();
                }
            }
            else if (mediaType == "application/x-www-form-urlencoded")
            {
                Dictionary<string, string> dic = ConvertObjectToDictionary(obj);
                if (obj != null)
                {
                    bool hasParam = false;
                    foreach (KeyValuePair<string, string> kv in dic)
                    {
                        string name = kv.Key;
                        string value = kv.Value;
                        // 忽略参数名或参数值为空的参数
                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                        {
                            if (hasParam)
                            {
                                jsonContent.Append("&");
                            }

                            jsonContent.Append(name);
                            jsonContent.Append("=");
                            if (encoder != null)
                            {
                                value = value.Utf8ToGb2312();
                            }
                            jsonContent.Append(value);
                            hasParam = true;
                        }
                    }
                    sendContent = jsonContent.ToString();
                }
            }
            else if (mediaType == "application/xml" || mediaType == "text/plain")
            {
                if (obj != null)
                {
                    sendContent = obj.ToString();
                }
            }
            else
            {
                sendContent = jsonContent.ToString().Utf8ToGb2312();
            }
            Task<HttpResponseMessage> httpResponseMessage = null;
            switch (method)
            {
                case EnumMethod.Put:
                    httpResponseMessage = _httpClient.PutAsync(webApiPath,
                        new StringContent(
                            sendContent,
                            encoder == null ? Encoding.UTF8 : encoder,
                            mediaType));
                    break;
                case EnumMethod.Delete:
                    httpResponseMessage = _httpClient.DeleteAsync(webApiPath);
                    break;
                default:
                    httpResponseMessage = _httpClient.PostAsync(webApiPath,
                        new StringContent(
                            sendContent,
                            encoder == null ? Encoding.UTF8 : encoder,
                            mediaType));
                    break;
            }
            string result = httpResponseMessage.Result.Content.ReadAsStringAsync().Result;
            if (typeof(T).ToString() == "System.String")
            {
                return (T)Convert.ChangeType(result, typeof(T));
            }
            return JsonConvert.DeserializeObject<T>(result);
        }
        #endregion

        #endregion

        #region 同步https Post Put Delete请求   T SSLWebApi<T>
        /// <summary>
        /// 同步https Post Put Delete请求
        /// </summary>
        /// <param name="webApiUri">请求地址 e.g http://xxx.xxx.xxx/</param>
        /// <param name="webApiPath">请求方法 e.g api/xxx/GetValue</param>
        /// <param name="obj">请求数据</param>
        /// <param name="method">请求方式</param>
        /// <param name="customHeaders">customHeaders</param>
        /// <param name="mediaType">默认 application/json</param>
        /// <param name="encoder">字符编码 默认为 UTF8</param>
        /// <param name="sslCer">https证书</param>
        /// <typeparam name="T">请求返回类型</typeparam>
        /// <returns>返回结果</returns>
        public T SSLWebApi<T>(string webApiUri, string webApiPath, object obj, EnumMethod method = EnumMethod.Post,
            Dictionary<string, string> customHeaders = null, string mediaType = "application/json", Encoding encoder = null)
        {
            ServicePointManager.ServerCertificateValidationCallback =
                new RemoteCertificateValidationCallback(CheckValidationResult);
            _httpClient.BaseAddress = new Uri(webApiUri);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            if (customHeaders != null)
            {
                foreach (KeyValuePair<string, string> customHeader in customHeaders)
                {
                    _httpClient.DefaultRequestHeaders.Add(customHeader.Key, customHeader.Value);
                }
            }
            _httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
            StringBuilder jsonContent = new StringBuilder();
            string sendContent = string.Empty;
            if (mediaType == "application/json")
            {
                if (obj != null)
                {
                    if (typeof(String) == obj.GetType())
                        jsonContent.Append(obj);
                    else
                        jsonContent.Append(JsonConvert.SerializeObject(obj));
                    sendContent = jsonContent.ToString();
                }
            }
            else if (mediaType == "application/x-www-form-urlencoded")
            {
                Dictionary<string, string> dic = ConvertObjectToDictionary(obj);
                if (obj != null)
                {
                    bool hasParam = false;
                    foreach (KeyValuePair<string, string> kv in dic)
                    {
                        string name = kv.Key;
                        string value = kv.Value;
                        // 忽略参数名或参数值为空的参数
                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                        {
                            if (hasParam)
                            {
                                jsonContent.Append("&");
                            }

                            jsonContent.Append(name);
                            jsonContent.Append("=");
                            if (encoder != null)
                            {
                                value = value.Utf8ToGb2312();
                            }
                            jsonContent.Append(value);
                            hasParam = true;
                        }
                    }
                    sendContent = jsonContent.ToString();
                }
            }
            else if (mediaType == "application/xml" || mediaType == "text/plain")
            {
                if (obj != null)
                {
                    sendContent = obj.ToString();
                }
            }
            else
            {
                sendContent = jsonContent.ToString().Utf8ToGb2312();
            }
            Task<HttpResponseMessage> httpResponseMessage = null;
            switch (method)
            {
                case EnumMethod.Put:
                    httpResponseMessage = _httpClient.PutAsync(webApiPath,
                        new StringContent(
                            sendContent.ToString(),
                            encoder == null ? Encoding.UTF8 : encoder,
                            mediaType));
                    break;
                case EnumMethod.Delete:
                    httpResponseMessage = _httpClient.DeleteAsync(webApiPath);
                    break;
                default:
                    httpResponseMessage = _httpClient.PostAsync(webApiPath,
                        new StringContent(
                            sendContent.ToString(),
                            encoder == null ? Encoding.UTF8 : encoder,
                            mediaType));
                    break;
            }
            string result = httpResponseMessage.Result.Content.ReadAsStringAsync().Result;
            if (typeof(T).ToString() == "System.String")
            {
                return (T)Convert.ChangeType(result, typeof(T));
            }
            return JsonConvert.DeserializeObject<T>(result);
        }

        #endregion

        #region 私有方法     T GetResult<T>  /  bool CheckValidationResult  /  Dictionary<string, string> ConvertObjectToDictionary
        private T GetResult<T>(Task<HttpResponseMessage> httpResponseMessage)
        {
            HttpResponseMessage result = httpResponseMessage.Result;
            // 确认响应成功，否则抛出异常
            // result.EnsureSuccessStatusCode();
            if (typeof(T) == typeof(byte[]))
            {
                return (T)Convert.ChangeType(result.Content.ReadAsByteArrayAsync().Result, typeof(T));
            }
            else if (typeof(T) == typeof(Stream))
            {
                return (T)Convert.ChangeType(result.Content.ReadAsStreamAsync().Result, typeof(T));
            }
            else
            {
                string str = result.Content.ReadAsStringAsync().Result.UnicodeToString();
                if (typeof(T) == typeof(String))
                    return (T)Convert.ChangeType(str, typeof(T));
                return JsonConvert.DeserializeObject<T>(str);
            }
        }
        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受     
        }
        private Dictionary<string, string> ConvertObjectToDictionary(Object obj)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in obj.GetType().GetRuntimeProperties())
            {
                dic.Add(item.Name, item.GetValue(obj).ToString());
            }
            return dic;
        }
        #endregion

    }
}