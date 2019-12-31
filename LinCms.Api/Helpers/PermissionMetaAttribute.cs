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
        public string? Auth { get; }
        public string? Module { get; }
        public bool Mount { get; } = false;

        public PermissionMetaAttribute(string auth, string module, bool mount = true)
        {
            Auth = auth;
            Module = module;
            Mount = mount;
        }

        public PermissionMetaAttribute(string role)
        {
            Roles = role;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var authorizationService = context.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            var authorizationResult = await authorizationService.AuthorizeAsync(context.HttpContext.User, null,
                new NameAuthorizationRequirement(Auth));
            if (!authorizationResult.Succeeded)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
