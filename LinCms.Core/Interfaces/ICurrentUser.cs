using System;
using System.Collections.Generic;
using System.Text;

namespace LinCms.Core.Interfaces
{
    public interface ICurrentUser
    {
        string? Token { get; }

        int Id { get; }
        string Username { get; }
        string? Nickname { get; }
        string? Avatar { get; }
        string? Email { get; }
        bool IsAdmin { get; }
        bool IsActive { get; }
        string Role { get; }
        string? GroupName { get; }
        List<string> Auths { get; }

        bool CheckPermission(string? authName);
        bool CheckRole(string roleName);
    }
}
