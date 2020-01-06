using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinCms.Api.Exceptions;
using LinCms.Api.Helpers;
using LinCms.Api.Services;
using LinCms.Core;
using LinCms.Core.Entities;
using LinCms.Core.EntityQueryParameters;
using LinCms.Core.Enums;
using LinCms.Core.Interfaces;
using LinCms.Core.RepositoryInterfaces;
using LinCms.Infrastructure.Messages;
using LinCms.Infrastructure.Resources.LinGroups;
using LinCms.Infrastructure.Resources.LinUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinCms.Api.Controllers.Cms
{
    /// <summary>
    /// 管理员接口
    /// </summary>
    [PermissionMeta(UserRole.Admin)]
    [Route("cms/admin")]
    public class AdminController : BasicController
    {
        private readonly ILinAdminRepository _adminRepository;

        public AdminController(ILinAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        /// <summary>
        /// 查询所有可分配的权限
        /// </summary>
        /// <returns>A newly created TodoItem</returns>
        /// <response code="200">返回可分配的权限的列表</response>
        [HttpGet("authority")]
        [PermissionMeta("查询所有可分配的权限", "管理员", mount:false)]
        public ActionResult<Dictionary<string, Dictionary<string, IEnumerable<string>>>> GetAllAuthorities()
        {
            var dispatchedMetas = PermissionMetaHandler.GetAllDispatchedMetas();
            var resource = MyMapper.Map<IEnumerable<PermissionMeta>, Dictionary<string, Dictionary<string, IEnumerable<string>>>>(dispatchedMetas);
            return Ok(resource);
        }

        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <param name="adminParameters"></param>
        /// <returns></returns>
        [HttpGet("users")]
        [PermissionMeta("查询所有用户", "管理员", mount: false)]
        public async Task<ActionResult<PaginatedResult<LinUserResource>>> GetAllUsers([FromQuery] AdminParameters adminParameters)
        {
            var list = await _adminRepository.GetAllUsersWithGroupAsync(adminParameters);

            var resources = MyMapper.Map<IEnumerable<LinUser>, IEnumerable<LinUserResource>>(list);

            var result = WrapPaginatedResult(list, resources);

            return Ok(result);
        }

        [HttpPut("{uid}")]
        [PermissionMeta("管理员更新用户信息", "管理员", mount: false)]
        public async Task<ActionResult<LinUserResource>> UpdateUser(int uid, LinUserUpdateByAdminResource linUserUpdateByAdminResource)
        {
            var user = await _adminRepository.GetUserAsync(uid);

            if (user == null)
            {
                throw new NotFoundException
                {
                    ErrorCode = ResultCode.UserNotFoundErrorCode
                };
            }

            if (linUserUpdateByAdminResource.Email != user.Email)
            {
                var users =await _adminRepository.GetAllUsersAsync();

                if (users.Any(u => u.Email == linUserUpdateByAdminResource.Email))
                {
                    throw new BadRequestException
                    {
                        ErrorCode = ResultCode.UserEmailExistedErrorCode
                    };
                }
            }

            MyMapper.Map(linUserUpdateByAdminResource, user);

            _adminRepository.Update(user);

            if (!await UnitOfWork.SaveAsync())
            {
                throw new Exception("Save Failed!");
            }

            var resource = MyMapper.Map<LinUserResource>(user);

            return Ok(resource);
        }

        [HttpPut("password/{uid}")]
        [PermissionMeta("修改用户密码", "管理员", mount: false)]
        public async Task<ActionResult<OkMsg>> ChangeUserPassword(int uid, ResetPasswordByAdminResource resetPasswordByAdminResource)
        {
            var user = await _adminRepository.GetUserAsync(uid);

            if (user == null)
            {
                throw new NotFoundException
                {
                    ErrorCode = ResultCode.UserNotFoundErrorCode
                };
            }

            _adminRepository.ResetPassword(user, resetPasswordByAdminResource.Password);

            if (!await UnitOfWork.SaveAsync())
            {
                throw new Exception("Save Failed!");
            }

            return Ok(new OkMsg
            {
                Msg = "密码修改成功"
            });
        }

        [HttpDelete("{uid}")]
        [Log("管理员删除了一个用户")]
        [PermissionMeta("删除用户", "管理员", mount: false)]
        public async Task<ActionResult<OkMsg>> DeleteUser(int uid)
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

            return Ok(new OkMsg
            {
                Msg = "删除用户成功"
            });
        }

        [HttpPut("disable/{uid}")]
        [PermissionMeta("禁用用户", "管理员", mount: false)]
        public async Task<ActionResult<OkMsg>> DisableUser(int uid)
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

            return Ok(new OkMsg
            {
                Msg = "禁用用户成功"
            });
        }

        [HttpPut("active/{uid}")]
        [PermissionMeta("激活用户", "管理员", mount: false)]
        public async Task<ActionResult<OkMsg>> ActiveUser(int uid)
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

            return Ok(new OkMsg
            {
                Msg = "激活用户成功"
            });
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

        [HttpGet("group/{gid}", Name = "GetGroup")]
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

        /// <summary>
        /// 新建权限组
        /// </summary>
        /// <param name="linGroupAddResource"></param>
        /// <returns></returns>
        /// <response code="201">返回新建带权限的权限组</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost("group")]
        [Log("管理员新建了一个权限组")]
        [PermissionMeta("新建权限组", "管理员", mount: false)]
        public async Task<ActionResult<LinGroupWithAuthsResource>> AddGroup(LinGroupAddResource linGroupAddResource)
        {
            var group = MyMapper.Map<LinGroupAddResource, LinGroup>(linGroupAddResource);

            var dispatchedMetas = PermissionMetaHandler.GetAllDispatchedMetas();

            var permissionMetas = dispatchedMetas.Where(meta => linGroupAddResource.Auths.Contains(meta.Auth));

            var linAuths = MyMapper.Map<IEnumerable<PermissionMeta>, IEnumerable<LinAuth>>(permissionMetas);

            group.LinAuths = linAuths.ToList();

            _adminRepository.Add(group);

            if (!await UnitOfWork.SaveAsync())
            {
                throw new Exception("Save Failed!");
            }

            var resource = MyMapper.Map<LinGroup, LinGroupWithAuthsResource>(group);

            return CreatedAtRoute("GetGroup", new { id = resource.Id }, resource);
        }

        [HttpPut("group/{gid}")]
        [PermissionMeta("更新一个权限组", "管理员", mount: false)]
        public async Task<ActionResult<LinGroupWithAuthsResource>> UpdateGroup(int gid, LinGroupUpdateResource linGroupUpdateResource)
        {
            var group = await _adminRepository.GetGroupWithAuthAndUserAsync(gid);

            if (group == null)
            {
                throw new NotFoundException
                {
                    ErrorCode = ResultCode.GroupNotFoundErrorCode
                };
            }

            MyMapper.Map(linGroupUpdateResource, group);

            _adminRepository.Update(group);

            if (!await UnitOfWork.SaveAsync())
            {
                throw new Exception("Save Failed!");
            }

            var resource = MyMapper.Map<LinGroup, LinGroupWithAuthsResource>(group);

            return Ok(resource);
        }

        [HttpDelete("group/{pid}")]
        [Log("管理员删除一个权限组")]
        [PermissionMeta("删除一个权限组", "管理员", mount: false)]
        public async Task<ActionResult<OkMsg>> DeleteGroup(int pid)
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

            return Ok(new OkMsg
            {
                Msg = "删除分组成功"
            });
        }

        [HttpPost("dispatch/patch")]
        [PermissionMeta("分配多个权限", "管理员", mount: false)]
        public async Task<ActionResult<OkMsg>> DispatchAuths(
            LinGroupDispatchAuthsResource linGroupDispatchAuthsResource)
        {
            var linAuths = new List<LinAuth>();
            var dispatchedMetas = PermissionMetaHandler.GetAllDispatchedMetas();

            var auths = (await _adminRepository.GetAllAuthsAsync()).ToList();

            foreach (var auth in linGroupDispatchAuthsResource.Auths)
            {
               var existedLinAuth =  auths.FirstOrDefault(a => a.GroupId == linGroupDispatchAuthsResource.GroupId && a.Auth == auth);
               if (existedLinAuth == null)
               {
                   var meta = dispatchedMetas.Where(m => m.Auth == auth).ToList();
                   if (meta.Any())
                   {
                       var addLinAuths = MyMapper.Map<IEnumerable<PermissionMeta>, IEnumerable<LinAuth>>(meta);
                       linAuths.AddRange(addLinAuths);
                   }
               }
            }

            _adminRepository.AddRange(linAuths);

            await UnitOfWork.SaveAsync();

            return Ok(new OkMsg
            {
                Msg = "添加权限成功"
            });
        }

        [HttpPost("remove")]
        [PermissionMeta("删除多个权限", "管理员", mount: false)]
        public async Task<ActionResult<OkMsg>> RemoveAuths(
            LinGroupDispatchAuthsResource linGroupDispatchAuthsResource)
        {
            var auths = await _adminRepository.GetAllAuthsAsync();

            var linAuths = auths.Where(auth =>
                auth.GroupId == linGroupDispatchAuthsResource.GroupId &&
                linGroupDispatchAuthsResource.Auths.Contains(auth.Auth)).ToList();

            _adminRepository.DeleteRange(linAuths);

            await UnitOfWork.SaveAsync();

            return Ok(new OkMsg
            {
                Msg = "删除权限成功"
            });
        }
    }
}
