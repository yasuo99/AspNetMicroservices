using Microsoft.EntityFrameworkCore;
using Ordering.Application.Constracts.Persistence;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(OrderDbContext orderDbContext) : base(orderDbContext)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByUsername(string username)
        {
            return await _orderDbContext.Orders
                .Where(order => order.Username.Equals(username))
                .ToListAsync();
        }
    }
}
