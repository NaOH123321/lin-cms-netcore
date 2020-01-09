using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinCms.Core.Entities;
using LinCms.Core.Interfaces;
using LinCms.Core.RepositoryInterfaces;
using LinCms.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace LinCms.Infrastructure.Repositories
{
    public class LinFileRepository : ILinFileRepository, IRepository
    {
        private readonly LinContext _linContext;

        public LinFileRepository(LinContext linContext)
        {
            _linContext = linContext;
        }

        public async Task<LinFile?> GetFileByMd5(string md5)
        {
            var query = _linContext.LinFiles
                .AsQueryable();

            query = query.Where(f => f.Md5 == md5);

            var linFile = await query.SingleOrDefaultAsync();
            return linFile;
        }

        public void Add(LinFile linFile)
        {
            _linContext.Add(linFile);
        }
    }
}
