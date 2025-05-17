using Microsoft.AspNetCore.Authorization;
using RemoteControllerMaster.Attributes.Authorize;
using System.Security.Claims;
using System;
using RemoteControllerMaster.Database.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;


namespace RemoteControllerMaster.Helpers.Authorize
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IUser2PermissionRepository _user2PermissionRepository;

        public PermissionHandler(IUser2PermissionRepository user2PermissionRepository)
        {
            _user2PermissionRepository = user2PermissionRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var httpContext = (context.Resource as DefaultHttpContext)
                ?? (context.Resource as HttpContext);

            if (httpContext == null)
            {
                return;
            }

            var userIdClaim = context.User.FindFirst(JwtRegisteredClaimNames.Sub)
                              ?? context.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return;
            }

            var requiredPermissions = AuthorizePermissionsAttribute.GetPermissionsFromEndpoint(httpContext);

            var userPermissions =
                (await _user2PermissionRepository.GetPermissionsAsync(userId))
                .Select(u2p => u2p.Permission);

            if (requiredPermissions.All(rp => userPermissions.Contains(rp)))
            {
                context.Succeed(requirement);
            } else
            {
                context.Fail();
            }
        }
    }
}
