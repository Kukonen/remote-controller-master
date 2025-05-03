using RemoteControllerMaster.Database.Models;


namespace RemoteControllerMaster.Database.Repositories.Interfaces
{
    public interface IUser2PermissionRepository
    {
        Task<User2Permission[]> GetPermissionsAsync(Guid userId);
        Task AddPermissionsAsync(Guid userId, IEnumerable<Enums.Permission> permissions);
        Task RemovePermissionsAsync(Guid userId);
    }
}
