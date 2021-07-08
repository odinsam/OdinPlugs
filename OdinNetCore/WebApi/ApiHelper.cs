using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OdinPlugs.OdinBasicDataType.OdinEnum;
using OdinPlugs.OdinUtils.OdinExtensions.BasicExtensions.OdinString;
using OdinPlugs.OdinUtils.Utils.OdinFiles;

namespace OdinPlugs.OdinNetCore.WebApi
{
    public class ApiHelper
    {
        /// <summary>
        /// 方法调用超时时间
        /// </summary> 
        /// <value></value>
        public static int MethodTimeOut { get; set; } = 5000;
        static Semaphore semaphore = new Semaphore(20, 20);

        /// <summary>
        /// 获取api请求的完整参数
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        // public static string GetWxParamsFromRequestBody(HttpRequest Request)
        // {
        //     using (var stream = Request.Body)
        //     {
        //         byte[] buffer = new byte[Request.ContentLength.Value];
        //         stream.Read(buffer, 0, buffer.Length);
        //         string content = Encoding.UTF8.GetString(buffer);
        //         return content;
        //     }
        // }
        public static string GetStringFromRequestBody(Stream requestBody)
        {
            using (var stream = new StreamReader(requestBody))
            {
                return stream.ReadToEnd();
            }
        }
        public static string GetStringFromRequestBodyAsync(Stream requesBody)
        {
            using (var stream = new StreamReader(requesBody, Encoding.UTF8))
            {
                return stream.ReadToEndAsync().Result;
            }
        }

        public static JObject GetRequestParams(string strParams, string method)
        {

            JObject jobj = new JObject();
            if (method == "GET")
            {
                if (strParams.Length > 0)
                {
                    strParams = strParams.Substring(1);
                    var aryParams = strParams.Split('&');
                    if (aryParams.Length > 0)
                    {
                        foreach (var item in aryParams)
                        {
                            string key = item.Split('=')[0];
                            string val = item.Split('=')[1];
                            jobj.Add(key, val);
                        }
                        return jobj;
                    }
                    return null;
                }
                else
                    return null;
            }
            else
            {
                System.Console.WriteLine(strParams);
                return JObject.Parse(strParams);
            }
        }

        /// <summary>
        /// 异步https Get请求
        /// </summary>
        /// <param name="webApiUri">请求地址 e.g http://xxx.xxx.xxx/</param>
        /// <param name="webApiPath">请求方法 e.g api/xxx/GetValue</param>
        /// <param name="customHeaders">customHeaders</param>
        /// <param name="mediaType">默认 application/json</param>
        /// <param name="sslCer">https证书 默认为 null</param>
        /// <typeparam name="T">请求返回类型</typeparam>
        /// <returns>返回Task结果</returns>
        public static async Task<T> GetSSLWebApiAsync<T>(string webApiUri, string webApiPath,
            Dictionary<string, string> customHeaders = null, string mediaType = "application/json", SslCer sslCer = null)
        {
            if (!webApiUri.StartsWith("https://"))
            {
                return await GetWebApiAsync<T>(webApiUri, webApiPath, customHeaders, mediaType);
            }
            else
            {
                return await Task.Run(() =>
                {
                    return GetSSLWebApi<T>(webApiUri, webApiPath, customHeaders, mediaType, sslCer);
                });
            }
        }

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
        private static T GetSSLWebApi<T>(string webApiUri, string webApiPath,
            Dictionary<string, string> customHeaders = null, string mediaType = "application/json", SslCer sslCer = null)
        {
            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip,
                SslProtocols = SslProtocols.Tls,
                UseDefaultCredentials = true,
                ClientCertificateOptions = ClientCertificateOption.Manual
            };
            if (sslCer != null)
            {
                if (sslCer.CerPassword != null)
                    handler.ClientCertificates.Add(new X509Certificate2(sslCer.CerPath, sslCer.CerPassword));
                else
                    handler.ClientCertificates.Add(new X509Certificate2(sslCer.CerPath));
            }
            using (HttpClient client = new HttpClient(handler))
            {
                ServicePointManager.ServerCertificateValidationCallback =
                    new RemoteCertificateValidationCallback(CheckValidationResult);
                client.BaseAddress = new Uri(webApiUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                if (customHeaders != null)
                {
                    foreach (KeyValuePair<string, string> customHeader in customHeaders)
                    {
                        client.DefaultRequestHeaders.Add(customHeader.Key, customHeader.Value);
                    }
                }
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
                Task<HttpResponseMessage> httpResponseMessage = client.GetAsync(webApiPath, HttpCompletionOption.ResponseContentRead);
                return GetResult<T>(httpResponseMessage);
            }
        }

