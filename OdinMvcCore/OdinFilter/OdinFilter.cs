using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using OdinPlugs.OdinCore.ConfigModel;
using OdinPlugs.OdinCore.Models.Aop;
using Serilog;
using OdinPlugs.OdinCore.Models;
using OdinPlugs.OdinNetCore.WebApi;
using OdinPlugs.OdinCore.Models.Exception.ExceptionExtends;

namespace OdinPlugs.OdinMvcCore.OdinFilter
{
    public class OdinFilter
    {
        public ConfigOptions Options { get; set; }
        public ActionExecutingContext ExecutingContext { get; set; }
        public ActionExecutedContext ExecutedContext { get; set; }
        public OdinFilter(ConfigOptions _options, ActionExecutingContext executingContext)
        {
            Options = _options;
            ExecutingContext = executingContext;
        }
        public OdinFilter(ConfigOptions _options, ActionExecutedContext executedContext)
        {
            Options = _options;
            ExecutedContext = executedContext;
        }



        #region 访问Ip检测
        /// <summary>
        /// ~ 启用Ip白名单过滤 
        /// ~ 如果 controller包含 [CheckIp]  或者  action包含 [CheckIpFilter] 则判断访问ip是否合法
        /// </summary>
        public string Executing_IpValidate()
        {
            string ip = ExecutingContext.HttpContext.Connection.RemoteIpAddress.ToString();
            Log.Information($"当前访问ip：【 { ip } 】");
            if (Options.FrameworkConfig.CheckIps.Enable)
            {
                if (ExecutingContext.Controller.GetType().GetCustomAttributes(true).Any(a => a.GetType() == typeof(NoCheckIpAttribute)) ||
                    ExecutingContext.ActionDescriptor.FilterDescriptors.Any(a => a.Filter.GetType() == typeof(NoCheckIpFilterAttribute)))
                {
                    #region 启用Ip白名单过滤
                    Log.Information("启用Ip白名单过滤");
                    var allowIpsStr = Options.FrameworkConfig.CheckIps.AllowIps;
                    List<string> allowIps = allowIpsStr.Split(',').ToList();
                    if (allowIps != null && allowIps.Count > 0)
                    {
                        foreach (var item in allowIps)
                        {
                            if (ip.StartsWith(item))
                            {
                                return ip;
                            }
                        }
                    }
                    if (!allowIps.Contains(ip))
                        throw new AllowIpException("sys-allowip");
                    #endregion
                }
                else
                {
                    #region 启用Ip黑名单过滤
                    Log.Information("启用Ip黑名单过滤");
                    #endregion
                    var disallowIpsStr = Options.FrameworkConfig.CheckIps.DisallowIps;
                    if (!string.IsNullOrEmpty(disallowIpsStr))
                    {
                        List<string> disallowIps = disallowIpsStr.Split(',').ToList();
                        foreach (var item in disallowIps)
                        {
                            if (ip.StartsWith(item))
                            {
                                throw new AllowIpException("sys-allowip-disallowips");
                            }
                        }
                        if (disallowIps.Contains(ip))
                            throw new AllowIpException("sys-allowip-disallowips");

                    }
                    return ip;
                }
            }
            return ip;
        }
        #endregion

        #region 访问链路检测
        /// <summary>
        /// ~ 访问链路检测
        /// </summary>
        public void Executing_ApiLink()
        {
            if (Options.FrameworkConfig.ApiLink)
            {
                if (ExecutingContext.Controller.GetType().GetCustomAttributes(true).Any(a => a.GetType() == typeof(NoGuidAttribute)) ||
                    ExecutingContext.ActionDescriptor.FilterDescriptors.Any(a => a.Filter.GetType() == typeof(NoGuidFilterAttribute)))
                    return;
                #region 启用参数签名检查
                Log.Information("启用访问链路检测");
                #endregion
                if (!ExecutingContext.HttpContext.Request.Headers.ContainsKey("guid"))
                {
                    throw new RequestGuidException("sys-apilink");
                }
            }
        }
        #endregion

