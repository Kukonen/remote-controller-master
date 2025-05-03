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
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    await EnsureCurrentWeekPartitionAsync(context);
                }

                await Task.Delay(TimeSpan.FromMinutes(30), cancellationToken);
            }
        }

        private async Task EnsureCurrentWeekPartitionAsync(ApplicationDbContext context)
        {
            PartitionTableInfo[] partitionTableInfos = new[]
            {
                new PartitionTableInfo()
                {
                    Schema = "analytics",
                    SourceTable = "statistics",
                    Days = 7
                },
                new PartitionTableInfo()
                {
                    Schema = "analytics",
                    SourceTable = "user_log",
                    Days = 7
                }
            };

            foreach(var partitionTableInfo in partitionTableInfos)
            {
                 await context.Database.ExecuteSqlRawAsync(GenerateSqlString(partitionTableInfo));
            }
        }

        private string GenerateSqlString(PartitionTableInfo partitionTableInfo)
        {
            DateTime now = DateTime.UtcNow;
            DateTime startOfWeek = now.AddDays(-(int)now.DayOfWeek + (int)DayOfWeek.Monday).Date;
            DateTime endOfWeek = startOfWeek.AddDays(partitionTableInfo.Days);
            string weekName = $"{startOfWeek:yyyy}w{ISOWeek.GetWeekOfYear(startOfWeek)}";
            string tableName = $"{partitionTableInfo.SourceTable}_{weekName}";

            return $@"
                DO $$
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1 FROM information_schema.tables 
                        WHERE table_schema = '{partitionTableInfo.Schema}' AND table_name = '{tableName}'
                    ) THEN
                        EXECUTE '
                            CREATE TABLE {partitionTableInfo.Schema}.{tableName} 
                            PARTITION OF {partitionTableInfo.Schema}.{partitionTableInfo.SourceTable}
                            FOR VALUES FROM (''{startOfWeek:yyyy-MM-dd}'') TO (''{endOfWeek:yyyy-MM-dd}'');
                        ';
                    END IF;
                END
                $$;
            ";
        }

        private class PartitionTableInfo
        {
            public string Schema { get; set; }
            public string SourceTable { get; set; }
            public int Days { get; set; }
        }
    }
}
