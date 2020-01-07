using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Autofac;
using AutoMapper;
using FluentValidation.AspNetCore;
using LinCms.Api.Configs;
using LinCms.Api.Extensions;
using LinCms.Api.Helpers;
using LinCms.Api.Services;
using LinCms.Core.Interfaces;
using LinCms.Infrastructure.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;

namespace LinCms.Api
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine("logs", @"log.txt"), rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy { ProcessDictionaryKeys = true }
                    };
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                })
                .AddFluentValidation(cf =>
                {
                    cf.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                });

            //设置MvcOptions的相关配置
            services.AddSingleton<IConfigureOptions<MvcOptions>, ConfigureMvcOptions>();
            //设置模型校验方式
            services.AddSingleton<IConfigureOptions<ApiBehaviorOptions>, ConfigureApiBehaviorOptions>();


            services.AddMyAuthentication();
            services.AddMyAuthorization();

            services.AddDbContext<LinContext>(options =>
            {
                //options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection"),
                //    b => b.MigrationsAssembly("LinCms.Api"));

                options.UseMySql(Configuration.GetConnectionString("MySqlConnection"),
                    b => b.MigrationsAssembly("LinCms.Api"));
                //去掉所有的外键
                options.ReplaceService<IMigrationsSqlGenerator, ExtendedMySqlGenerator>();
            });

            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<ILinLogger, LinLogger>();


            //设置Swagger
            services.AddMySwaggerGen();
            //设置跨域
            services.AddCors();
            //注册资源映射关系 MappingProfile
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //校验资源
            services.AddFluentValidators();
            ////注册映射关系
            //services.AddPropertyMappings();
            //注册数据仓库
            services.AddRepositories();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<BatchRegisterServiceModule>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseMyExceptionHandler();
                app.UseHsts();
            }

            app.UseStatusCodeHandling();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "LinCms V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseCors(builder =>
            {
                var withOrigins = Configuration.GetSection("WithOrigins").Get<string[]>();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                //builder.AllowAnyOrigin();
                //builder.AllowCredentials();
                builder.WithOrigins(withOrigins);
            });

            app.UseSnakeCaseQuery();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
