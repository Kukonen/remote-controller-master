using RemoteControllerMaster.Database.Models;


namespace RemoteControllerMaster.Database.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User[]> GetAllAsync();
        Task<User?> GetByLoginAsync(string login);
        Task<User?> GetByUserIdAsync(Guid userId);
        Task<bool> ExistsByLoginAsync(string login);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task RemoveAsync(User user);
        Task RemoveAsync(Guid userId);
    }
}
