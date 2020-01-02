using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LinCms.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace LinCms.Api.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class PermissionMetaAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public string? Auth { get; }
        public string? Module { get; }
        public string Role { get; }
        public bool Mount { get; } = false;

        public PermissionMetaAttribute(string auth, string module, string role = UserRole.Group, bool mount = true)
        {
            Auth = auth;
            Module = module;
            Mount = mount;
            Role = role;
        }

        public PermissionMetaAttribute(string role)
        {
            Role = role;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            //如果有AllowAnonymous特性则不需要验证权限
            var auditAction = (context.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo;
            var logAttribute = auditAction?.GetCustomAttribute<AllowAnonymousAttribute>();
            if (logAttribute != null)
            {
                return;
            }

            var authorizationService = context.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            var authorizationResult = await authorizationService.AuthorizeAsync(context.HttpContext.User, null,
                new PermissionMetaRequirement
                {
                    Name = Auth,
                    Role = Role
                });
            if (!authorizationResult.Succeeded)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
