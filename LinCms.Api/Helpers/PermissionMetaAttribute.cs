using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace LinCms.Api.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class PermissionMetaAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public PermissionMetaAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var authorizationService = context.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            var authorizationResult = await authorizationService.AuthorizeAsync(context.HttpContext.User, null, new NameAuthorizationRequirement(Name));
            if (!authorizationResult.Succeeded)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
