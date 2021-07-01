using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OdinPlugs.OdinMvcCore.OdinFilter
{
    public class ApiSecurityAttribute : Attribute
    {

    }
    public class ApiSecurityFilterAttribute : ActionFilterAttribute
    {
        public ApiSecurityFilterAttribute()
        {
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
    public class NoApiSecurityAttribute : Attribute
    {

    }
    public class NoApiSecurityFilterAttribute : ActionFilterAttribute
    {
        public NoApiSecurityFilterAttribute()
        {
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}