        #region 访问参数验证
        /// <summary>
        /// ~ 访问参数验证
        /// </summary>
        /// <param name="strParams"></param>
        public void Executing_ParamsSign(string strParams)
        {
            if (Options.FrameworkConfig.ParamsSign.Enable)
            {
                if (!ExecutingContext.Controller.GetType().GetCustomAttributes(true).Any(a => a.GetType() == typeof(NoParamSignCheckAttribute)) &&
                    !ExecutingContext.ActionDescriptor.FilterDescriptors.Any(a => a.Filter.GetType() == typeof(NoParamSignCheckFilterAttribute)))
                {
                    if (!string.IsNullOrEmpty(strParams))
                    {
                        #region 启用参数签名检查
                        Log.Information("启用参数签名检查");
                        #endregion
                        JObject jobj = ApiHelper.GetRequestParams(strParams, ExecutingContext.HttpContext.Request.Method);
                        if (jobj != null && jobj.Property("sign") == null)
                            throw new ParamSignException("sys-paramsign-undefind");
                        bool validate = new ParamsSignHelper().ValiedateSign(jobj, Options.FrameworkConfig.ParamsSign.signKey);
                        if (!validate)
                            throw new ParamSignException("sys-paramsign-error");
                    }
                }
            }
        }
        #endregion

        #region Token认证
        /// <summary>
        /// ~ Token认证
        /// </summary>
        /// <param name="apiCallRecordModel"></param>
        public void Executing_TokenValidate(Aop_ApiInvokerRecord_Model apiCallRecordModel, string strParams)
        {
            if (Options.ValidateToken.Enable)
            {
                if (ExecutingContext.Controller.GetType().GetCustomAttributes(true).Any(a => a.GetType() == typeof(NoTokenAttribute)) ||
                    ExecutingContext.ActionDescriptor.FilterDescriptors.Any(a => a.Filter.GetType() == typeof(NoTokenFilterAttribute)))
                    return;
                #region 启用Token检查
                string token = string.Empty;
                string userName = string.Empty;
                //如果没有token参数
                if (!ExecutingContext.HttpContext.Request.Headers.ContainsKey("token"))
                {
                    throw new TokenException("sys-token-undefind");
                }
                token = ExecutingContext.HttpContext.Request.Headers["token"].ToString();
                System.Console.WriteLine($"启用Token检查:{token}");
                // JObject jobj = null;
                // if (!string.IsNullOrEmpty(strParams))
                //     jobj = ApiHelper.GetRequestParams(strParams, ExecutingContext.HttpContext.Request.Method);
                var authenServerUril = Options.ValidateToken.Uri;
                var authenServerApiPath = Options.ValidateToken.TokenValidate;
                var result = ApiHelper.PostSSLWebApiAsync<OdinActionResult>(authenServerUril,
                    authenServerApiPath,
                    new { token = token }).Result;
                if (result.StatusCode != "ok")
                    throw new TokenException(result.StatusCode);
                #endregion
            }
        }
        #endregion

        #region Token续约
        public void Executed_ReNewToken(Aop_ApiInvokerRecord_Model apiCallRecordModel, OdinActionResult result)
        {
            if (Options.ValidateToken.Enable)
            {
                if (ExecutedContext.Controller.GetType().GetCustomAttributes(true).Any(a => a.GetType() == typeof(NoTokenAttribute)) ||
                    ExecutedContext.ActionDescriptor.FilterDescriptors.Any(a => a.Filter.GetType() == typeof(NoTokenFilterAttribute)))
                    return;
                else
                {
                    Log.Information("启用Token续约检查");
                    //如果没有token参数
                    if (!ExecutedContext.HttpContext.Request.Headers.ContainsKey("token"))
                    {
                        throw new TokenException("sys-token-undefind");
                    }
                    var token = ExecutedContext.HttpContext.Request.Headers["token"].ToString();
                    var apiServerUril = Options.ValidateToken.Uri;
                    var apiServerApiPath = Options.ValidateToken.ReNewToken;
                    var r = ApiHelper.PostSSLWebApiAsync<OdinActionResult>(apiServerUril,
                        apiServerApiPath,
                        new { token = token });
                    var data = r.Result;
                    result.Token = data.Token;
                    if (data.StatusCode == "ok" || data.StatusCode == "reNewToken")
                    {
                    }
                }
            }
        }
        #endregion
    }
}