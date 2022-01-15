using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ordering.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Extensions
{
    public static class HostExtension
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder, int? retry = 0) where TContext: DbContext
        {
            var retryForAvailablity = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var logger = services.GetRequiredService<ILogger<TContext>>();

                var context = services.GetRequiredService<TContext>();

                try
                {
                    logger.LogInformation("Seed database with {dbContext} start", typeof(TContext));
                    InvokeSeeder(seeder, context, services);
                    logger.LogInformation("Seed database with {dbContext} success", typeof(TContext));
                }
                catch (SqlException ex)
                {
                    logger.LogError(ex, "An error occured while migrating database {dbContext}", typeof(TContext));
                    if(retryForAvailablity < 50)
                    {
                        retryForAvailablity++;
                        Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, seeder, retryForAvailablity);
                    }
                    throw;
                }
            }
            return host;
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services) where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
