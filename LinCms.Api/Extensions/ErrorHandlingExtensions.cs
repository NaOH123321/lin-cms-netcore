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
                var response = context.HttpContext.Response;
                var statusCode = response.StatusCode;
                response.ContentType = MediaTypeNames.Application.Json;

                return statusCode switch
                    {
                    StatusCodes.Status401Unauthorized => response.WriteAsync(new UnauthorizedMsg().ToJson()),
                    StatusCodes.Status403Forbidden => response.WriteAsync(new ForbiddenMsg().ToJson()),
                    StatusCodes.Status404NotFound => context.HttpContext.Request.Path.Value.Contains("wwwroot")
                        ? response.WriteAsync(new StaticFileNotFoundMsg().ToJson())
                        : response.WriteAsync(new NotFoundMsg().ToJson()),
                    StatusCodes.Status405MethodNotAllowed => response.WriteAsync(new MethodNotAllowedMsg().ToJson()),
                    StatusCodes.Status406NotAcceptable => response.WriteAsync(new NotAcceptableMsg().ToJson()),
                    StatusCodes.Status415UnsupportedMediaType => response.WriteAsync(new UnsupportedMediaTypeMsg()
                        .ToJson()),
                    _ => throw new Exception("Error, status code: " + response.StatusCode),
                    };
            });
        }
    }
}
