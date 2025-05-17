using Microsoft.IdentityModel.Tokens;
using System.Text;
using RemoteControllerMaster.Helpers.Authorize;
using Microsoft.AspNetCore.Authorization;


namespace RemoteControllerMaster.Registrators
{
    public static class AuthorizeRegistrator
    {
        public static void RegisterAuthorize(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();

            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secret-jwt-sring-must-be-min-32-charecters-!!")),
                        ValidateLifetime = true,
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("PermissionPolicy", policy =>
                {
                    policy.Requirements.Add(new PermissionRequirement());
                });
            });
        }
    }
}
