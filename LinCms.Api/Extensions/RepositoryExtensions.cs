using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LinCms.Core.Interfaces;
using LinCms.Core.RepositoryInterfaces;
using LinCms.Infrastructure.Database;
using LinCms.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace LinCms.Api.Extensions
{
    public static class RepositoryExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ILinUserRepository, LinUserRepository>();
            services.AddScoped<ILinLogRepository, LinLogRepository>();
            services.AddScoped<IBookRepository, BookRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
