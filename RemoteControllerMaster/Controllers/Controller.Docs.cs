using Microsoft.AspNetCore.Mvc;
using RemoteControllerMaster.Attributes.Authorize;
using RemoteControllerMaster.Database.Models;


namespace RemoteControllerMaster.Controllers
{
    public partial class Controller
    {
        [HttpGet]
        public string Get()
        {
            return "docs";
        }

        [HttpPost]
        [AuthorizePermissions(Enums.Permission.User_Read)]
        public async Task<User> PostUserUser(Permission permission)
        {
            await UserLogAsync(
                null,
                permission,
                new User()
                {
                    Login = "login",
                    PasswordHash = "password",
                    UserId = Guid.NewGuid(),
                }
            );

            return new User()
            {
                Login = "login",
                PasswordHash = "password",
                UserId = Guid.NewGuid(),
            };
        }
    }
}
