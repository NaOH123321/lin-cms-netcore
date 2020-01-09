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
            //allowAnonymousAttribute
            var allowAnonymousAttribute =
                context.MethodInfo.GetCustomAttribute(typeof(AllowAnonymousAttribute)) as AllowAnonymousAttribute;

            //controller的AuthorizeAttribute
            var controllerRequiredScopes = context.MethodInfo.ReflectedType
                ?.GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Select(attr => attr.Policy)
                .Distinct().ToList();

            //method的AuthorizeAttribute
            var methodRequiredScopes = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Select(attr => attr.Policy)
                .Distinct().ToList();

            var requiredScopes = controllerRequiredScopes == null
                ? methodRequiredScopes
                : methodRequiredScopes.Union(controllerRequiredScopes).ToList();

            if (requiredScopes.Any() && allowAnonymousAttribute == null)
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
