using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using LinCms.Api.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace LinCms.Api.Extensions
{
    public static class SwaggerExtensions
    {
        public static void AddMySwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "LinCms API",
                    Description = "A simple and practical CMS implemented by ASP.NET Core",
                    //TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "NaOH",
                        Email = "duanhaixiao@hotmail.com",
                        Url = new Uri("http://localhost:5000")
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "NaOH",
                        Url = new Uri("http://localhost:5000")
                    }
                });

                //添加一个必须的全局安全过滤信息，和AddSecurityDefinition方法指定的方案名称要一致，这里是Bearer。
                options.OperationFilter<SwaggerSecurityFilter>();
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 参数结构: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });

                options.OrderActionsBy(o => o.GroupName);
                // 为 Swagger JSON and UI设置xml文档注释路径
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath, true);

                var xmlPath2 = Path.Combine(AppContext.BaseDirectory, "LinCms.Core.xml");
                options.IncludeXmlComments(xmlPath2);

                var xmlPath3 = Path.Combine(AppContext.BaseDirectory, "LinCms.Infrastructure.xml");
                options.IncludeXmlComments(xmlPath3);
                //var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                //var xmlPath = Path.Combine(basePath, "SwaggerDemo.xml");
                //c.IncludeXmlComments(xmlPath);

                options.AddFluentValidationRules();
                options.EnableAnnotations();
            });
        }
    }
}
