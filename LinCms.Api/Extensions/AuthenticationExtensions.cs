using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinCms.Api.Configs;
using LinCms.Api.Helpers;
using LinCms.Api.Services;
using LinCms.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LinCms.Api.Extensions
{
    public static class AuthenticationExtensions
    {
        public static void AddMyAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, configureOptions: null);
            //验证JwtBearer的相关配置
            services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
            //注册token相关的服务
            services.AddTransient<ITokenService, TokenService>();
        }
    }
}
