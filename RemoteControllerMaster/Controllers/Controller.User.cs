using Microsoft.AspNetCore.Mvc;
using RemoteControllerMaster.Dtos.User;

namespace RemoteControllerMaster.Controllers
{
    public partial class Controller
    {
        [HttpGet("GetUsersWithPermissionsAndMachines")]
        public async Task<IActionResult> GetUsersWithPermissionsAndMachines()
        {
            return await _userPresenter.GetUsersWithPermissionsAndMachines();
        }

        [HttpPost("UpdateUserPermission")]
        public async Task<IActionResult> UpdateUserPermission([FromBody] User2PermissionUpdateRequestDto request)
        {
            return await _userPresenter.UpdateUserPermission(request);
        }

        [HttpPost("UpdateUserMachines")]
        public async Task<IActionResult> UpdateUserMachines(User2MachineUpdateRequestDto request)
        {
            return await _userPresenter.UpdateUserMachines(request);
        }
    }
}
