using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LinCms.Api.Exceptions;
using LinCms.Core;
using LinCms.Core.RepositoryInterfaces;
using LinCms.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace LinCms.Api.Helpers
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<NameAuthorizationRequirement>
    {
        private readonly ILinUserRepository _linUserRepository;

        public PermissionAuthorizationHandler(ILinUserRepository linUserRepository)
        {
            _linUserRepository = linUserRepository;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, NameAuthorizationRequirement requirement)
        {
            var userId = context.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier).Value;
            if (!int.TryParse(userId, out var uid))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            if (context.User.IsInRole(UserAdmin.Admin.ToString()))
            {
                context.Succeed(requirement);
            }

            if (_linUserRepository.CheckPermission(uid, requirement.RequiredName))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