        /// <summary>
        /// 异步https Get请求
        /// </summary>
        /// <param name="webApiUri">请求地址 e.g http://xxx.xxx.xxx/</param>
        /// <param name="webApiPath">请求方法 e.g api/xxx/GetValue</param>
        /// <param name="customHeaders">customHeaders</param>
        /// <param name="mediaType">默认 application/json</param>
        /// <typeparam name="T">请求返回类型</typeparam>
        /// <returns>返回Task结果</returns>
        public static async Task<T> GetWebApiAsync<T>(string webApiUri, string webApiPath,
            Dictionary<string, string> customHeaders = null, string mediaType = "application/json")
        {
            return await Task.Run(() =>
            {
                return GetWebApi<T>(webApiUri, webApiPath, customHeaders, mediaType);
            });
        }

        /// <summary>
        /// 同步http Get请求
        /// </summary>
        /// <param name="webApiUri">请求地址 e.g http://xxx.xxx.xxx/</param>
        /// <param name="webApiPath">请求方法 e.g api/xxx/GetValue</param>
        /// <param name="customHeaders">customHeaders</param>
        /// <param name="mediaType">默认 application/json</param>
        /// <typeparam name="T">请求返回类型</typeparam>
        /// <returns>返回结果</returns>
        private static T GetWebApi<T>(string webApiUri, string webApiPath,
            Dictionary<string, string> customHeaders = null, string mediaType = "application/json")
        {
            using (HttpClient client = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip }))
            {
                client.BaseAddress = new Uri(webApiUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                if (customHeaders != null)
                {
                    foreach (KeyValuePair<string, string> customHeader in customHeaders)
                    {
                        client.DefaultRequestHeaders.Add(customHeader.Key, customHeader.Value);
                    }
                }
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
                Task<HttpResponseMessage> httpResponseMessage = client.GetAsync(webApiPath, HttpCompletionOption.ResponseContentRead);
                return GetResult<T>(httpResponseMessage);
            }
        }

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
        public static async Task<T> PostSSLWebApiAsync<T>(string webApiUri, string webApiPath, Object obj,
            EnumMethod method = EnumMethod.Post,
            Dictionary<string, string> customHeaders = null,
            string mediaType = "application/json", Encoding encoder = null,
            SslCer sslCer = null)
        {
            if (!webApiUri.StartsWith("https://"))
            {
                return await PostWebApiAsync<T>(webApiUri, webApiPath, obj, method, customHeaders, mediaType, encoder);
            }
            else
            {
                return await Task.Run(() =>
                {
                    return PostSSLWebApi<T>(webApiUri, webApiPath, obj, method, customHeaders, mediaType, encoder, sslCer);
                });
            }
        }

