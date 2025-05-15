using Microsoft.AspNetCore.Mvc;
using RemoteControllerMaster.Dtos.Auth;
using RemoteControllerMaster.Dtos.User;

namespace RemoteControllerMaster.Presenters.Interfaces
{
    public interface IUserPresenter
    {
        Task<IActionResult> Register(RegisterRequest request);
        Task<IActionResult> Login(LoginRequestDto loginRequest);
        Task<IActionResult> Refresh(RefreshRequest refreshRequest);
        Task<IActionResult> GetUsersWithPermissionsAndMachines();
        Task<IActionResult> UpdateUserPermission(User2PermissionUpdateRequestDto request);
        Task<IActionResult> UpdateUserMachines(User2MachineUpdateRequestDto request);
    }
}
