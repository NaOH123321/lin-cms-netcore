using System;
using System.Collections.Generic;
using System.Text;
using LinCms.Core.Entities;

namespace LinCms.Core.Interfaces
{
    public interface ICurrentUser
    {
        int Id { get; }
        string Username { get; }
        string? Nickname { get; }
        string? Avatar { get; }
        string? Email { get; }
        bool IsAdmin { get; }
        bool IsActive { get; }
        string Role { get; }
        string? GroupName { get; }
        List<LinAuth> Auths { get; }

        bool CheckPermission(string? authName);
        bool CheckRole(string roleName);
    }
}
