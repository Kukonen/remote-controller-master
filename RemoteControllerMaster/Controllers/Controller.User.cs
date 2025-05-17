using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RemoteControllerMaster.Attributes.Authorize;
using RemoteControllerMaster.Dtos.User;

namespace RemoteControllerMaster.Controllers
{
    public partial class Controller
    {
        [AuthorizePermissions(new Enums.Permission[] { Enums.Permission.User_Read })]
        [HttpGet("GetUsersWithPermissionsAndMachines")]
        public async Task<IActionResult> GetUsersWithPermissionsAndMachines()
        {
            return await _userPresenter.GetUsersWithPermissionsAndMachines();
        }

        [AuthorizePermissions(new Enums.Permission[] { Enums.Permission.User_Write })]
        [HttpPost("UpdateUserPermission")]
        public async Task<IActionResult> UpdateUserPermission([FromBody] User2PermissionUpdateRequestDto request)
        {
            return await _userPresenter.UpdateUserPermission(request);
        }

        [AuthorizePermissions(new Enums.Permission[] { Enums.Permission.User_Write })]
        [HttpPost("UpdateUserMachines")]
        public async Task<IActionResult> UpdateUserMachines(User2MachineUpdateRequestDto request)
        {
            return await _userPresenter.UpdateUserMachines(request);
        }

        [AuthorizePermissions(new Enums.Permission[] { Enums.Permission.User_Write })]
        [HttpDelete("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            return await _userPresenter.DeleteUser(userId);
        }

        [HttpGet("GetUserWithPermissionsAndMachines/{userId}")]
        public async Task<IActionResult> GetUserWithPermissionsAndMachines(Guid userId)
        {
            return await _userPresenter.GetUserWithPermissionsAndMachines(userId);
        }
    }
}
