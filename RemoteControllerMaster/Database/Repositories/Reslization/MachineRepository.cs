using Microsoft.EntityFrameworkCore;
using RemoteControllerMaster.Database.Models;
using RemoteControllerMaster.Database.Repositories.Interfaces;

namespace RemoteControllerMaster.Database.Repositories.Reslization
{
    public class MachineRepository : IMachineRepository
    {
        private readonly ApplicationDbContext _context;

        public MachineRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Machine[]> GetAll()
        {
            return await _context.Machines.ToArrayAsync();
        }

        public async Task<Machine[]> GetByMachinesIds(Guid[] machineIds)
        {
            return await _context.Machines.Where(m => machineIds.Contains(m.MachineId)).ToArrayAsync();
        }

        public async Task AddMachine(Machine machine)
        {
            _context.Machines.Add(machine);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateMachine(Machine machine)
        {
            _context.Machines.Update(machine);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveMachine(Guid machineId)
        {
            var machine = await _context.Machines.FirstAsync(m => m.MachineId == machineId);
            if (machine != null)
            {
                _context.Machines.Remove(machine);
                await _context.SaveChangesAsync();
            }
        }
    }
}
