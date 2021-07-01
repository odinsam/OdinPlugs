using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using OdinPlugs.OdinCore.Models;

namespace OdinPlugs.OdinMvcCore.OdinFilter
{
    public class OdinModelValidationFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => new
                    {
                        Name = e.Key,
                        Message = e.Value.Errors.First().ErrorMessage
                    }).ToArray();
                context.Result = new OdinActionResult(errors, responseCode: context.HttpContext.Response.StatusCode);
            }
        }
    }
}