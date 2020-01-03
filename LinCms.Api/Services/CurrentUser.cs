using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using LinCms.Core;
using LinCms.Core.Entities;
using LinCms.Core.Interfaces;
using LinCms.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LinCms.Api.Services
{
    public class CurrentUser : ICurrentUser
    {
        public int Id { get; private set; }
        public string Username { get; private set; } = null!;
        public string? Nickname { get; private set; }
        public string? Avatar { get; private set; }
        public string? Email { get; private set; }
        public bool IsAdmin { get; private set; }
        public bool IsActive { get; private set; }
        public string Role { get; private set; } = null!;
        public int? GroupId { get; private set; }
        public string? GroupName { get; private set; }
        public List<LinAuth> Auths { get; private set; } = new List<LinAuth>();

        public CurrentUser(IHttpContextAccessor httpContextAccessor, LinContext linContext)
        {
            if (httpContextAccessor.HttpContext == null) return;
            var claims = httpContextAccessor.HttpContext.User.Claims;
            var uid = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(uid, out var id)) return;

            var user = linContext.LinUsers
                .Include(u => u.LinGroup)
                .ThenInclude(g => g.LinAuths)
                .FirstOrDefault(u => u.Id == id);
            if (user == null) return;
            Init(user);
        }

        private void Init(LinUser user)
        {
            Id = user.Id;
            Username = user.Username;
            Nickname = user.Nickname;
            Avatar = user.Avatar;
            Email = user.Email;
            IsAdmin = user.Admin == (short)UserAdmin.Admin;
            IsActive = user.Active == (short)UserActive.Active;

            SetRole(user.GroupId);
            SetGroupAndAuth(user.LinGroup);
        }

        private void SetRole(int? groupId)
        {
            if (!IsAdmin)
            {
                Role = groupId != null ? UserRole.Group : UserRole.Every;
            }
            else
            {
                Role = UserRole.Admin;
            }
        }

        private void SetGroupAndAuth(LinGroup? group)
        {
            if (group == null) return;

            GroupId = group.Id;
            GroupName = group.Name;
            Auths = group.LinAuths.ToList();

            //foreach (var linAuth in group.LinAuths)
            //{
            //    if (linAuth.Auth != null)
            //    {
            //        Auths.Add(linAuth);
            //    }
            //}
        }

        public bool CheckPermission(string? authName)
        {
            return authName != null && Auths.Select(a => a.Auth).Contains(authName);
        }

        public bool CheckRole(string roleName)
        {
            switch (Role)
            {
                case UserRole.Admin:
                case UserRole.Group when roleName != UserRole.Admin:
                    return true;
                case UserRole.Every when roleName == UserRole.Every:
                    return true;
                default:
                    return false;
            }
        }
    }
}
