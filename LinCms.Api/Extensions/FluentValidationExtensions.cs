using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using LinCms.Infrastructure.Resources;
using LinCms.Infrastructure.Resources.Books;
using LinCms.Infrastructure.Resources.LinUsers;
using LinCms.Infrastructure.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace LinCms.Api.Extensions
{
    public static class FluentValidationExtensions
    {
        public static void AddFluentValidators(this IServiceCollection services)
        {
            services.AddTransient<IValidator<LinUserAddResource>, LinUserAddResourceValidator>();
            services.AddTransient<IValidator<LinUserLoginResource>, LinUserLoginResourceValidator>();
            //校验资源
            services.AddTransient<IValidator<BookAddResource>, BookAddOrUpdateResourceValidator>();
            services.AddTransient<IValidator<BookUpdateResource>, BookAddOrUpdateResourceValidator>();
        }
    }
}
