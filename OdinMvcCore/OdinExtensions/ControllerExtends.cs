using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OdinPlugs.OdinCore.Models;
using OdinPlugs.OdinMvcCore.OdinHttp;

namespace OdinPlugs.OdinMvcCore.OdinExtensions
{

    public static class ControllerExtends
    {
        public static T GetDIServices<T>(this Controller controller)
        {
            return controller.HttpContext.RequestServices.GetRequiredService<T>();
        }
        public static RequestParamsModel GetRequestParams(this Controller controller)
        {
            return OdinRequestParamasHelper.GetRequestParams(controller);
        }
    }
}