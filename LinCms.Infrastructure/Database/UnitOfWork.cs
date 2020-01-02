using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LinCms.Infrastructure.Database;
using LinCms.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LinCms.Infrastructure.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LinContext _linContext;

        public UnitOfWork(LinContext linContext)
        {
            _linContext = linContext;
        }

        public bool Save()
        {
            return  _linContext.SaveChanges() > 0;
        }

        public async Task<bool> SaveAsync()
        {
            return await _linContext.SaveChangesAsync() > 0;
        }
    }
}
