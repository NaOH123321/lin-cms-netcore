using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MTMAPI.Helper;
using Swashbuckle.AspNetCore.Swagger;

namespace MTMAPI.Extensions
{
    public static class SwaggerExtension
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "MTM API",
                    Version = "v1"
                });

                c.EnableAnnotations();
                c.OperationFilter<HttpHeaderFilter>();

                c.AddSecurityDefinition("MTMAPI", new OpenApiSecurityScheme()
                {
                    Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                // c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                //     {
                //         new OpenApiSecurityScheme {
                //             Reference = new OpenApiReference {
                //                 Type = ReferenceType.SecurityScheme,
                //                 Id = "MTMAPI"
                //             }
                //         },
                //         Array.Empty<string>()
                //     }
                // });

                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location); //获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = Path.Combine(basePath, "MTMAPI.xml");
                c.IncludeXmlComments(xmlPath);
                c.AddFluentValidationRules();
            });
        }
    }
}