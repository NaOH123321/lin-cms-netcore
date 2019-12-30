using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LinCms.Core.Entities;

namespace LinCms.Core.RepositoryInterfaces
{
    public interface ILinUserRepository
    {
        Task<LinUser> Verify(string username);

        bool CheckPermission(int uid, string permissionName);
    }
}
