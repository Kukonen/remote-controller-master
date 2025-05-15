using RemoteControllerMaster.Database.Models;
using RemoteControllerMaster.Dtos.Machine;

namespace RemoteControllerMaster.Mappers
{
    public static class MachineMapper
    {
        public static MachineDto MapToDto(this Machine machine)
        {
            return new MachineDto()
            {
                MachineId = machine.MachineId,
                MachineName = machine.MachineName,
            };
        }
    }
}
