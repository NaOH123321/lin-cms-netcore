using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Autofac;
using AutoMapper;
using FluentValidation.AspNetCore;
using LinCms.Api.Controllers.V1;
using LinCms.Api.Extensions;
using LinCms.Api.Helpers;
using LinCms.Core.Interfaces;
using LinCms.Infrastructure.Database;
using LinCms.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
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

            services.AddControllers(options =>
                {
                    options.ReturnHttpNotAcceptable = true;
                    options.Filters.Add(new HttpResponseExceptionFilter());
                    //改变model的验证信息
                    options.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => "请求的body不能为空");
                    options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(value =>
                        $"{value}不是有效的值");
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy { ProcessDictionaryKeys = true }
                    };
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
                })
                .AddFluentValidation(cf =>
                {
                    cf.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                });

            //设置模型校验方式
            services.AddSingleton<IConfigureOptions<ApiBehaviorOptions>, ConfigureApiBehaviorOptions>();

            //services.AddMediaTypes();
            services.AddMyAuthentication();
            services.AddMyAuthorization();

            services.AddDbContext<LinContext>(options =>
            {
                //options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection"),
                //    b => b.MigrationsAssembly("LinCms.Api"));

                options.UseMySql(Configuration.GetConnectionString("MySqlConnection"),
                    b => b.MigrationsAssembly("LinCms.Api"));
                //options.ConfigureWarnings(
                //    w => w.Ignore(CoreEventId.IncludeIgnoredWarning));
            });

            ////设置跨域
            //services.AddCors();
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
            builder.RegisterModule<DefaultModule>();
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
            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStatusCodeHandling();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
