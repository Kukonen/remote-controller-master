using RemoteControllerMaster.Database.Repositories.Interfaces;
using RemoteControllerMaster.Database.Repositories.Reslization;

namespace RemoteControllerMaster.Registrators
{
    public static class Registrator
    {
        public static void RegisterScope(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserRepository, UserRepository>();
        }

        public static void RegisterConfiguration(this WebApplicationBuilder builder)
        {
            builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)

                .AddJsonFile("appsettings.database.json", optional: true, reloadOnChange: true)

                .AddEnvironmentVariables();
        }
    }
}
