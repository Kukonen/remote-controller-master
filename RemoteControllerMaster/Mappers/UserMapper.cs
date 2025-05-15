using RemoteControllerMaster.Database.Models;
using RemoteControllerMaster.Dtos.User;

namespace RemoteControllerMaster.Mappers
{
    public static class UserMapper
    {
        public static UserDto MapToDto(this User user)
        {
            return new UserDto()
            {
                UserId = user.UserId,
                Login = user.Login
            };
        }
    }
}
