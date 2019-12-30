using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinCms.Core.Entities;
using LinCms.Core.RepositoryInterfaces;
using LinCms.Infrastructure.Database;
using LinCms.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace LinCms.Infrastructure.Repositories
{
    public class LinUserRepository : ILinUserRepository
    {
        private readonly LinContext _linContext;

        public LinUserRepository(LinContext linContext)
        {
            _linContext = linContext;
        }

        public async Task<LinUser> Verify(string username)
        {
            var query = _linContext.LinUsers
                .AsQueryable();

            query = query.Where(u => u.Username == username);

            var user = await query.SingleOrDefaultAsync();
            return user;
        }

        public bool CheckPermission(int uid, string permissionName)
        {
            var currentUser = _linContext.LinUsers.Find(uid);

            var linAuth = _linContext.LinAuths.FirstOrDefault(auth =>
                auth.GroupId == currentUser.GroupId && auth.Auth == permissionName);

            return linAuth != null;
        }
    }
}
