using RemoteControllerMaster.Database.Models;
using RemoteControllerMaster.Database.Repositories.Interfaces;

namespace RemoteControllerMaster.Database.Repositories.Reslization
{
    public class UserLogRepository : IUserLogRepository
    {
        private readonly ApplicationDbContext _context;

        public UserLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddUserLogAsync(UserLog userLog)
        {
            _context.UserLogs.Add(userLog);
            await _context.SaveChangesAsync();
        }
    }
}
