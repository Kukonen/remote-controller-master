using Microsoft.AspNetCore.Mvc;
using RemoteControllerMaster.Attributes.Authorize;
using RemoteControllerMaster.Database.Models;
using RemoteControllerMaster.Database.Repositories.Interfaces;
using RemoteControllerMaster.Dtos.Auth;
using RemoteControllerMaster.Enums;
using RemoteControllerMaster.Helpers.Authorize;


namespace RemoteControllerMaster.Controllers
{
    public partial class Controller
    {
        [AuthorizePermissions(new Enums.Permission[] { Enums.Permission.User_Write })]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            return await _userPresenter.Register(request);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            return await _userPresenter.Login(request);
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            return await _userPresenter.Refresh(refreshRequest);
        }
    }

}
