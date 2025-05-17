using RemoteControllerMaster.Database.Models;

namespace RemoteControllerMaster.Database.Repositories.Interfaces
{
    public interface IUser2MachineRepository
    {
        Task<IEnumerable<User2Machine>> GetByUsersIdsAsync(IEnumerable<Guid> usersIds);
        Task AddMachinesByUserIdAsync(Guid userId, IEnumerable<Guid> machinesIds);
        Task RemoveMachinesAsync(Guid userId);
    }
}
