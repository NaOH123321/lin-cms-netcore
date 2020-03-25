using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LinCms.Api.Helpers
{
    public class SwaggerSecurityFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            //controller的AllowAnonymousAttribute
            var allowAnonymous1 =
                context.MethodInfo.DeclaringType?.GetCustomAttribute(typeof(AllowAnonymousAttribute)) as AllowAnonymousAttribute;

            //method的AllowAnonymousAttribute
            var allowAnonymous2 =
                context.MethodInfo.GetCustomAttribute(typeof(AllowAnonymousAttribute)) as AllowAnonymousAttribute;

            var allowAnonymousAttributeNotExist = allowAnonymous1 == null && allowAnonymous2 == null;


            //controller的AuthorizeAttribute和method的AuthorizeAttribute
            var requiredScopes = context.MethodInfo.DeclaringType?.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<AuthorizeAttribute>()
                .Select(attr => attr.Policy)
                .Distinct().ToList();

            if (requiredScopes != null && requiredScopes.Any() && allowAnonymousAttributeNotExist)
            {
                operation.Responses.Add(StatusCodes.Status401Unauthorized.ToString(),
                    new OpenApiResponse {Description = "未授权"});
                operation.Responses.Add(StatusCodes.Status403Forbidden.ToString(),
                    new OpenApiResponse {Description = "禁止访问"});

                var security = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = JwtBearerDefaults.AuthenticationScheme,
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        requiredScopes.ToList()
                    }
                };
                operation.Security.Add(security);
            }
        }
    }
}
