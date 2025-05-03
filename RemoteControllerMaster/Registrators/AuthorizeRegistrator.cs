using RemoteControllerMaster.Enums;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace RemoteControllerMaster.Registrators
{
    public static class AuthorizeRegistrator
    {
        public static void RegisterAuthorize(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authorize:AuthorizeHash"])),
                        ValidateLifetime = true
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                foreach (var permission in Enum.GetValues<Permission>())
                {
                    options.AddPolicy(permission.ToString(), policy =>
                    {
                        policy.RequireClaim("permission", permission.ToString());
                    });
                }
            });
        }
    }
}
