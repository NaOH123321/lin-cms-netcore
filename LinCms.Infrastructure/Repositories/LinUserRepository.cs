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

        public async Task<LinUser> GetDetailAsync(int id)
        {
            var query = _linContext.LinUsers
                .AsQueryable();

            query = query.Where(b => b.Id == id);

            var linUser = await query.SingleOrDefaultAsync();
            return linUser;
        }

        public async Task<LinUser> Verify(string username, string password)
        {
            var query = _linContext.LinUsers
                .AsQueryable();

            var encryptPassword = Pbkdf2Encrypt.EncryptPassword(password);
            query = query.Where(u => u.Username == username && u.Password== encryptPassword);

            var user = await query.SingleOrDefaultAsync();
            return user;
        }

        public async Task<bool> CheckPermission(int uid, string permissionName)
        {
            var currentUser = _linContext.LinUsers.Find(uid);

            var linAuth = await _linContext.LinAuths.FirstOrDefaultAsync(auth =>
                auth.GroupId == currentUser.GroupId && auth.Auth == permissionName);

            return linAuth != null;
        }

        public void Add(LinUser user)
        {
            _linContext.Add(user);
        }
        public void Update(LinUser user)
        {
            _linContext.Update(user);
        }

        public void Delete(LinUser user)
        {
            _linContext.Remove(user);
        }
    }
}
