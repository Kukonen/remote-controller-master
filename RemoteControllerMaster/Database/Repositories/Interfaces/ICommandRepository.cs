using Microsoft.EntityFrameworkCore;
using RemoteControllerMaster.Database.Models;

namespace RemoteControllerMaster.Database.Repositories.Interfaces
{
    public interface ICommandRepository
    {
        Task<Command[]> GetAll();

        Task<Command[]> GetByCommandsIds(Guid[] commandIds);

        Task AddCommand(Command command);

        Task UpdateCommand(Command command);
        Task RemoveCommand(Guid commandId);
    }
}
