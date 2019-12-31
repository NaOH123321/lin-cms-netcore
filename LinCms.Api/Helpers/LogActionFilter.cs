using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using LinCms.Api.Exceptions;
using LinCms.Api.Extensions;
using LinCms.Core.Entities;
using LinCms.Core.Interfaces;
using LinCms.Core.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LinCms.Api.Helpers
{
    public class LogActionFilter : IActionFilter, IOrderedFilter
    {
        private readonly ILinLogRepository _linLogRepository;
        private readonly IUnitOfWork _unitOfWork;

        public int Order { get; set; } = int.MaxValue - 10;

        public LogActionFilter(ILinLogRepository linLogRepository, IUnitOfWork unitOfWork)
        {
            _linLogRepository = linLogRepository;
            _unitOfWork = unitOfWork;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null) return;

            var userId = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var userName = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            if (!int.TryParse(userId, out var id)) return;

            var auditAction = (context.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo;
            var logAttribute = auditAction?.GetCustomAttribute<LogAttribute>();
            if (logAttribute == null) return;

            var log = new LinLog
            {
                UserId = id,
                UserName = userName,
                Method = context.HttpContext.Request.Method,
                Path = context.HttpContext.Request.Path,
                StatusCode = context.HttpContext.Response.StatusCode,
                Time = DateTime.Now,
                Message = logAttribute.Msg
            };
                
            var permissionMetaAttribute = auditAction?.GetCustomAttribute<PermissionMetaAttribute>();
            if (permissionMetaAttribute != null)
            {
                log.Authority = permissionMetaAttribute.Auth;
            }

            _linLogRepository.Add(log);
            if (!_unitOfWork.SaveAsync().Result)
            {
                throw new Exception("Save Failed!");
            }
        }
    }
}
