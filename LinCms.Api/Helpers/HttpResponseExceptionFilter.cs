using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using LinCms.Api.Exceptions;
using LinCms.Infrastructure.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LinCms.Api.Helpers
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; set; } = int.MaxValue - 20;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //如果accept不是application/json或*/*则返回
            var acceptHeader = context.HttpContext.Request.Headers[nameof(HttpRequestHeader.Accept)];
            if (string.Equals(acceptHeader, "*/*") ||
                string.Equals(acceptHeader, MediaTypeNames.Application.Json)) return;

            var notAcceptableMsg = new NotAcceptableMsg();
            var result = new ObjectResult(notAcceptableMsg)
            {
                StatusCode = notAcceptableMsg.Code
            };
            result.ContentTypes.Add(MediaTypeNames.Application.Json);
            context.Result = result;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is CommonException exception)
            {
                context.Result = new ObjectResult(exception.ResponseMessage)
                {
                    StatusCode = exception.Code,
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
