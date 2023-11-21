using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIS.Errors;
using Talabat.APIS.Extensions;
using Talabat.APIS.Helpers;
using Talabat.APIS.MiddleWares;
using Talabat.core.Entities;
using Talabat.core.Entities.Identity;
using Talabat.core.Repositories;
using Talabat.core.Repositories.Identity;
using Talabat.repository;
using Talabat.repository.Data;
using Talabat.repository.Identity;

namespace Talabat.APIS
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }
            );
            builder.Services.AddDbContext<core.Repositories.Identity.AppIdentityDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            builder.Services.AddSingleton<IConnectionMultiplexer>(Options =>
            {
                var Connection = builder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(Connection);
            });
            builder.Services.AddApplicationServices();
            builder.Services.AddIdentityServices(builder.Configuration);

            builder.Services.AddCors(Options =>
            {
                Options.AddPolicy("MyPolicy", Options =>
                {
                    Options.AllowAnyHeader();
                    Options.AllowAnyMethod();
                    Options.WithOrigins(builder.Configuration["FrontBaseUrl"]);
                });
            }
            );





            var app = builder.Build();


            #region Update-Database
            //StoreContext dbcontext = new StoreContext();
            //await dbcontext.Database.MigrateAsync();
            using var Scope = app.Services.CreateScope();
            //Group of services lifetime scope
            var services = Scope.ServiceProvider;
            //Services itself

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
            var dbContext = services.GetRequiredService<StoreContext>();
            //Ask CLR for creating object from DbContext Explicitly
            await dbContext.Database.MigrateAsync();
                var UserManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUserAsync(UserManager);
                var IdentityDbContext = services.GetRequiredService<AppIdentityDbContext>();

                #region Data Seeding
                await StoreContextSeed.SeedAsync(dbContext);
            #endregion

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error Occured During Applying Migration");
            }
            #endregion



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMiddleware<ExceptionMiddleware>();
                app.UseSwaggarMiddleware();
                
            }
            app.UseStatusCodePagesWithRedirects("/error/{0}");

            app.UseHttpsRedirection();
            app.UseCors("MyPolicy");
            app.UseAuthorization();
            app.UseAuthorization();
            app.UseStaticFiles();



            app.MapControllers();

            app.Run();
        }
    }
}