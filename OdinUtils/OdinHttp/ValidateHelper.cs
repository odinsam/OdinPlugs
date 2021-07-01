using System.Linq;
using Newtonsoft.Json.Linq;
using OdinPlugs.OdinBasicDataType.OdinEnum;
using OdinPlugs.OdinCore.Models;
using OdinPlugs.OdinCore.Models.ErrorCode;
using OdinPlugs.OdinMvcCore.OdinErrorCode;
using OdinPlugs.OdinMvcCore.OdinInject;
using Serilog;

namespace OdinPlugs.OdinUtils.OdinHttp
{
    public class ValidateHelper
    {
        public static OdinActionResult GetParamsValidate(string errorMethod, JObject jObj, params string[] getParamas)
        {
            var odinErrorCodeHelper = OdinInjectHelper.GetService<IOdinErrorCode>();
            ErrorCode_Model model = odinErrorCodeHelper.GetErrorModel("sys-error");
            if (jObj == null && getParamas.Length > 0)
            {
                model = odinErrorCodeHelper.GetErrorModel("sys-requestparams-null");
                return new OdinActionResult { StatusCode = model.ErrorCode, Message = $"{model.ShowMessage} - {errorMethod}-1" };
            }
            foreach (var item in getParamas)
            {
                if (!jObj.ContainsKey(item))
                {
                    model = odinErrorCodeHelper.GetErrorModel("sys-requestparams-undefind");
                    Log.Error($"error:{model.ErrorMessage} - {errorMethod}-1");
                    return new OdinActionResult { StatusCode = model.ErrorCode, Message = $"{model.ShowMessage} - {errorMethod}-1" };
                }
            }
            return null;
        }
        public static OdinActionResult GetParamsValidateDefault(string errorMethod, JObject jObj, params string[] getParamas)
        {
            if (jObj == null && getParamas.Length > 0)
            {
                return new OdinActionResult { StatusCode = "sys-requestparams-null", Message = $"缺少参数 - {errorMethod}-1" };
            }
            foreach (var item in getParamas)
            {
                if (!jObj.ContainsKey(item))
                {
                    Log.Error($"error:缺少参数 - {errorMethod}-1");
                    return new OdinActionResult { StatusCode = "sys-requestparams-undefind", Message = $"缺少参数 { item } - {errorMethod}-1" };
                }
            }
            return null;
        }

        public static OdinActionResult PostParamsValidate(string errorMethod, JObject jObj, params string[] validateParamas)
        {
            var odinErrorCodeHelper = OdinInjectHelper.GetService<IOdinErrorCode>();
            ErrorCode_Model model = odinErrorCodeHelper.GetErrorModel("sys-error");
            if (jObj == null && validateParamas.Length > 0)
            {
                model = odinErrorCodeHelper.GetErrorModel("sys-requestparams-null");
                return new OdinActionResult { StatusCode = model.ErrorCode, Message = $"{model.ShowMessage} - {errorMethod}-1" };
            }
            var jProp = jObj.Properties();
            model = odinErrorCodeHelper.GetErrorModel("sys-requestparams-undefind");
            foreach (var item in validateParamas)
            {
                var result = jProp.SingleOrDefault(j => j.Name == item);
                if (result == null)
                {
                    return new OdinActionResult { StatusCode = model.ErrorCode, Message = $"{model.ShowMessage} - {errorMethod}-1" };
                }
            }
            return null;
        }

        public static OdinActionResult PostParamsValidateDefault(string errorMethod, JObject jObj, params string[] validateParamas)
        {
            var odinErrorCodeHelper = OdinInjectHelper.GetService<IOdinErrorCode>();
            var model = odinErrorCodeHelper.GetErrorModel("sys-requestparams-undefind");
            if (jObj == null && validateParamas.Length > 0)
            {
                return new OdinActionResult { StatusCode = model.ErrorCode, Message = $"缺少参数 - {errorMethod}-1" };
            }
            var jProp = jObj.Properties();
            model = odinErrorCodeHelper.GetErrorModel("sys-requestparams-undefind");
            foreach (var item in validateParamas)
            {
                var result = jProp.SingleOrDefault(j => j.Name == item);
                if (result == null)
                {
                    return new OdinActionResult { StatusCode = model.ErrorCode, Message = $"缺少参数 { item } - {errorMethod}-1" };
                }
            }
            return null;
        }

        public static OdinActionResult PostParamsValidate(string errorMethod, JObject jObj, EnumContentType contentType, params string[] validateParamas)
        {
            if (contentType == EnumContentType.applicationJson)
            {
                PostParamsValidate(errorMethod, jObj, validateParamas);
            }
            return null;
        }

        public static OdinActionResult PostParamsValidateDefault(string errorMethod, JObject jObj, EnumContentType contentType, params string[] validateParamas)
        {
            if (contentType == EnumContentType.applicationJson)
            {
                PostParamsValidateDefault(errorMethod, jObj, validateParamas);
            }
            return null;
        }
    }
}