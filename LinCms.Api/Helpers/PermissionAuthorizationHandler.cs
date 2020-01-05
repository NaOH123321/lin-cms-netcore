using System.Threading.Tasks;
using LinCms.Core;
using LinCms.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace LinCms.Api.Helpers
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionMetaRequirement>
    {
        private readonly ICurrentUser _currentUser;

        public PermissionAuthorizationHandler(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionMetaRequirement requirement)
        {
            if (_currentUser.IsAdmin || requirement.Role == UserRole.Every)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            if (!_currentUser.CheckRole(requirement.Role))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            if (_currentUser.CheckPermission(requirement.Name))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
