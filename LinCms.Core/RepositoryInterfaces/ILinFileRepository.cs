using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LinCms.Core.Entities;

namespace LinCms.Core.RepositoryInterfaces
{
    public interface ILinFileRepository
    {
        Task<LinFile?> GetFileByMd5(string md5);

        void Add(LinFile linFile);
    }
}
