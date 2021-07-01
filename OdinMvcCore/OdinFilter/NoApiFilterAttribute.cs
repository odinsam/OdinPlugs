using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OdinPlugs.OdinMvcCore.OdinFilter
{
    public class ApiAttribute : Attribute
    {

    }
    public class ApiFilterAttribute : ActionFilterAttribute
    {
        public ApiFilterAttribute()
        {
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
    public class NoApiAttribute : Attribute
    {

    }
    public class NoApiFilterAttribute : ActionFilterAttribute
    {
        public NoApiFilterAttribute()
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
