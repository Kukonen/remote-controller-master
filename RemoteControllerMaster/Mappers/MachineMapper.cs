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
                IpAddress = machine.IpAddress,
            };
        }

        public static Machine MapToDo(this MachineDto machine)
        {
            return new Machine()
            {
                MachineId = machine.MachineId,
                MachineName = machine.MachineName,
                IpAddress = machine.IpAddress,
            };
        }
    }
}
