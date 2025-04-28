using RemoteControllerMaster.BackgroundServices;


namespace RemoteControllerMaster.Registrators
{
    public static class BackgroundServicesRegister
    {
        public static void RegisterBackgroundServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddHostedService<PartitionBackgroundService>();
        }
    }
}
