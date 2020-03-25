using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LinCms.Api.Helpers
{
    public class SwaggerSnakeCaseParameterFilter: IParameterFilter
    {
        private readonly SnakeCaseNamingStrategy _namingStrategy = new SnakeCaseNamingStrategy();

        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            parameter.Name = _namingStrategy.GetPropertyName(parameter.Name, false);
        }
    }
}
