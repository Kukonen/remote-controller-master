using RemoteControllerMaster.Database.Models;
using RemoteControllerMaster.Dtos.User;

namespace RemoteControllerMaster.Mappers
{
    public static class User2MachineMapper
    {
        public static User2MachineDto MapToDto(this User2Machine user2Machine)
        {
            return new User2MachineDto()
            {
                UserId = user2Machine.UserId,
                MachineId = user2Machine.MachineId
            };
        }
    }
}
