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
        }

        public static void RegisterRepositories(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserLogRepository, UserLogRepository>();
            builder.Services.AddScoped<IUser2PermissionRepository, User2PermissionRepository>();
            builder.Services.AddScoped<IAuthorizeTokenRepository, AuthorizeTokenRepository>();
            builder.Services.AddScoped<IMachineRepository, MachineRepository>();
            builder.Services.AddScoped<IUser2MachineRepository, User2MachineRepository>();
        }
    }
}
