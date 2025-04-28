using Microsoft.EntityFrameworkCore;
using RemoteControllerMaster.Database;


namespace RemoteControllerMaster.Registrators
{
    public static class DbRegister
    {
        public static void RegisterDb(this WebApplicationBuilder builder)
        {
            var a = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
