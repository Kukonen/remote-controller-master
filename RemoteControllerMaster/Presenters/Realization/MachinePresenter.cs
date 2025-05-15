using Microsoft.AspNetCore.Mvc;
using RemoteControllerMaster.Database.Repositories.Interfaces;
using RemoteControllerMaster.Mappers;
using RemoteControllerMaster.Presenters.Interfaces;

namespace RemoteControllerMaster.Presenters.Realization
{
    public class MachinePresenter : IMachinePresenter
    {
        private readonly IMachineRepository _machineRepository;

        public MachinePresenter(IMachineRepository machineRepository)
        {
            _machineRepository = machineRepository;
        }

        public async Task<IActionResult> GetAllMachines()
        {
            var machines = await _machineRepository.GetAll();

            return new OkObjectResult(machines.Select(m => m.MapToDto()));
        }
    }
}
