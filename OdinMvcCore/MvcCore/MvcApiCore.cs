using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OdinPlugs.OdinBasicDataType.OdinEnum;
using OdinPlugs.OdinCore.ConfigModel;
using OdinPlugs.OdinCore.Models;
using OdinPlugs.OdinCore.Models.Aop;
using OdinPlugs.OdinCore.Models.ErrorCode;
using OdinPlugs.OdinMAF.OdinCacheManager;
using OdinPlugs.OdinMAF.OdinMongoDb;
using OdinPlugs.OdinMvcCore.OdinInject;
using OdinPlugs.OdinNetCore.WebApi.HttpClientHelper.HttpClientInterface;
using OdinPlugs.OdinUtils.OdinHttp;

namespace OdinPlugs.OdinMvcCore.MvcCore
{
    public class MvcApiCore : IMvcApiCore
    {
        private readonly ConfigOptions options;
        private readonly IOdinCacheManager cacheManager;

        public IOdinHttpClientFactory odinHttpClientFactory { get; }

        private readonly IOdinMongo mongoHelper;

        public MvcApiCore(ConfigOptions _options)
        {
            options = _options;
            this.cacheManager = OdinInjectHelper.GetService<IOdinCacheManager>();
            this.odinHttpClientFactory = OdinInjectHelper.GetService<IOdinHttpClientFactory>();
            this.mongoHelper = OdinInjectHelper.GetService<IOdinMongo>();
        }

        [Obsolete("method属性请使用 enumMethod 枚举，使用ValidateParams重载方法进行调用")]
        public OdinActionResult ValidateParams(string guid, ApiCommentConfig api, JObject jObj, string method, params string[] param)
        {
            OdinActionResult validate = null;
            if (method.ToUpper() == "GET")
                validate = ValidateHelper.GetParamsValidate(api.ApiPath, jObj, param);
            else
                validate = ValidateHelper.PostParamsValidate(api.ApiPath, jObj, param);
            if (validate != null)
            {
                ErrorCode_Model errorModel = cacheManager.Get<ErrorCode_Model>(validate.StatusCode);
                if (options.Global.EnableAop)
                    SendAop(guid, "Aop_ValidateParamsCatch", api, errorModel);
                return validate;
            }
            return null;
        }

        public OdinActionResult ValidateParams(string guid, ApiCommentConfig api, JObject jObj, EnumMethod method, params string[] param)
        {
            OdinActionResult validate = null;
            if (method == EnumMethod.Get)
                validate = ValidateHelper.GetParamsValidate(api.ApiPath, jObj, param);
            else
                validate = ValidateHelper.PostParamsValidate(api.ApiPath, jObj, param);
            if (validate != null)
            {
                ErrorCode_Model errorModel = cacheManager.Get<ErrorCode_Model>(validate.StatusCode);
                if (options.Global.EnableAop)
                    SendAop(guid, "Aop_ValidateParamsCatch", api, errorModel);
                return validate;
            }
            return null;
        }
        public OdinActionResult ValidateParamsDefault(JObject jObj, EnumMethod method, params string[] param)
        {
            OdinActionResult validate = null;
            if (method == EnumMethod.Get)
                validate = ValidateHelper.GetParamsValidateDefault(method.ToString(), jObj, param);
            else
                validate = ValidateHelper.PostParamsValidateDefault(method.ToString(), jObj, param);

            if (validate != null)
                return validate;
            return null;
        }

        public OdinActionResult ValidateParams(string guid, ApiCommentConfig api, JObject jObj, EnumMethod method, EnumContentType contentType = EnumContentType.applicationJson, params string[] param)
        {
            OdinActionResult validate = null;
            if (method == EnumMethod.Get)
                validate = ValidateHelper.GetParamsValidate(api.ApiPath, jObj, param);
            else
                validate = ValidateHelper.PostParamsValidate(api.ApiPath, jObj, contentType, param);
            if (validate != null)
            {
                ErrorCode_Model errorModel = cacheManager.Get<ErrorCode_Model>(validate.StatusCode);
                if (options.Global.EnableAop)
                    SendAop(guid, "Aop_ValidateParamsCatch", api, errorModel);
                return validate;
            }
            return null;
        }

        public OdinActionResult ValidateParamsDefault(JObject jObj, EnumMethod method, EnumContentType contentType = EnumContentType.applicationJson, params string[] param)
        {
            OdinActionResult validate = null;
            if (method == EnumMethod.Get)
                validate = ValidateHelper.GetParamsValidateDefault(method.ToString(), jObj, param);
            else
                validate = ValidateHelper.PostParamsValidateDefault(method.ToString(), jObj, contentType, param);

            if (validate != null)
                return validate;
            return null;
        }

        public void SendAop(string guid, string aopRouteingKey, ApiCommentConfig api, ErrorCode_Model errorModel, RequestParamsModel jobjParam = null, Exception ex = null)
        {
            if (options.Global.EnableAop)
            {
                this.mongoHelper.AddModel<Aop_ApiInvokerCatch_Model>(
                    aopRouteingKey,
                    new Aop_ApiInvokerCatch_Model
                    {
                        GUID = guid,
                        ControllerName = api.ApiController,
                        ActionName = api.ApiAction,
                        RequestUrl = api.ApiPath,
                        InputParams = jobjParam != null ? JsonConvert.SerializeObject(jobjParam) : null,
                        ErrorMessage = errorModel.ErrorMessage,
                        ShowMessage = errorModel.ShowMessage,
                        ErrorCode = errorModel.ErrorCode,
                        Ex = ex != null ? ex : null
                    });
            }
        }
    }
}