using Microsoft.EntityFrameworkCore;
using RemoteControllerMaster.Database.Models;
using RemoteControllerMaster.Database.Repositories.Interfaces;

namespace RemoteControllerMaster.Database.Repositories.Reslization
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToArrayAsync();
        }

        public async Task<User?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task InsertAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }
    }
}
