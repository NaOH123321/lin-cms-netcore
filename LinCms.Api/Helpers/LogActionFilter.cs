using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using LinCms.Core.Interfaces;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LinCms.Api.Helpers
{
    public class LogActionFilter : IResultFilter, IOrderedFilter
    {
        public int Order { get; set; } = int.MaxValue - 10;

        private readonly ILinLogger _linLogger;

        public LogActionFilter(ILinLogger linLogger)
        {
            _linLogger = linLogger;
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {

        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.Exception != null) return;
            if (context.HttpContext.Response.StatusCode >= 300) return;

            var auditAction = (context.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo;
            var logAttribute = auditAction?.GetCustomAttribute<LogAttribute>();
            if (logAttribute == null) return;

            var permissionMetaAttribute = auditAction?.GetCustomAttribute<PermissionMetaAttribute>();

            var currentUser = (ICurrentUser) context.HttpContext.RequestServices.GetService(typeof(ICurrentUser));
            _linLogger.AddLog(currentUser.Id, currentUser.Username, logAttribute.Template, permissionMetaAttribute?.Auth);
        }
    }
}
