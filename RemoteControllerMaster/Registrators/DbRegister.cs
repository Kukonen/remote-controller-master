using Microsoft.EntityFrameworkCore;
using RemoteControllerMaster.Database;
using RemoteControllerMaster.Database.Repositories.Interfaces;
using RemoteControllerMaster.Database.Repositories.Reslization;

namespace RemoteControllerMaster.Registrators
{
    public static class DbRegister
    {
        public static void RegisterDb(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
