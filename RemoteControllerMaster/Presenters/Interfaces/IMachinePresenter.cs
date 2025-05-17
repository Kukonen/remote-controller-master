using Microsoft.AspNetCore.Mvc;
using RemoteControllerMaster.Dtos.Machine;
using RemoteControllerMaster.Models.App;

namespace RemoteControllerMaster.Presenters.Interfaces
{
    public interface IMachinePresenter
    {
        Task<IActionResult> GetAllMachines();
        Task<IActionResult> CreateMachine(MachineDto machineDto);
        Task<IActionResult> GetUserMachines(UserContext userContext);
        Task<IActionResult> UpdateMachine(MachineDto machineDto);
        Task<IActionResult> DeleteMachine(Guid machineId);
    }
}
