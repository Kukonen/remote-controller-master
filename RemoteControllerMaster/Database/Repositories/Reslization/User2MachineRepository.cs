using Microsoft.EntityFrameworkCore;
using RemoteControllerMaster.Database.Models;
using RemoteControllerMaster.Database.Repositories.Interfaces;


namespace RemoteControllerMaster.Database.Repositories.Reslization
{
    public class User2MachineRepository : IUser2MachineRepository
    {
        private readonly ApplicationDbContext _context;

        public User2MachineRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User2Machine>> GetByUsersIdsAsync(IEnumerable<Guid> usersIds)
        {
            return await _context.Users2Machines.Where(u2p => usersIds.Contains(u2p.UserId)).ToArrayAsync();
        }

        public async Task AddMachinesByUserIdAsync(Guid userId, IEnumerable<Guid> machinesIds)
        {
            foreach (var machineId in machinesIds)
            {
                _context.Users2Machines.Add(new User2Machine
                {
                    UserId = userId,
                    MachineId = machineId
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveMachinesAsync(Guid userId)
        {
            var userMachines = _context.Users2Machines.Where(um => um.UserId == userId);
            _context.Users2Machines.RemoveRange(userMachines);
            await _context.SaveChangesAsync();
        }
    }
}
