using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LinCms.Core.Entities;
using LinCms.Core.RepositoryInterfaces;
using LinCms.Infrastructure.Database;

namespace LinCms.Infrastructure.Repositories
{
    public class LinLogRepository : ILinLogRepository
    {
        private readonly LinContext _linContext;

        public LinLogRepository(LinContext linContext)
        {
            _linContext = linContext;
        }

        public void Add(LinLog log)
        {
            _linContext.Add(log);
        }
    }
}
