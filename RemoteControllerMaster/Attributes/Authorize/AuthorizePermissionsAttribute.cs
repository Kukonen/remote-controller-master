using Microsoft.AspNetCore.Authorization;
using RemoteControllerMaster.Enums;


namespace RemoteControllerMaster.Attributes.Authorize
{
    public class AuthorizePermissionsAttribute : AuthorizeAttribute
    {
        public AuthorizePermissionsAttribute(params Permission[] permissions)
        {
            Policy = "PermissionPolicy";
            Permissions = permissions;
        }

        public Permission[] Permissions { get; }

        public static Permission[] GetPermissionsFromEndpoint(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var attr = endpoint?.Metadata.GetMetadata<AuthorizePermissionsAttribute>();
            return attr?.Permissions ?? Array.Empty<Permission>();
        }
    }
}
