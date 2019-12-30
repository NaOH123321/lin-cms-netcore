using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LinCms.Core.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> SaveAsync();
    }
}
