using Microsoft.EntityFrameworkCore;
using RemoteControllerMaster.Database;
using System.Globalization;


namespace RemoteControllerMaster.BackgroundServices
{
    public class PartitionBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public PartitionBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    await EnsureCurrentWeekPartitionAsync(dbContext);
                }

                await Task.Delay(TimeSpan.FromMinutes(30), cancellationToken);
            }
        }

        private async Task EnsureCurrentWeekPartitionAsync(ApplicationDbContext dbContext)
        {
            DateTime now = DateTime.UtcNow;
            DateTime startOfWeek = now.AddDays(-(int)now.DayOfWeek + (int)DayOfWeek.Monday).Date;
            DateTime endOfWeek = startOfWeek.AddDays(7);
            string weekName = $"{startOfWeek:yyyy}w{ISOWeek.GetWeekOfYear(startOfWeek)}";
            string tableName = $"statistics_{weekName}";

            var createPartitionSql = $@"
                DO $$
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1 FROM information_schema.tables 
                        WHERE table_schema = 'analytics' AND table_name = '{tableName}'
                    ) THEN
                        EXECUTE '
                            CREATE TABLE analytics.{tableName} 
                            PARTITION OF analytics.statistics
                            FOR VALUES FROM (''{startOfWeek:yyyy-MM-dd}'') TO (''{endOfWeek:yyyy-MM-dd}'');
                        ';
                    END IF;
                END
                $$;
            ";

            await dbContext.Database.ExecuteSqlRawAsync(createPartitionSql);
        }
    }
}
