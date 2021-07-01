using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OdinPlugs.OdinBasicDataType.OdinEnum;
using OdinPlugs.OdinMvcCore.OdinInject.InjectInterface;

namespace OdinPlugs.OdinNetCore.WebApi.HttpClientHelper.HttpClientInterface
{
    public interface IApiHelper : IAutoInjectWithParamas
    {
        /// <summary>
        /// ~ 通过sslCer 重构 HttpClient
        /// </summary>
        /// <param name="sslCer">SslCer 对象</param>
        void CreateHttpClientBySslCer(SslCer sslCer);

        /// <summary>
        /// ~ 异步调用api方法
        /// </summary>
        /// <param name="method">EnumMethod类型 Get，Post，Put，Delete</param>
        /// <param name="webApiUri">请求地址 e.g https://xxx.xxx.xxx/ or http://xxx.xxx.xxx/</param>
        /// <param name="webApiPath">求方法 e.g api/xxx/xxx</param>
        /// <param name="obj">请求数据</param>
        /// <param name="customHeaders">customHeaders</param>
        /// <param name="mediaType">默认 application/json</param>
        /// <param name="encoder">字符编码 默认为 UTF8</param>
        /// <typeparam name="T">返回类型</typeparam>
        /// <returns>返回Task结果</returns>
        Task<T> InvokeByMethodAsync<T>(EnumMethod method, string webApiUri, string webApiPath, Object obj = null, Dictionary<string, string> customHeaders = null, string mediaType = "application/json", Encoding encoder = null);

        /// <summary>
        /// ~ 同步调用api方法
        /// </summary>
        /// <param name="method">EnumMethod类型 Get，Post，Put，Delete</param>
        /// <param name="webApiUri">请求地址 e.g https://xxx.xxx.xxx/ or http://xxx.xxx.xxx/</param>
        /// <param name="webApiPath">求方法 e.g api/xxx/xxx</param>
        /// <param name="obj">请求数据</param>
        /// <param name="customHeaders">customHeaders</param>
        /// <param name="mediaType">默认 application/json</param>
        /// <param name="encoder">字符编码 默认为 UTF8</param>
        /// <typeparam name="T">返回类型</typeparam>
        /// <returns>返回Task结果</returns>
        T InvokeByMethod<T>(EnumMethod method, string webApiUri, string webApiPath, Object obj = null, Dictionary<string, string> customHeaders = null, string mediaType = "application/json", Encoding encoder = null);

        /// <summary>
        /// ~ 同步调用api方法
        /// </summary>
        /// <param name="method">请求类型 Get，Post，Put，Delete</param>
        /// <param name="webApiUri">请求地址 e.g https://xxx.xxx.xxx/ or http://xxx.xxx.xxx/</param>
        /// <param name="webApiPath">求方法 e.g api/xxx/xxx</param>
        /// <param name="obj">请求数据</param>
        /// <param name="customHeaders">customHeaders</param>
        /// <param name="mediaType">默认 application/json</param>
        /// <param name="encoder">字符编码 默认为 UTF8</param>
        /// <typeparam name="T">返回类型</typeparam>
        /// <returns>返回Task结果</returns>
        T InvokeByMethod<T>(string method, string webApiUri, string webApiPath, Object obj = null, Dictionary<string, string> customHeaders = null, string mediaType = "application/json", Encoding encoder = null);

        /// <summary>
        /// ~ 异步https/http Get请求
        /// </summary>
        /// <param name="webApiUri">请求地址 e.g https://xxx.xxx.xxx/ or http://xxx.xxx.xxx/ </param>
        /// <param name="webApiPath">请求方法 e.g api/xxx/xxx</param>
        /// <param name="customHeaders">customHeaders</param>
        /// <param name="mediaType">默认 application/json</param>
        /// <param name="sslCer">https证书 默认为 null</param>
        /// <typeparam name="T">请求返回类型</typeparam>
        /// <returns>返回结果</returns>
        Task<T> GetSSLWebApiAsync<T>(string webApiUri, string webApiPath, Dictionary<string, string> customHeaders = null, string mediaType = "application/json");

        /// <summary>
        /// ~ 同步https/http Get请求
        /// </summary>
        /// <param name="webApiUri">请求地址 e.g https://xxx.xxx.xxx/ or http://xxx.xxx.xxx/ </param>
        /// <param name="webApiPath">请求方法 e.g api/xxx/xxx</param>
        /// <param name="customHeaders">customHeaders</param>
        /// <param name="mediaType">默认 application/json</param>
        /// <param name="sslCer">https证书 默认为 null</param>
        /// <typeparam name="T">请求返回类型</typeparam>
        /// <returns>返回结果</returns>
        T GetSSLWebApi<T>(string webApiUri, string webApiPath, Dictionary<string, string> customHeaders = null, string mediaType = "application/json");

        /// <summary>
        /// ~ 异步https Post Put Delete请求
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
        Task<T> SSLWebApiAsync<T>(string webApiUri, string webApiPath, Object obj,
            EnumMethod method = EnumMethod.Post,
            Dictionary<string, string> customHeaders = null,
            string mediaType = "application/json", Encoding encoder = null);

        /// <summary>
        /// ~ 同步https Post Put Delete请求
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
        T SSLWebApi<T>(string webApiUri, string webApiPath, Object obj,
            EnumMethod method = EnumMethod.Post,
            Dictionary<string, string> customHeaders = null,
            string mediaType = "application/json", Encoding encoder = null);

    }
}