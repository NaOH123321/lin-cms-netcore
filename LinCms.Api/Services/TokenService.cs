using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LinCms.Core;
using LinCms.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LinCms.Api.Services
{
    public class TokenService : ITokenService
    {
        public IConfiguration Configuration { get; }

        public SecurityKey ServerSecret { get; }
        public string Issuer { get; }
        public string Audience { get; }

        public TokenService(IConfiguration configuration)
        {
            Configuration = configuration;

             ServerSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:ServerSecret"]));
             Issuer = Configuration["JWT:Issuer"];
             Audience = Configuration["JWT:Audience"];
        }

        public int GetDurationInMinutes()
        {
            return int.Parse(Configuration["JWT:TokenDurationInMinutes"]);
        }

        public int GetRefreshDurationInMinutes()
        {
            return int.Parse(Configuration["JWT:RefreshTokenDurationInMinutes"]);
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var now = DateTime.Now;
            var identity = new ClaimsIdentity(claims);
            var signingCredentials = new SigningCredentials(ServerSecret, SecurityAlgorithms.HmacSha256);
            var handler = new JwtSecurityTokenHandler();

            var token = handler.CreateJwtSecurityToken(
                Issuer,
                Audience,
                identity,
                now,
                DateTime.Now.AddMinutes(GetDurationInMinutes()),
                now,
                signingCredentials
            );
            var jwtToken = handler.WriteToken(token);
            return jwtToken;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = Issuer,
                ValidAudience = Audience,
                IssuerSigningKey = ServerSecret,
                ValidateLifetime = false
            };

            return GetPrincipalFromToken(tokenValidationParameters, token);
        }

        public ClaimsPrincipal GetPrincipalFromValidToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = Issuer,
                ValidAudience = Audience,
                IssuerSigningKey = ServerSecret,
            };

            return GetPrincipalFromToken(tokenValidationParameters, token);
        }

        private ClaimsPrincipal GetPrincipalFromToken(TokenValidationParameters parameters , string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, parameters, out var securityToken);
            if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
