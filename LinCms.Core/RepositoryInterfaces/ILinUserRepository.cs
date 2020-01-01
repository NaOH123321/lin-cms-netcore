using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LinCms.Core.Entities;

namespace LinCms.Core.RepositoryInterfaces
{
    public interface ILinUserRepository
    {
        Task<LinUser?> GetDetailAsync(int id);

        Task<LinUser?> Verify(string username, string password);

        Task<bool> CheckPermission(int uid, string permissionName);

        void Add(LinUser user);

        void Update(LinUser user);

        void Delete(LinUser user);
    }
}
