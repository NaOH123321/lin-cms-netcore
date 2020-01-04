using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace LinCms.Api.Extensions
{
    public static class SnakeCaseQueryExtensions
    {
        public static IApplicationBuilder UseSnakeCaseQuery(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SnakeCaseQueryMiddleware>();
        }
    }

    public class SnakeCaseQueryMiddleware
    {
        private readonly RequestDelegate _next;

        //Your constructor will have the dependencies needed for database access
        public SnakeCaseQueryMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var query = context.Request.QueryString;
            if (query.HasValue)
            {
                var parms = string.Join("&", query.Value.TrimStart('?').Split('&').Select(s =>
                {
                    var kv = s.Split('=');
                    var k = kv[0].Replace("_", "");
                    var v = kv[1];
                    return $"{k}={v}";
                }));
                QueryString newQuery = new QueryString($"?{parms}");
                context.Request.QueryString = newQuery;
            }

            //Let the next middleware (MVC routing) handle the request
            //In case the path was updated, the MVC routing will see the updated path
            await _next.Invoke(context);
        }
    }
}
