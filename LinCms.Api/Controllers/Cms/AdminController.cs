using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinCms.Api.Exceptions;
using LinCms.Api.Helpers;
using LinCms.Core;
using LinCms.Core.Entities;
using LinCms.Core.EntityQueryParameters;
using LinCms.Core.Interfaces;
using LinCms.Core.RepositoryInterfaces;
using LinCms.Infrastructure.Resources.LinGroups;
using LinCms.Infrastructure.Resources.LinUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinCms.Api.Controllers.Cms
{
    [PermissionMeta(UserRole.Admin)]
    [Route("cms/admin")]
    public class AdminController : BasicController
    {
        private readonly ILinAdminRepository _adminRepository;

        public AdminController(ILinAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        [HttpGet("authority")]
        [PermissionMeta("查询所有可分配的权限", "管理员", mount:false)]
        public ActionResult<IEnumerable<PermissionMeta>> GetAllAuthorities()
        {
            var dispatchedMetas = PermissionMetaHandler.GetAllDispatchedMetas();
            var resource = MyMapper.Map<IEnumerable<PermissionMeta>, IEnumerable<PermissionMeta>>(dispatchedMetas);
            return Ok(resource);
        }


        [HttpGet("users")]
        [PermissionMeta("查询所有用户", "管理员", mount: false)]
        public async Task<ActionResult<PaginatedResult<LinUserResource>>> GetAllUsers([FromQuery] AdminParameters adminParameters)
        {
            var list = await _adminRepository.GetAllUsersWithGroupAsync(adminParameters);

            var resources = MyMapper.Map<IEnumerable<LinUser>, IEnumerable<LinUserResource>>(list);

            var result = WrapPaginatedResult(list, resources);

            return Ok(result);
        }

        [HttpDelete("{uid}")]
        [Log("管理员删除了一个用户")]
        [PermissionMeta("删除用户", "管理员", mount: false)]
        public async Task<ActionResult> DeleteUser(int uid)
        {
            var user = await _adminRepository.GetUserAsync(uid);

            if (user == null)
            {
                throw new NotFoundException
                {
                    ErrorCode = ResultCode.UserNotFoundErrorCode
                };
            }

            _adminRepository.Delete(user);

            if (!await UnitOfWork.SaveAsync())
            {
                throw new Exception("Save Failed!");
            }

            return NoContent();
        }

        [HttpPut("disable/{uid}")]
        [PermissionMeta("禁用用户", "管理员", mount: false)]
        public async Task<ActionResult> DisableUser(int uid)
        {
            var user = await _adminRepository.GetUserAsync(uid);

            if (user == null)
            {
                throw new NotFoundException
                {
                    ErrorCode = ResultCode.UserNotFoundErrorCode
                };
            }

            if (user.Active == (short) UserActive.NotActive)
            {
                throw new ForbiddenException
                {
                    ErrorCode = ResultCode.UserShouldBeActiveErrorCode
                };
            }

            _adminRepository.Update(user);

            if (!await UnitOfWork.SaveAsync())
            {
                throw new Exception("Save Failed!");
            }

            return NoContent();
        }

        [HttpPut("active/{uid}")]
        [PermissionMeta("激活用户", "管理员", mount: false)]
        public async Task<ActionResult> ActiveUser(int uid)
        {
            var user = await _adminRepository.GetUserAsync(uid);

            if (user == null)
            {
                throw new NotFoundException
                {
                    ErrorCode = ResultCode.UserNotFoundErrorCode
                };
            }

            if (user.Active == (short)UserActive.Active)
            {
                throw new ForbiddenException
                {
                    ErrorCode = ResultCode.UserShouldBeNotActiveErrorCode
                };
            }

            _adminRepository.Update(user);

            if (!await UnitOfWork.SaveAsync())
            {
                throw new Exception("Save Failed!");
            }

            return NoContent();
        }


        [HttpGet("groups")]
        [PermissionMeta("查询所有权限组及其权限", "管理员", mount: false)]
        public async Task<ActionResult<PaginatedResult<LinGroupWithAuthsResource>>> GetAdminGroups([FromQuery] AdminParameters adminParameters)
        {
            var list = await _adminRepository.GetAllGroupsWithAuthAsync(adminParameters);

            var resources = MyMapper.Map<IEnumerable<LinGroup>, IEnumerable<LinGroupWithAuthsResource>>(list);

            var result = WrapPaginatedResult(list, resources);

            return Ok(result);
        }

        [HttpGet("group/all")]
        [PermissionMeta("查询所有权限组", "管理员", mount: false)]
        public async Task<ActionResult<IEnumerable<LinGroupResource>>> GetAllGroups()
        {
            var list = await _adminRepository.GetAllGroupsAsync();

            var resources = MyMapper.Map<IEnumerable<LinGroup>, IEnumerable<LinGroupResource>>(list);

            return Ok(resources);
        }

        [HttpGet("group/{gid}")]
        [PermissionMeta("查询一个权限组及其权限", "管理员", mount: false)]
        public async Task<ActionResult<LinGroupWithAuthsResource>> GetGroup(int gid)
        {
            var group = await _adminRepository.GetGroupWithAuthAndUserAsync(gid);

            if (group == null)
            {
                throw new NotFoundException
                {
                    ErrorCode = ResultCode.GroupNotFoundErrorCode
                };
            }

            var resource = MyMapper.Map<LinGroup, LinGroupWithAuthsResource>(group);

            return Ok(resource);
        }



        [HttpDelete("group/{pid}")]
        [Log("管理员删除一个权限组")]
        [PermissionMeta("删除一个权限组", "管理员", mount: false)]
        public async Task<ActionResult> DeleteGroup(int pid)
        {
            var group = await _adminRepository.GetGroupWithAuthAndUserAsync(pid);

            if (group == null)
            {
                throw new NotFoundException
                {
                    ErrorCode = ResultCode.GroupNotFoundErrorCode
                };
            }

            if (group.LinUsers.Count > 0)
            {
                throw new ForbiddenException
                {
                    ErrorCode = ResultCode.GroupHasUserErrorCode
                };
            }

            _adminRepository.Delete(group);
            _adminRepository.DeleteRange(group.LinAuths);

            if (!await UnitOfWork.SaveAsync())
            {
                throw new Exception("Save Failed!");
            }

            return NoContent();
        }
    }
}
