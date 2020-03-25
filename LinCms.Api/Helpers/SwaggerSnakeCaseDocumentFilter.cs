using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LinCms.Api.Helpers
{
    public class SwaggerSnakeCaseDocumentFilter : IDocumentFilter
    {
        private readonly SnakeCaseNamingStrategy _namingStrategy = new SnakeCaseNamingStrategy();

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var path in swaggerDoc.Paths.ToArray())
            {
                var apiDescription = context.ApiDescriptions.First(ad => "/" + ad.RelativePath == path.Key);
                var newKey = path.Key;
                foreach (var parameterDescription in apiDescription.ParameterDescriptions.Where(pd =>
                    pd.Source == BindingSource.Path))
                {
                    newKey = newKey.Replace(
                        "{" + parameterDescription.Name + "}",
                        "{" + _namingStrategy.GetPropertyName(parameterDescription.Name, false) + "}");
                }

                swaggerDoc.Paths.Remove(path.Key);
                swaggerDoc.Paths.Add(newKey, path.Value);
            }
        }
    }
}
