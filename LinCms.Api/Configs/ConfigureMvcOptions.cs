using LinCms.Api.Helpers;
using LinCms.Core.Interfaces;
using LinCms.Core.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LinCms.Api.Configs
{
    public class ConfigureMvcOptions : IConfigureNamedOptions<MvcOptions>
    {
        private readonly ILinLogger _linLogger;

        public ConfigureMvcOptions(ILinLogger linLogger)
        {
            _linLogger = linLogger;
        }

        public void Configure(MvcOptions options)
        {
            Configure(string.Empty, options);
        }

        public void Configure(string name, MvcOptions options)
        {
            options.ReturnHttpNotAcceptable = true; //开启accept验证
            options.Filters.Add(new HttpResponseExceptionFilter());
            options.Filters.Add(new LogActionFilter(_linLogger));

            //改变model的验证信息
            options.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => "请求的body不能为空");
            options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(value => $"{value}不是有效的值");
        }
    }
}
