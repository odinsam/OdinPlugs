using Microsoft.AspNetCore.Mvc;
using OdinPlugs.OdinCore.Models;
using OdinPlugs.OdinMvcCore.OdinHttp;

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