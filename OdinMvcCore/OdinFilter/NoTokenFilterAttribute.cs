using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OdinPlugs.OdinMvcCore.OdinFilter
{
    public class NoTokenAttribute : Attribute
    {

    }
    public class NoTokenFilterAttribute : ActionFilterAttribute
    {
        public NoTokenFilterAttribute()
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