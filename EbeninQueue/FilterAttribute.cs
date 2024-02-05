using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace EbeninQueue
{
    public class AuthenticateRequiredAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var request = actionContext.HttpContext.Request;

            request.Headers.TryGetValue("Authorization", out Microsoft.Extensions.Primitives.StringValues tokenHeader);
            string tokenText = tokenHeader.ToString().Replace("Bearer","").Trim();

            if (tokenText != "1AAAAAAA-2BBB-3CCC-4DDD-5EEEEEEEEE01")
            {
                actionContext.Result = new BadRequestObjectResult("İşlem için yetki gerekli");
            }
        }

    }
}
