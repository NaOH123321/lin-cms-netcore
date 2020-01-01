using System;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LinCms.Api.Services;
using LinCms.Core.Interfaces;
using LinCms.Core.RepositoryInterfaces;
using LinCms.Infrastructure.Messages;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LinCms.Api.Configs
{
    public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
    {
        public IConfiguration Configuration { get; }

        public ConfigureJwtBearerOptions(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Configure(JwtBearerOptions options)
        {
            Configure(string.Empty, options);
        }

        public void Configure(string name, JwtBearerOptions options)
        {
            if (name == JwtBearerDefaults.AuthenticationScheme)
            {
                options.Events = new JwtBearerEvents()
                {
                    //OnMessageReceived = context =>
                    //{
                    //    context.Token = context.Request.Query["access_token"];
                    //    return Task.CompletedTask;
                    //},

                    OnChallenge = context =>
                    {
                        var exception = context.AuthenticateFailure;

                        if (exception == null)
                        {
                            return Task.CompletedTask;
                        }

                        context.HandleResponse();
                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                        if (exception is SecurityTokenValidationException ||
                            exception is ArgumentException)
                        {
                            context.Response.WriteAsync(new UnauthorizedNotValidTokenMsg().ToJson());
                        }

                        if (exception is SecurityTokenExpiredException)
                        {
                            context.Response.WriteAsync(new UnauthorizedTokenTimeoutMsg().ToJson());
                        }

                        return Task.CompletedTask;
                    }
                };

                var serverSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:ServerSecret"]));
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = Configuration["JWT:Issuer"],
                    ValidAudience = Configuration["JWT:Audience"],
                    IssuerSigningKey = serverSecret,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            }
        }
    }
}