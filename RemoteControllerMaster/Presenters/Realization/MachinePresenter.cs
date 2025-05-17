using Microsoft.AspNetCore.Mvc;
using RemoteControllerMaster.Database.Repositories.Interfaces;
using RemoteControllerMaster.Dtos.Machine;
using RemoteControllerMaster.Mappers;
using RemoteControllerMaster.Models.App;
using RemoteControllerMaster.Presenters.Interfaces;

namespace RemoteControllerMaster.Presenters.Realization
{
    public class MachinePresenter : IMachinePresenter
    {
        private readonly IMachineRepository _machineRepository;
        private readonly IUser2MachineRepository _user2MachineRepository;

        public MachinePresenter(
            IMachineRepository machineRepository, 
            IUser2MachineRepository user2MachineRepository
        )
        {
            _machineRepository = machineRepository;
            _user2MachineRepository = user2MachineRepository;
        }

        public async Task<IActionResult> GetAllMachines()
        {
            var machines = await _machineRepository.GetAll();

            return new OkObjectResult(machines.Select(m => m.MapToDto()));
        }

        public async Task<IActionResult> CreateMachine(MachineDto machineDto)
        {
            await _machineRepository.AddMachine(machineDto.MapToDo());

            return new CreatedResult();
        }
        public async Task<IActionResult> GetUserMachines(UserContext userContext)
        {
            var machines = await _machineRepository.GetAll();
            var userMahcines = await _user2MachineRepository.GetByUsersIdsAsync(new[] { userContext.UserId });

            return new OkObjectResult(machines.Where(machine => userMahcines.Any(u2m => u2m.MachineId == machine.MachineId)));
        }
        public async Task<IActionResult> UpdateMachine(MachineDto machineDto)
        {
            await _machineRepository.UpdateMachine(machineDto.MapToDo());

            return new OkResult();
        }
        public async Task<IActionResult> DeleteMachine(Guid machineId)
        {
            await _machineRepository.RemoveMachine(machineId);

            return new OkResult();
        }
    }
}
