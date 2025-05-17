using RemoteControllerMaster.Database.Repositories.Interfaces;
using RemoteControllerMaster.Models.App;

namespace RemoteControllerMaster.Middlewares
{
    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;

        public UserContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUser2PermissionRepository permissionRepo)
        {
            var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
            {
                var permissions = await permissionRepo.GetPermissionsAsync(userId);

                var userContext = new UserContext
                {
                    UserId = userId,
                    Permissions = permissions.Select(u2p => u2p.Permission).ToArray()
                };

                context.Items["UserContext"] = userContext;
            }

            await _next(context);
        }
    }
}
