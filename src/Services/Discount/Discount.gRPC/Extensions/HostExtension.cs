using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Discount.gRPC.Extensions
{
    public static class HostExtension
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvailablity = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var configuration = services.GetRequiredService<IConfiguration>();

                try
                {
                    using var connection = new NpgsqlConnection(configuration.GetConnectionString("Postgre"));

                    connection.Open();

                    using var command = new NpgsqlCommand { Connection = connection };
                    logger.LogInformation("Migrating Postgre database");
                    command.CommandText = "DROP TABLE IF EXISTS Coupon";

                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY,
                                                                ProductName VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT,
                                                                Remain INT)";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount, Remain) VALUES('IPhone X', 'IPhone discount', 5, 10)";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount, Remain) VALUES('Nokia X5', 'Nokia discount', 5, 10)";
                    command.ExecuteNonQuery();

                    connection.Close();
                    logger.LogInformation("Migrated Postgre database");
                }
                catch (NpgsqlException ex)
                {
                    logger.LogInformation(ex, "Migrate Postgre database error");
                    if(retryForAvailablity < 50)
                    {
                        retryForAvailablity++;
                        Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, retryForAvailablity);
                    }
                }
                
            }
            return host;
        }
    }
}
