using RemoteControllerMaster.Database.Models;

namespace RemoteControllerMaster.Database.Repositories.Interfaces
{
    public interface IMachineRepository
    {
        Task<Machine[]> GetAll();
        Task<Machine[]> GetByMachinesIds(Guid[] machineIds);
    }
}
