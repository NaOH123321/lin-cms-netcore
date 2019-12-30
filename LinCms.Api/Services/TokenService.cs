using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LinCms.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LinCms.Api.Services
{
    public class TokenService : ITokenService
    {
        public IConfiguration Configuration { get; }

        public TokenService(IConfiguration configuration)
        {
            Configuration = configuration;
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
            var serverSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:ServerSecret"]));
            var now = DateTime.Now;
            var issuer = Configuration["JWT:Issuer"];
            var audience = Configuration["JWT:Audience"];
            var identity = new ClaimsIdentity(claims);
            var signingCredentials = new SigningCredentials(serverSecret, SecurityAlgorithms.HmacSha256);
            var handler = new JwtSecurityTokenHandler();

            var token = handler.CreateJwtSecurityToken(
                issuer,
                audience,
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
                ValidIssuer = Configuration["JWT:Issuer"],
                ValidAudience = Configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:ServerSecret"])),
                ValidateLifetime = false 
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
