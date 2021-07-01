using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OdinPlugs.OdinMvcCore.OdinFilter
{
    public class NoParamSignCheckAttribute : Attribute
    {

    }
    public class NoParamSignCheckFilterAttribute : ActionFilterAttribute
    {
        public NoParamSignCheckFilterAttribute()
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