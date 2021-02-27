using System;
using FingergunsApi.App.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FingergunsApi.App.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<T>(this IHost host) where T : ApplicationDbContext
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var db = services.GetRequiredService<T>();
                db.Database.Migrate();
                
                var env = services.GetRequiredService<IWebHostEnvironment>();
                if (env.IsDevelopment() || env.IsLocal())
                {
                    DbInitializer.Initialize(db);
                }
            }
            catch (Exception e)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(e, "An error occurred while migrating the database");
            }

            return host;
        }
    }
}