using RemoteControllerMaster.Database.Models;

namespace RemoteControllerMaster.Database.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByUserIdAsync(Guid userId);
        Task InsertAsync(User user);
    }
}
