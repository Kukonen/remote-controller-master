using Microsoft.AspNetCore.Mvc;
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
