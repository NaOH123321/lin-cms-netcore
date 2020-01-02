using System.Collections.Generic;
using System.Security.Claims;

namespace LinCms.Core.Interfaces
{
    public interface ITokenService
    {
        int GetDurationInMinutes();
        int GetRefreshDurationInMinutes();
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        ClaimsPrincipal GetPrincipalFromValidToken(string token);
    }
}