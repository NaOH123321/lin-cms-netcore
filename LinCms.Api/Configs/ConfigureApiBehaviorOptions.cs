﻿using System.Net.Mime;
using LinCms.Infrastructure.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LinCms.Api.Configs
{
    public class ConfigureApiBehaviorOptions : IConfigureNamedOptions<ApiBehaviorOptions>
    {
        public void Configure(ApiBehaviorOptions options)
        {
            Configure(string.Empty, options);
        }

        public void Configure(string name, ApiBehaviorOptions options)
        {
            options.SuppressMapClientErrors = true;
            options.InvalidModelStateResponseFactory = context =>
            {
                var badRequestMsg = new BadRequestMsg
                {
                    Msg = new ValidationProblemDetails(context.ModelState).Errors
                };

                var result = new ObjectResult(badRequestMsg)
                {
                    StatusCode = badRequestMsg.Code
                };
                result.ContentTypes.Add(MediaTypeNames.Application.Json);

                return result;
            };
        }
    }
}
