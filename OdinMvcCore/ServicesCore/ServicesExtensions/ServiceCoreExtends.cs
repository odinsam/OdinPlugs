using System;
using System.Reflection;
using OdinPlugs.OdinCore.Models;
using OdinPlugs.OdinInject;
using OdinPlugs.OdinMvcCore.OdinErrorCode;
using OdinPlugs.OdinMvcCore.ServicesCore.ServicesInterface;

namespace OdinPlugs.OdinMvcCore.ServicesCore.ServicesExtensions
{
    public static class ServiceCoreExtends
    {
        public static OdinActionResult ServiceDbSave(this IService service, int saveChange, OdinActionResult success, OdinActionResult failed) =>
            saveChange > 0 ? success : failed;

        public static OdinActionResult ServiceOk(this IService service, string stateCode = "ok") =>
            new OdinActionResult { StatusCode = stateCode };

        public static OdinActionResult ServiceResult(this IService service,
                Object data = null, string stateCode = "ok", string token = "")
        {
            var odinErrorCodeHelper = OdinInjectCore.GetService<IOdinErrorCode>();
            return new OdinActionResult
            {

                Data = data,
                StatusCode = stateCode,
                Message = odinErrorCodeHelper.GetErrorModel(stateCode).ShowMessage,
                Token = token,
                Handle = odinErrorCodeHelper.GetErrorModel(stateCode).Handle
            };
        }


        // public static OdinActionResult ServiceResultNotify(this ServiceCore service,
        //     Object data = null, string stateCode = "ok", string token = "")
        // {
        //     ErrorModel errorModel = ErrorHelper.SelectErrorModel(stateCode);
        //     if (errorModel.Handle == "mail")
        //         service.core.SendMail("guid", service.api, null, service.developerName, service.developerEmailAddress);
        //     return new OdinActionResult
        //     {
        //         Data = data,
        //         StatusCode = stateCode,
        //         Message = errorModel.ShowMessage,
        //         Token = token,
        //         Handle = errorModel.Handle
        //     };
        // }

        public static OdinActionResult ServiceError(this IService service, string stateCode = "sys-error", MethodBase methosBase = null, Object data = null, string token = "")
        {
            var odinErrorCodeHelper = OdinInjectCore.GetService<IOdinErrorCode>();
            var errorModel = odinErrorCodeHelper.GetErrorModel(stateCode);
            //throw new ServiceException(stateCode);
            return new OdinActionResult
            {
                Data = data,
                StatusCode = stateCode,
                Message = errorModel.ShowMessage,
                Token = token,
                Handle = errorModel.Handle,
                ErrorMessage = $"{errorModel.ErrorCode} - {methosBase.DeclaringType.FullName}",
            };
        }

        // public static OdinActionResult ServiceErrorNotify(this ServiceCore service, string stateCode = "sys-error", MethodBase methosBase = null, Object data = null, string token = "")
        // {
        //     ErrorModel errorModel = ErrorHelper.SelectErrorModel(stateCode);
        //     if (errorModel.Handle == "mail")
        //         service.core.SendMail("guid", service.api, null, service.developerName, service.developerEmailAddress);
        //     //throw new ServiceException(stateCode);
        //     return new OdinActionResult
        //     {
        //         Data = data,
        //         StatusCode = stateCode,
        //         Message = ErrorHelper.SelectErrorModel(stateCode).ShowMessage,
        //         Token = token,
        //         Handle = ErrorHelper.SelectErrorModel(stateCode).Handle,
        //         ErrorMessage = $"{errorModel.ErrorCode} - {methosBase.DeclaringType.FullName}",
        //     };
        // }

    }
}