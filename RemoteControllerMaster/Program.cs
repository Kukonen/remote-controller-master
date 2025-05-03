using Microsoft.EntityFrameworkCore;
using RemoteControllerMaster.Database;
using RemoteControllerMaster.Registrators;


namespace RemoteControllerMaster
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateSlimBuilder(args);

            builder.RegisterConfiguration();
            builder.RegisterDb();
            builder.RegisterRepositories();
            builder.RegisterScope();
            builder.RegisterBackgroundServices();
            builder.RegisterAuthorize();

            builder.Services.AddControllers();

            var app = builder.Build();

            DataBaseInitalizer.Init(app);

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.RegisterMiddlewares();

            app.Run();
        }
    }
}
