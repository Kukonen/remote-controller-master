using Microsoft.AspNetCore.Authorization;
using RemoteControllerMaster.Enums;

namespace RemoteControllerMaster.Attributes.Authorize
{
    public class AuthorizePermissionsAttribute : AuthorizeAttribute
    {
        public AuthorizePermissionsAttribute(params Permission[] permissions)
        {
            Policy = string.Join(",", permissions.Select(p => p.ToString()));
        }
    }
}
