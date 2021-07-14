using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OdinPlugs.OdinCore.Models;
using OdinPlugs.OdinUtils.Utils.OdinHttp;
using OdinPlugs.OdinUtils.Utils.OdinHttp.Models;

namespace OdinPlugs.OdinMvcCore.OdinExtensions
{

    public static class ControllerExtends
    {
        public static RequestParamsModel GetRequestParams(this Controller controller)
        {
            return OdinRequestParamasHelper.GetRequestParams(controller);
        }
    }
}