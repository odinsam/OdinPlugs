using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace OdinPlugs.OdinMvcCore.OdinRoute
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class OdinActionRouteAttribute : RouteAttribute, IApiDescriptionGroupNameProvider
    {
        /// <summary>
        /// 分组名称,是来实现接口 IApiDescriptionGroupNameProvider
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 自定义版本+路由构造函数，继承基类路由
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="version"></param>
        public OdinActionRouteAttribute(string routeName, string apiVersion) : base($"/api/v{apiVersion}/[controller]/" + routeName)
        {
            if (routeName.StartsWith("/")) throw new Exception("action RouteName must startWith /");
            GroupName = $"v{apiVersion}";
        }
    }
}