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

            string frontendCors = "AllowFrontend";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: frontendCors,
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5173")
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials();
                    });
            });

            builder.Services.AddControllers();

            var app = builder.Build();

            DataBaseInitalizer.Init(app);

            app.UseCors(frontendCors);
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.RegisterMiddlewares();

            app.Run();
        }
    }
}
