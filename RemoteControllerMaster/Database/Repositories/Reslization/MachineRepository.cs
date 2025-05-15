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
    }
}
