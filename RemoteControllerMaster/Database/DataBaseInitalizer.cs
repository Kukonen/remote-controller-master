using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace RemoteControllerMaster.Database
{
    public static class DataBaseInitalizer
    {
        public static void Init(WebApplication app)
        {
            var configuration = app.Services.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var context = app.Services.CreateScope()
                .ServiceProvider.GetRequiredService<ApplicationDbContext>();

            context.Database.Migrate();
            //context.Database.EnsureCreated();
        }
    }
}
