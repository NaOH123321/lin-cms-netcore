using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LinCms.Infrastructure.Messages;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LinCms.Api.Helpers
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
                    OnTokenValidated = context =>
                    {
                        var ssd = context.Principal.Claims;
                        return Task.CompletedTask;
                    },

                    //OnAuthenticationFailed = context =>
                    //{
                    //    context.Response.ContentType = MediaTypeNames.Application.Json;
                    //    context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                    //    if (context.Exception is SecurityTokenInvalidSignatureException ||
                    //        context.Exception is ArgumentException)
                    //    {
                    //        context.Response.WriteAsync(new UnauthorizedNotValidTokenMsg().ToJson());
                    //    }

                    //    if (context.Exception is SecurityTokenExpiredException)
                    //    {
                    //        context.Response.WriteAsync(new UnauthorizedTokenTimeoutMsg().ToJson());
                    //    }
                    //    return Task.CompletedTask;
                    //},

                    OnChallenge = context =>
                    {
                        context.HandleResponse();

                        var sd = context.AuthenticateFailure;

                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                        //context.Response.WriteAsync("");

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