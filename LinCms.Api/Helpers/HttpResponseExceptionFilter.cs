using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinCms.Api.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LinCms.Api.Helpers
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; set; } = int.MaxValue - 20;

        public void OnActionExecuting(ActionExecutingContext context)
        {

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
