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

        void Add(LinUser user);

        void ChangePassword(LinUser user, string password);
        void Update(LinUser user);

        void Delete(LinUser user);
    }
}
