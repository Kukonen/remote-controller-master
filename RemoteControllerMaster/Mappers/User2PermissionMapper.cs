using RemoteControllerMaster.Database.Models;
using RemoteControllerMaster.Dtos.User;

namespace RemoteControllerMaster.Mappers
{
    public static class User2PermissionMapper
    {
        public static User2PermissionDto MapToDto(this User2Permission user2Permission)
        {
            return new User2PermissionDto() 
            {
                UserId = user2Permission.UserId,
                Permission = user2Permission.Permission
            };
        }
    }
}
