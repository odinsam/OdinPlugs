using System;
using Newtonsoft.Json.Linq;
using OdinPlugs.OdinBasicDataType.OdinEnum;
using OdinPlugs.OdinCore.Models;
using OdinPlugs.OdinCore.Models.ErrorCode;
using OdinPlugs.OdinMvcCore.OdinInject.InjectInterface;

namespace OdinPlugs.OdinMvcCore.MvcCore
{
    public interface IMvcApiCore : IAutoInjectWithParamas
    {
        [Obsolete("method属性请使用 enumMethod 枚举，使用ValidateParams重载方法进行调用")]
        OdinActionResult ValidateParams(string guid, ApiCommentConfig api, JObject jObj, string method, params string[] param);

        OdinActionResult ValidateParams(string guid, ApiCommentConfig api, JObject jObj, EnumMethod method, params string[] param);
        OdinActionResult ValidateParamsDefault(JObject jObj, EnumMethod method, params string[] param);

        OdinActionResult ValidateParams(string guid, ApiCommentConfig api, JObject jObj, EnumMethod method, EnumContentType contentType = EnumContentType.applicationJson, params string[] param);

        OdinActionResult ValidateParamsDefault(JObject jObj, EnumMethod method, EnumContentType contentType = EnumContentType.applicationJson, params string[] param);
    }
}