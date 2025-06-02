using Microsoft.AspNetCore.Mvc;
using RemoteControllerMaster.Dtos.Commands;
using RemoteControllerMaster.Models.App;

namespace RemoteControllerMaster.Presenters.Interfaces
{
    public interface ICommandPresenter
    {
        Task<IActionResult> GetAllCommands();
        Task<IActionResult> CreateCommand(CommandDto machineDto);
        Task<IActionResult> UpdateCommand(CommandDto machineDto);
        Task<IActionResult> DeleteCommand(Guid machineId);
        Task<IActionResult> ExecuteCommand(ExecuteCommandDto executeCommandDto);
    }
}
