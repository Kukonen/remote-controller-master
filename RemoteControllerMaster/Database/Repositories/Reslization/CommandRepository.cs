using Microsoft.EntityFrameworkCore;
using RemoteControllerMaster.Database.Models;
using RemoteControllerMaster.Database.Repositories.Interfaces;


namespace RemoteControllerMaster.Database.Repositories.Reslization
{
    public class CommandRepository : ICommandRepository
    {
        private readonly ApplicationDbContext _context;

        public CommandRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Command[]> GetAll()
        {
            return await _context.Commands.ToArrayAsync();
        }

        public async Task<Command[]> GetByCommandsIds(Guid[] commandIds)
        {
            return await _context.Commands.Where(m => commandIds.Contains(m.CommandId)).ToArrayAsync();
        }

        public async Task AddCommand(Command command)
        {
            _context.Commands.Add(command);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateCommand(Command command)
        {
            _context.Commands.Update(command);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveCommand(Guid commandId)
        {
            var command = await _context.Commands.FirstAsync(m => m.CommandId == commandId);
            if (command != null)
            {
                _context.Commands.Remove(command);
                await _context.SaveChangesAsync();
            }
        }
    }
}

