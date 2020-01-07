using System;
using System.Collections.Generic;
using System.Text;
using LinCms.Core.Entities;

namespace LinCms.Core.RepositoryInterfaces
{
    public interface ILinFileRepository
    {
        void Add(LinFile linFile);

        void AddRange(List<LinFile> linFiles);
    }
}
