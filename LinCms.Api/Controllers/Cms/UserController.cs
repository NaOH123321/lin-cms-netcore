using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LinCms.Api.Exceptions;
using LinCms.Api.Helpers;
using LinCms.Core;
using LinCms.Core.Entities;
using LinCms.Core.Interfaces;
using LinCms.Core.RepositoryInterfaces;
using LinCms.Infrastructure.Resources.LinUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinCms.Api.Controllers.Cms
{
    [Authorize]
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

        [HttpPost("register")]
        [Log("管理员新建了一个用户")]
        [PermissionMeta("注册", "用户", "Admin", false)]
        public async Task<ActionResult<LinUserResource>> Register(LinUserAddResource linUserAddResource)
        {
            var user = MyMapper.Map<LinUserAddResource, LinUser>(linUserAddResource);

            _linUserRepository.Add(user);

            if (!await UnitOfWork.SaveAsync())
            {
                throw new Exception("Save Failed!");
            }

            var resource = MyMapper.Map<LinUserResource>(user);
            return Ok(resource);
            //return CreatedAtRoute("GetBook", new { id = resource.Id }, resource);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login(LinUserLoginResource linUserLoginResource)
        {
            var user = await _linUserRepository.Verify(linUserLoginResource.Username, linUserLoginResource.Password);
            if (user == null)
            {
                throw new BadRequestException
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
                    new Claim(ClaimTypes.Name, user.Username),

                    new Claim("asd", new {usr=user.Admin }.ToString()),
                    new Claim(ClaimTypes.Role, ((UserAdmin)user.Admin).ToString())
                }),
                refreshToken = _tokenService.GenerateRefreshToken(),
            };

            return Ok(result);
        }

        #region application/x-www-form-urlencoded登录测试方法
        [AllowAnonymous]
        [HttpPost("login")]
        [RequestHeaderMatchingMediaType("content-type", new[] { "application/x-www-form-urlencoded" })]
        public async Task<IActionResult> LoginByForm([FromForm] LinUserLoginResource linUserLoginResource)
        {
            var user = await _linUserRepository.Verify(linUserLoginResource.Username, linUserLoginResource.Password);
            if (user == null)
            {
                throw new BadRequestException
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
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, ((UserAdmin)user.Admin).ToString())
                }),
                refreshToken = _tokenService.GenerateRefreshToken(),
            };

            return Ok(result);
        }
        #endregion
    }
}
