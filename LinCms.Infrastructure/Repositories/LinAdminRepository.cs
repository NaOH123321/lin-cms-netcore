using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinCms.Core;
using LinCms.Core.Entities;
using LinCms.Core.EntityQueryParameters;
using LinCms.Core.RepositoryInterfaces;
using LinCms.Infrastructure.Database;
using LinCms.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace LinCms.Infrastructure.Repositories
{
    public class LinAdminRepository : ILinAdminRepository
    {
        private readonly LinContext _linContext;

        public LinAdminRepository(LinContext linContext)
        {
            _linContext = linContext;
        }

        public async Task<LinUser?> GetUserAsync(int id)
        {
            var query = _linContext.LinUsers
                .AsQueryable();

            query = query.Where(b => b.Id == id);

            var linUser = await query.SingleOrDefaultAsync();
            return linUser;
        }
        public async Task<LinGroup?> GetGroupWithAuthAndUserAsync(int id)
        {
            var query = _linContext.LinGroups
                .Include(g => g.LinAuths)
                .Include(g => g.LinUsers)
                .AsQueryable();

            query = query.Where(b => b.Id == id);

            var linGroup = await query.SingleOrDefaultAsync();
            return linGroup;
        }

        public async Task<PaginatedList<LinUser>> GetAllUsersWithGroupAsync(AdminParameters adminParameters)
        {
            var query = _linContext.LinUsers
                .Include(u => u.LinGroup)
                .AsQueryable();

            if (adminParameters.GroupId != null)
            {
                query = query.Where(u => u.GroupId == adminParameters.GroupId);
            }

            var total = await query.CountAsync();
            var data = await query
                .Skip(adminParameters.Start + adminParameters.Page * adminParameters.Count)
                .Take(adminParameters.Count)
                .ToListAsync();

            return new PaginatedList<LinUser>(adminParameters.Page, adminParameters.Count, total, data);
        }
        public async Task<PaginatedList<LinGroup>> GetAllGroupsWithAuthAsync(AdminParameters adminParameters)
        {
            var query = _linContext.LinGroups
                .Include(g => g.LinAuths)
                .AsQueryable();

            var total = await query.CountAsync();
            var data = await query
                .Skip(adminParameters.Start + adminParameters.Page * adminParameters.Count)
                .Take(adminParameters.Count)
                .ToListAsync();

            return new PaginatedList<LinGroup>(adminParameters.Page, adminParameters.Count, total, data);
        }
        public async Task<IEnumerable<LinGroup>> GetAllGroupsAsync()
        {
            var query = _linContext.LinGroups
                .AsQueryable();

            var data = await query.ToListAsync();

            return data;
        }

        public void Add(LinUser user)
        {
            user.Password = Pbkdf2Encrypt.EncryptPassword(user.Password!);
            _linContext.Add(user);
        }
        public void Add(LinGroup group)
        {
            _linContext.Add(group);
        }

        public void Update(LinUser user)
        {
            _linContext.Update(user);
        }
        public void Update(LinGroup group)
        {
            _linContext.Update(group);
        }

        public void Delete(LinUser user)
        {
            _linContext.Remove(user);
        }
        public void Delete(LinGroup group)
        {
            _linContext.Remove(group);
        }
        public void DeleteRange(IEnumerable<LinAuth> auths)
        {
            _linContext.RemoveRange(auths);
        }
    }
}
