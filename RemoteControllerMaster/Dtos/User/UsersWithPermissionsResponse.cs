using RemoteControllerMaster.Dtos.Machine;

namespace RemoteControllerMaster.Dtos.User
{
    public class UsersWithPermissionsResponse
    {
        public UserDto User { get; set; }
        public Enums.Permission[] Permissions { get; set; }
        public MachineDto[] Machines { get; set; }
    }
}
