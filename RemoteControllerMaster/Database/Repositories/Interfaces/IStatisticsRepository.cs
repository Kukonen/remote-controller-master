using RemoteControllerMaster.Database.Models;

namespace RemoteControllerMaster.Database.Repositories.Interfaces
{
    public interface IStatisticsRepository
    {
        Task InsertAsync(Statistic statistic);
    }
}
