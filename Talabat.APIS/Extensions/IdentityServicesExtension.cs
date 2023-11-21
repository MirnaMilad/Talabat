using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Talabat.core.Entities.Identity;
using Talabat.core.Repositories.Identity;
using Talabat.core.Services;
using Talabat.services;

namespace Talabat.APIS.Extensions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services , IConfiguration configuration)
        {

            Services.AddScoped<ITokenService, TokenService>();

            Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>();

            Services.AddAuthentication(Options =>
            {
                Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme;
            }
            
            )
                .AddJwtBearer(
                Options =>
                {
                    Options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer= true,
                        ValidIssuer = configuration["JWT.ValidIssuer"],
                        ValidateAudience= true,
                        ValidAudience= configuration["JWT.ValidAudience"],
                        ValidateLifetime= true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:key"]))

                    };
                }
                );
            return Services;
        }
    }
}
