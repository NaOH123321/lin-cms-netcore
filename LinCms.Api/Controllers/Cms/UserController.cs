using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LinCms.Api.Exceptions;
using LinCms.Api.Helpers;
using LinCms.Core;
using LinCms.Core.Interfaces;
using LinCms.Core.RepositoryInterfaces;
using LinCms.Infrastructure.Helpers;
using LinCms.Infrastructure.Resources;
using Microsoft.AspNetCore.Mvc;

namespace LinCms.Api.Controllers.Cms
{
    [Route("cms/user")]
    public class UserController : BasicController
    {
        private readonly ILinUserRepository _linUserRepository;
        private readonly ITokenService _tokenService;

        public UserController(ILinUserRepository linUserRepository, ITokenService tokenService)
        {
            _linUserRepository = linUserRepository;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LinUserLoginResource linUserLoginResource)
        {
            var user = await _linUserRepository.Verify(linUserLoginResource.Username);
            if (user == null)
            {
                throw new NotFoundException
                {
                    ErrorCode = ResultCode.UserNotFoundErrorCode
                };
            }

            var encryptPassword = Pbkdf2Encrypt.EncryptPassword(linUserLoginResource.Password);
            if (encryptPassword != user.Password)
            {
                throw new NotFoundException
                {
                    ErrorCode = ResultCode.UserPasswordErrorCode
                };
            }

            if (user.Active != (short)UserActive.Active)
            {
                throw new UnauthorizedException()
                {
                    ErrorCode = ResultCode.UserInactiveErrorCode
                };
            }

            var result = new
            {
                token = _tokenService.GenerateAccessToken(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                refreshToken = _tokenService.GenerateRefreshToken(),
            };

            return Ok(result);
        }


        [HttpPost("login")]
        [RequestHeaderMatchingMediaType("content-type", new[] { "application/x-www-form-urlencoded" })]
        public async Task<IActionResult> LoginByForm([FromForm] LinUserLoginResource linUserLoginResource)
        {
            var user = await _linUserRepository.Verify(linUserLoginResource.Username);
            if (user == null)
            {
                throw new NotFoundException
                {
                    ErrorCode = ResultCode.UserNotFoundErrorCode
                };
            }

            var encryptPassword = Pbkdf2Encrypt.EncryptPassword(linUserLoginResource.Password);
            if (encryptPassword != user.Password)
            {
                throw new NotFoundException
                {
                    ErrorCode = ResultCode.UserPasswordErrorCode
                };
            }

            if (user.Active != (short) UserActive.Active)
            {
                throw new UnauthorizedException()
                {
                    ErrorCode = ResultCode.UserInactiveErrorCode
                };
            }

            var result = new
            {
                token = _tokenService.GenerateAccessToken(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                refreshToken = _tokenService.GenerateRefreshToken(),
            };

            return Ok(result);
        }
    }
}