        // public static async Task<T> PostSSLWebApiAsyn<T>(string webApiUri, string webApiPath, string obj,
        //                                         EnumMethod method = EnumMethod.Post,
        //                                         Dictionary<string, string> customHeaders = null,
        //                                         string mediaType = "application/json", Encoding encoder = null,
        //                                         SslCer sslCer = null)
        // {
        //     if (!webApiUri.StartsWith("https://"))
        //     {
        //         return await PostWebApiAsync<T>(webApiUri, webApiPath, obj, method, customHeaders, mediaType, encoder);
        //     }
        //     else
        //     {
        //         return await Task.Run(() =>
        //         {
        //             return PostSSLWebApi<T>(webApiUri, webApiPath, obj, method, customHeaders, mediaType, encoder, sslCer);
        //         });
        //     }
        // }

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
        private static T PostSSLWebApi<T>(string webApiUri, string webApiPath, Object obj,
            EnumMethod method = EnumMethod.Post,
            Dictionary<string, string> customHeaders = null,
            string mediaType = "application/json", Encoding encoder = null,
            SslCer sslCer = null)
        {
            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip,
                SslProtocols = SslProtocols.Tls,
                UseDefaultCredentials = true,
                ClientCertificateOptions = ClientCertificateOption.Manual
            };
            if (sslCer != null)
            {
                if (sslCer.CerPassword != null)
                    handler.ClientCertificates.Add(new X509Certificate2(sslCer.CerPath, sslCer.CerPassword));
                else
                    handler.ClientCertificates.Add(new X509Certificate2(sslCer.CerPath));
            }
            using (HttpClient httpPostClient = new HttpClient(handler))
            {
                ServicePointManager.ServerCertificateValidationCallback =
                    new RemoteCertificateValidationCallback(CheckValidationResult);
                httpPostClient.BaseAddress = new Uri(webApiUri);
                httpPostClient.DefaultRequestHeaders.Clear();
                httpPostClient.DefaultRequestHeaders.Accept.Clear();
                if (customHeaders != null)
                {
                    foreach (KeyValuePair<string, string> customHeader in customHeaders)
                    {
                        httpPostClient.DefaultRequestHeaders.Add(customHeader.Key, customHeader.Value);
                    }
                }
                httpPostClient.DefaultRequestHeaders.Connection.Add("keep-alive");
                StringBuilder jsonContent = new StringBuilder();
                string sendContent = string.Empty;
                if (mediaType == "application/json")
                {
                    if (obj != null)
                    {
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
                        httpResponseMessage = httpPostClient.PutAsync(webApiPath,
                            new StringContent(
                                sendContent.ToString(),
                                encoder == null ? Encoding.UTF8 : encoder,
                                mediaType));
                        break;
                    case EnumMethod.Delete:
                        httpResponseMessage = httpPostClient.DeleteAsync(webApiPath);
                        break;
                    default:
                        httpResponseMessage = httpPostClient.PostAsync(webApiPath,
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
                //System.Console.WriteLine(result);
                return JsonConvert.DeserializeObject<T>(result);
            }
        }

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
        public static async Task<T> PostWebApiAsync<T>(string webApiUri, string webApiPath, Object obj,
            EnumMethod method = EnumMethod.Post,
            Dictionary<string, string> customHeaders = null,
            string mediaType = "application/json", Encoding encoder = null)
        {
            return await Task.Run(() =>
            {
                return PostWebApi<T>(webApiUri, webApiPath, obj, method, customHeaders, mediaType, encoder);
            });
        }

        public static async Task<T> PostWebApiAsync<T>(string webApiUri, string webApiPath, string obj,
            EnumMethod method = EnumMethod.Post,
            Dictionary<string, string> customHeaders = null,
            string mediaType = "application/json", Encoding encoder = null)
        {
            return await Task.Run(() =>
            {
                return PostWebApi<T>(webApiUri, webApiPath, obj, method, customHeaders, mediaType, encoder);
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
        private static T PostWebApi<T>(string webApiUri, string webApiPath, Object obj,
            EnumMethod method = EnumMethod.Post,
            Dictionary<string, string> customHeaders = null,
            string mediaType = "application/json", Encoding encoder = null)
        {
            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip,
                UseDefaultCredentials = true,
                ClientCertificateOptions = ClientCertificateOption.Manual
            };
            using (HttpClient httpPostClient = new HttpClient(handler))
            {
                ServicePointManager.ServerCertificateValidationCallback =
                    new RemoteCertificateValidationCallback(CheckValidationResult);
                httpPostClient.BaseAddress = new Uri(webApiUri);
                httpPostClient.DefaultRequestHeaders.Clear();
                httpPostClient.DefaultRequestHeaders.Accept.Clear();
                if (customHeaders != null)
                {
                    foreach (KeyValuePair<string, string> customHeader in customHeaders)
                    {
                        httpPostClient.DefaultRequestHeaders.Add(customHeader.Key, customHeader.Value);
                    }
                }
                httpPostClient.DefaultRequestHeaders.Connection.Add("keep-alive");
                StringBuilder jsonContent = new StringBuilder();
                string sendContent = string.Empty;
                if (mediaType == "application/json")
                {
                    if (obj != null)
                    {
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
                        httpResponseMessage = httpPostClient.PutAsync(webApiPath,
                            new StringContent(
                                sendContent,
                                encoder == null ? Encoding.UTF8 : encoder,
                                mediaType));
                        break;
                    case EnumMethod.Delete:
                        httpResponseMessage = httpPostClient.DeleteAsync(webApiPath);
                        break;
                    default:
                        httpResponseMessage = httpPostClient.PostAsync(webApiPath,
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
        }

        // public static async Task<T> PostSSLDataAndFileAsync<T>(string webApiUri, string webApiPath, Object obj,
        //                                         EnumMethod method = EnumMethod.Post,
        //                                         Dictionary<string, string> customHeaders = null,
        //                                         string mediaType = "application/json", Encoding encoder = null,
        //                                         SslCer sslCer = null)
        // {
        //     if (!webApiUri.StartsWith("https://"))
        //     {
        //         return await PostDataAndFileAsync<T>(webApiUri, webApiPath, obj, method, customHeaders, mediaType, encoder);
        //     }
        //     else
        //     {
        //         return await Task.Run(() =>
        //         {
        //             return PostSSLDataAndFile<T>(webApiUri, webApiPath, obj, method, customHeaders, mediaType, encoder, sslCer);
        //         });
        //     }
        // }

        public static T PostFormData<T>(string webApiUri, string webApiPath,
            Dictionary<string, byte[]> filesContent,
            Dictionary<string, string> dicContent = null,
            Dictionary<string, string> customHeaders = null)
        {
            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip,
                UseDefaultCredentials = true,
                ClientCertificateOptions = ClientCertificateOption.Manual
            };
            using (HttpClient httpClient = new HttpClient(handler))
            {
                ServicePointManager.ServerCertificateValidationCallback =
                    new RemoteCertificateValidationCallback(CheckValidationResult);
                httpClient.BaseAddress = new Uri(webApiUri);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                if (customHeaders != null)
                {
                    foreach (KeyValuePair<string, string> customHeader in customHeaders)
                    {
                        httpClient.DefaultRequestHeaders.Add(customHeader.Key, customHeader.Value);
                    }
                }
                httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
                string boundary = DateTime.Now.Ticks.ToString("X");
                var formData = new MultipartFormDataContent(boundary);
                if (dicContent != null)
                {
                    foreach (var item in dicContent.Keys)
                    {
                        formData.Add(new StringContent(dicContent[item]), item);
                    }
                }
                if (filesContent != null)
                {
                    foreach (var fileName in filesContent.Keys)
                    {
                        var fileData = filesContent[fileName];
                        ByteArrayContent fileContent = new ByteArrayContent(fileData);
                        formData.Add(fileContent, "files", fileName);
                    }
                }
                Task<HttpResponseMessage> httpResponseMessage = httpClient.PostAsync(webApiPath, formData);
                string result = httpResponseMessage.Result.Content.ReadAsStringAsync().Result;
                if (typeof(T).ToString() == "System.String")
                {
                    return (T)Convert.ChangeType(result, typeof(T));
                }
                return JsonConvert.DeserializeObject<T>(result);
            }
        }

        public static T PostByteData<T>(string webApiUri, string webApiPath,
            byte[] bytedata,
            Dictionary<string, string> customHeaders = null)
        {
            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip,
                UseDefaultCredentials = true,
                ClientCertificateOptions = ClientCertificateOption.Manual
            };
            using (HttpClient httpClient = new HttpClient(handler))
            {
                ServicePointManager.ServerCertificateValidationCallback =
                    new RemoteCertificateValidationCallback(CheckValidationResult);
                httpClient.BaseAddress = new Uri(webApiUri);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                if (customHeaders != null)
                {
                    foreach (KeyValuePair<string, string> customHeader in customHeaders)
                    {
                        httpClient.DefaultRequestHeaders.Add(customHeader.Key, customHeader.Value);
                    }
                }
                httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
                Task<HttpResponseMessage> httpResponseMessage = httpClient.PostAsync(webApiPath, new ByteArrayContent(bytedata));
                string result = httpResponseMessage.Result.Content.ReadAsStringAsync().Result;
                if (typeof(T).ToString() == "System.String")
                {
                    return (T)Convert.ChangeType(result, typeof(T));
                }
                return JsonConvert.DeserializeObject<T>(result);
            }
        }

        public static async Task<string> WxPostMaterialImageAsync(string webApiUri, string webApiPath, string fileFullePath,
            byte[] fileBytes = null)
        {
            await Task.Run(() =>
            {
                System.Console.WriteLine(webApiUri + webApiPath);
                System.Console.WriteLine(fileFullePath);
                System.Console.WriteLine($"fileBytes:{fileBytes.Length}");
                HttpWebRequest request = WebRequest.Create(webApiUri + webApiPath) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
                request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
                byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
                byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
                int pos = fileFullePath.LastIndexOf(FileHelper.DirectorySeparatorChar);
                string fileName = fileFullePath.Substring(pos + 1);
                System.Console.WriteLine($"filename:{fileName}");
                //请求头部信息
                StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"media\";\"\r\n;filelength=\"{0}\";filename=\"{1}\";\r\ncontent-type:application/octet-stream\r\n\r\n",
                    fileBytes.Length, fileName));
                byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());
                Stream postStream = request.GetRequestStream();
                postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                postStream.Write(fileBytes, 0, fileBytes.Length);
                postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
                postStream.Close();
                //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream instream = response.GetResponseStream();
                StreamReader sr = new StreamReader(instream, Encoding.UTF8);
                string content = sr.ReadToEnd();
                System.Console.WriteLine($"content:{content}");
                return content;
                // using (HttpClient httpPostClient = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip }))
                // {
                //     httpPostClient.BaseAddress = new Uri(webApiUri);
                //     httpPostClient.DefaultRequestHeaders.Clear();
                //     httpPostClient.DefaultRequestHeaders.Accept.Clear();
                //     if (customHeaders != null)
                //     {
                //         foreach (KeyValuePair<string, string> customHeader in customHeaders)
                //         {
                //             httpPostClient.DefaultRequestHeaders.Add(customHeader.Key, customHeader.Value);
                //         }
                //     }
                //     //httpPostClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
                //     using (var multipartFormDataContent = new MultipartFormDataContent())
                //     {

                //         if(obj!=null)
                //         {
                //             foreach (var item in obj.GetType().GetRuntimeProperties())
                //             {
                //                 multipartFormDataContent.Add(new StringContent(item.GetValue(obj).ToString()), String.Format("\"{0}\"", item.Name));
                //             }
                //         }
                //         if(files!=null && files.Count>0)
                //         {
                //             foreach (var item in files)
                //             {
                //                 multipartFormDataContent.Add(new ByteArrayContent(item.UploadFile),item.ShowFile, item.FileName);
                //             }
                //         }
                //         httpPostClient.DefaultRequestHeaders.Connection.Add("keep-alive");
                //         Task<HttpResponseMessage> httpResponseMessage = httpPostClient.PostAsync(webApiPath, multipartFormDataContent);
                //         return httpResponseMessage.Result.Content.ReadAsStringAsync();
                //     }
                // }
            });
            return null;
        }

        private static T GetResult<T>(Task<HttpResponseMessage> httpResponseMessage)
        {
            HttpResponseMessage result = httpResponseMessage.Result;
            // 确认响应成功，否则抛出异常
            result.EnsureSuccessStatusCode();
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
                string str = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(str);
            }
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受     
        }

        private static Dictionary<string, string> ConvertObjectToDictionary(Object obj)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in obj.GetType().GetRuntimeProperties())
            {
                dic.Add(item.Name, item.GetValue(obj).ToString());
            }
            return dic;
        }

    }
}