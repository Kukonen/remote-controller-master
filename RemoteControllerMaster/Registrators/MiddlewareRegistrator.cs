﻿using RemoteControllerMaster.Middlewares;

namespace RemoteControllerMaster.Registrators
{
    public static class MiddlewareRegistrator
    {
        public static void RegisterMiddlewares(this WebApplication app)
        {
            app.UseMiddleware<UserContextMiddleware>();
        }
    }
}
