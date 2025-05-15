using Microsoft.AspNetCore.Mvc;

namespace RemoteControllerMaster.Presenters.Interfaces
{
    public interface IMachinePresenter
    {
        Task<IActionResult> GetAllMachines();
    }
}
