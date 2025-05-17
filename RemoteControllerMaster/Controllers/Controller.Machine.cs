using Microsoft.AspNetCore.Mvc;
using RemoteControllerMaster.Attributes.Authorize;
using RemoteControllerMaster.Dtos.Machine;
using RemoteControllerMaster.Models.App;


namespace RemoteControllerMaster.Controllers
{
    public partial class Controller
    {
        [AuthorizePermissions(new Enums.Permission[] { Enums.Permission.Machine_Read })]
        [HttpGet("GetAllMachines")]
        public async Task<IActionResult> GetAllMachines()
        {
            return await _machinePresenter.GetAllMachines();
        }

        [AuthorizePermissions(new Enums.Permission[] { Enums.Permission.Machine_Write })]
        [HttpPost("AddMachine")]
        public async Task<IActionResult> CreateMachine(MachineDto machineDto)
        {
            return await _machinePresenter.CreateMachine(machineDto);
        }

        [HttpGet("GetUserMachines")]
        public async Task<IActionResult> GetUserMachines()
        {
            var userContext = HttpContext.Items["UserContext"] as UserContext;

            if (userContext == null)
            {
                return new BadRequestResult();
            }

            return await _machinePresenter.GetUserMachines(userContext);
        }

        [AuthorizePermissions(new Enums.Permission[] { Enums.Permission.Machine_Write })]
        [HttpPost("UpdateMachine")]
        public async Task<IActionResult> UpdateMachine(MachineDto machineDto)
        {
            return await _machinePresenter.UpdateMachine(machineDto);
        }

        [AuthorizePermissions(new Enums.Permission[] { Enums.Permission.Machine_Write })]
        [HttpPost("DeleteMachine")]
        public async Task<IActionResult> DeleteMachine(MachineDto machineDto)
        {
            return await _machinePresenter.UpdateMachine(machineDto);
        }
    }
}
