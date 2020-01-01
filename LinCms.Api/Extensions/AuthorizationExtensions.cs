using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using LinCms.Api.Helpers;
using LinCms.Api.Services;

namespace LinCms.Api.Extensions
{
    public static class AuthorizationExtensions
    {
        public static void AddMyAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization();
            services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
        }
    }
}
