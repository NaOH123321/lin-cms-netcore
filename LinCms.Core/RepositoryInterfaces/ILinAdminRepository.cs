using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LinCms.Core.Entities;
using LinCms.Core.EntityQueryParameters;

namespace LinCms.Core.RepositoryInterfaces
{
    public interface ILinAdminRepository
    {
        Task<LinUser?> GetUserAsync(int id);
        Task<LinGroup?> GetGroupWithAuthAndUserAsync(int id);

        Task<PaginatedList<LinUser>> GetAllUsersWithGroupAsync(AdminParameters adminParameters);
        Task<PaginatedList<LinGroup>> GetAllGroupsWithAuthAsync(AdminParameters adminParameters);
        Task<IEnumerable<LinUser>> GetAllUsersAsync();
        Task<IEnumerable<LinGroup>> GetAllGroupsAsync();
        Task<IEnumerable<LinAuth>> GetAllAuthsAsync();

        void Add(LinUser user);
        void Add(LinGroup group);
        void AddRange(IEnumerable<LinAuth> auths);

        void ResetPassword(LinUser user, string password);
        void Update(LinUser user);
        void Update(LinGroup group);

        void Delete(LinUser user);
        void Delete(LinGroup group);
        void DeleteRange(IEnumerable<LinAuth> auths);
    }
}
