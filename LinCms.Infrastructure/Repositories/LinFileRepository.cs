using System;
using System.Collections.Generic;
using System.Text;
using LinCms.Core.Entities;
using LinCms.Core.Interfaces;
using LinCms.Core.RepositoryInterfaces;
using LinCms.Infrastructure.Database;

namespace LinCms.Infrastructure.Repositories
{
    public class LinFileRepository : ILinFileRepository, IRepository
    {
        private readonly LinContext _linContext;

        public LinFileRepository(LinContext linContext)
        {
            _linContext = linContext;
        }

        public void Upload()
        {

        }

        public void Add(LinFile linFile)
        {
            _linContext.Add(linFile);
        }
        public void AddRange(List<LinFile> linFiles)
        {
            _linContext.AddRange(linFiles);
        }
    }
}
