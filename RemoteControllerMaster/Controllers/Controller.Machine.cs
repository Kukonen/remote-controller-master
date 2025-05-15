using Microsoft.AspNetCore.Mvc;


namespace RemoteControllerMaster.Controllers
{
    public partial class Controller
    {
        [HttpGet("GetAllMachines")]
        public async Task<IActionResult> GetAllMachines()
        {
            return await _machinePresenter.GetAllMachines();
        }
    }
}
