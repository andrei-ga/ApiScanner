using ApiScanner.Business.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ApiScanner.Web.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class IsAdminAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var accountManager = context.HttpContext.RequestServices.GetService<IAccountManager>();
            var user = context.HttpContext.User;
            var isAdmin = await accountManager.IsAdmin(user?.Identity?.Name?.Substring(user.Identity.Name.IndexOf('\\') + 1));
            if (!isAdmin)
                context.Result = new ForbidResult();
            else
                await next();
        }
    }
}
