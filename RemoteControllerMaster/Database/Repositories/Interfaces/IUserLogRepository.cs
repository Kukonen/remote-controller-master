using RemoteControllerMaster.Database.Models;

namespace RemoteControllerMaster.Database.Repositories.Interfaces
{
    public interface IUserLogRepository
    {
        Task AddUserLogAsync(UserLog userLog);
    }
}
