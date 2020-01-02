using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using LinCms.Core;
using LinCms.Core.Interfaces;
using LinCms.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LinCms.Api.Services
{
    public class CurrentUser : ICurrentUser
    {
        public string? Token { get; }

        public int Id { get; }
        public string Username { get; } = null!;
        public string? Nickname { get; }
        public string? Avatar { get; }
        public string? Email { get; }
        public bool IsAdmin { get; }
        public bool IsActive { get; }
        public string Role { get; } = null!;
        public string? GroupName { get; }
        public List<string> Auths { get; } = new List<string>();

        public CurrentUser(IHttpContextAccessor httpContextAccessor, LinContext linContext)
        {
            try
            {
                var headers = httpContextAccessor.HttpContext.Request.Headers;
                var authorizationHeader = headers[HttpRequestHeader.Authorization.ToString()];
                Token = authorizationHeader.ToString().Split(" ")[1];
            }
            catch (Exception)
            {
                Token = null;
                return;
            }

            var claims = httpContextAccessor.HttpContext.User.Claims;
            var uid = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(uid, out var id)) return;

            var user = linContext.LinUsers
                .Include(u => u.LinGroup)
                .ThenInclude(g => g.LinAuths)
                .FirstOrDefault(u => u.Id == id);
            if (user == null) return;

            Id = user.Id;
            Username = user.Username;
            Nickname = user.Nickname;
            Avatar = user.Avatar;
            Email = user.Email;
            IsAdmin = user.Admin == (short) UserAdmin.Admin;
            IsActive = user.Active == (short) UserActive.Active;

            if (!IsAdmin)
            {
                Role = user.GroupId != null ? UserRole.Group : UserRole.Every;
            }
            else
            {
                Role = UserRole.Admin;
            }

            var linGroup = user.LinGroup;
            if (linGroup == null) return;

            GroupName = linGroup.Name;
            foreach (var linAuth in linGroup.LinAuths)
            {
                if (linAuth.Auth != null)
                {
                    Auths.Add(linAuth.Auth);
                }
            }
        }

        public bool CheckPermission(string? authName)
        {
            return authName != null && Auths.Contains(authName);
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
