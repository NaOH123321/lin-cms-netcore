using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using LinCms.Infrastructure.Messages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace LinCms.Api.Extensions
{
    public static class ErrorHandlingExtensions
    {
        public static IApplicationBuilder UseStatusCodeHandling(this IApplicationBuilder app)
        {
            return app.UseStatusCodePages(context =>
            {
                var statusCode = context.HttpContext.Response.StatusCode;
                context.HttpContext.Response.ContentType = MediaTypeNames.Application.Json;

                return statusCode switch
                {
                    StatusCodes.Status401Unauthorized => context.HttpContext.Response.WriteAsync(new UnauthorizedMsg().ToJson()),
                    StatusCodes.Status403Forbidden => context.HttpContext.Response.WriteAsync(new ForbiddenMsg().ToJson()),
                    StatusCodes.Status404NotFound => context.HttpContext.Response.WriteAsync(new NotFoundMsg().ToJson()),
                    StatusCodes.Status405MethodNotAllowed => context.HttpContext.Response.WriteAsync(new MethodNotAllowedMsg().ToJson()),
                    StatusCodes.Status406NotAcceptable => context.HttpContext.Response.WriteAsync(new NotAcceptableMsg().ToJson()),
                    StatusCodes.Status415UnsupportedMediaType => context.HttpContext.Response.WriteAsync(new UnsupportedMediaTypeMsg().ToJson()),
                    _ => throw new Exception("Error, status code: " + context.HttpContext.Response.StatusCode),
                };
            });
        }
    }
}
