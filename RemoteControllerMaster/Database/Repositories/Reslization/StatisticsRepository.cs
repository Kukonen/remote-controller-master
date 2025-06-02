using RemoteControllerMaster.Database.Models;
using RemoteControllerMaster.Database.Repositories.Interfaces;
using System.Reflection.PortableExecutable;

namespace RemoteControllerMaster.Database.Repositories.Reslization
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly ApplicationDbContext _context;

        public StatisticsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task InsertAsync(Statistic statistic)
        {
            _context.Statistics.Add(statistic);

            await _context.SaveChangesAsync();
        }
    }
}
