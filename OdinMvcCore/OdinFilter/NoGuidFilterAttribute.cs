using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OdinPlugs.OdinMvcCore.OdinFilter
{
    public class NoGuidAttribute : Attribute
    {

    }
    public class NoGuidFilterAttribute : ActionFilterAttribute
    {
        public NoGuidFilterAttribute()
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