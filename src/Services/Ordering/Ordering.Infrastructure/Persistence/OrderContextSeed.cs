using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderDbContext orderDbContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderDbContext.Orders.Any())
            {
                orderDbContext.Orders.AddRange(GetPreconfiguredOrders());
                await orderDbContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {dbContext}", typeof(OrderDbContext));
            }
        }
        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order()
                {
                    Username = "swn",
                    FirstName = "Thanh",
                    LastName = "Luyen",
                    Email = "yasuo120999@gmail.com",
                    Phone = "0983961054",
                    PaymentMethod = Domain.Constants.PaymentMethod.COD,
                    Address = "Bu Xia, Dak O, Bu Gia Map, Binh Phuoc",
                    City = "HCM",
                    ZipCode = "100000"
                }
            };
        }
    }
}
