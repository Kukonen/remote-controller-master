﻿using RemoteControllerMaster.Database.Repositories.Interfaces;
using RemoteControllerMaster.Database.Repositories.Reslization;
using RemoteControllerMaster.Models.App;
using RemoteControllerMaster.Presenters.Interfaces;
using RemoteControllerMaster.Presenters.Realization;

namespace RemoteControllerMaster.Registrators
{
    public static class Registrator
    {
        public static void RegisterScope(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserPresenter, UserPresenter>();
            builder.Services.AddScoped<IMachinePresenter, MachinePresenter>();
            builder.Services.AddScoped<ICommandPresenter, CommandPresenter>();
        }

        public static void RegisterConfiguration(this WebApplicationBuilder builder)
        {
            builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.database.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.authorize.json", optional: true, reloadOnChange: true)

                .AddEnvironmentVariables();
        }
    }
}
