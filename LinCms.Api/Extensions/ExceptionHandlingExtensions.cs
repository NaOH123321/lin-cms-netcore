using System.Net.Mime;
using LinCms.Infrastructure.Messages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace LinCms.Api.Extensions
{
    public static class ExceptionHandlingExtensions
    {
        public static void UseMyExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync(new InternalServerErrorMsg().ToJson());
                });
            });
        }
    }
}
