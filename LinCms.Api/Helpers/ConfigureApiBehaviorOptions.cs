using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using LinCms.Infrastructure.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LinCms.Api.Helpers
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
                    Msg = new BadRequestObjectResult(context.ModelState).Value
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
