using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using LinCms.Api.Exceptions;
using LinCms.Api.Helpers;
using LinCms.Api.Services;
using LinCms.Core;
using LinCms.Core.Entities;
using LinCms.Core.Interfaces;
using LinCms.Core.RepositoryInterfaces;
using LinCms.Infrastructure.Messages;
using LinCms.Infrastructure.Resources.LinUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinCms.Api.Controllers.Cms
{
    [Authorize]
    [Route("cms/user")]
    public class UserController : BasicController
    {
        private readonly ILinUserRepository _linUserRepository;
        private readonly ITokenService _tokenService;
        private readonly ILinLogger _linLogger;

        public UserController(ILinUserRepository linUserRepository, ITokenService tokenService, ILinLogger linLogger)
        {
            _linUserRepository = linUserRepository;
            _tokenService = tokenService;
            _linLogger = linLogger;
        }

        [HttpPost("register")]
        [Log("管理员新建了一个用户")]
        [PermissionMeta("注册", "用户", UserRole.Admin, false)]
        public async Task<ActionResult<string>> Register(LinUserAddResource linUserAddResource)
        {
            var user = MyMapper.Map<LinUserAddResource, LinUser>(linUserAddResource);

            _linUserRepository.Add(user);

            if (!await UnitOfWork.SaveAsync())
            {
                throw new Exception("Save Failed!");
            }

            return new CreatedMsg().ToJson();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [PermissionMeta("登陆", "用户", UserRole.Every, false)]
        public async Task<ActionResult<object>> Login(LinUserLoginResource linUserLoginResource)
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

            var result = GetTokenResult(user.Id.ToString());

            //记录日志
            _linLogger.AddLog(user.Id, user.Username, $"{user.Username}登陆成功获取了令牌", "登陆");

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("refresh")]
        [PermissionMeta("刷新令牌", "用户", UserRole.Every, false)]
        public ActionResult<object> Refresh()
        {
            string userId;
            try
            {
                var authorizationHeader = HttpContext.Request.Headers[HttpRequestHeader.Authorization.ToString()];
                var token = authorizationHeader.ToString().Split(" ")[1];
                var principal = _tokenService.GetPrincipalFromValidToken(token);
                userId = principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            }
            catch (Exception)
            {
                throw new UnauthorizedNotValidTokenException();
            }

            var result = GetTokenResult(userId);
            return Ok(result);
        }

        [HttpGet("information")]
        [PermissionMeta("查询自己信息", "用户", UserRole.Every, false)]
        public ActionResult<LinUserResource> Information()
        {
            var resource = MyMapper.Map<LinUserResource>(CurrentUser);
            return Ok(resource);
        }

        [HttpGet("auths")]
        [PermissionMeta("查询自己拥有的权限", "用户", UserRole.Every, false)]
        public ActionResult<LinUserWithAuthsResource> GetAllowedAuths()
        {
            var resource = MyMapper.Map<LinUserWithAuthsResource>(CurrentUser);
            return Ok(resource);
        }

        [HttpPut]
        [PermissionMeta("用户更新信息", "用户", UserRole.Every, false)]
        public ActionResult<LinUserResource> UpdateInformation()
        {
            var resource = MyMapper.Map<LinUserResource>(CurrentUser);

            return Ok(resource);
        }

        [HttpPut("change_password")]
        [Log("{user.username}修改了自己的密码")]
        [PermissionMeta("修改密码", "用户", UserRole.Every, false)]
        public ActionResult<LinUserResource> ChangePassword()
        {
            var resource = MyMapper.Map<LinUserResource>(CurrentUser);

            return Ok(resource);
        }

        private object GetTokenResult(string uid)
        {
            var result = new
            {
                accessToken = _tokenService.GenerateAccessToken(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, uid),
                    new Claim(TokenOption.Type, TokenOption.AccessType)
                }),
                refreshToken = _tokenService.GenerateAccessToken(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, uid),
                    new Claim(TokenOption.Type, TokenOption.RefreshType)
                })
            };

            return result;
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
