using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<OrderResponseModel>> GetOrdersByUsername(string username)
        {
            var response = await _httpClient.GetAsync($"/api/v1/Order/{username}");

            return await response.ReadContentAs<List<OrderResponseModel>>();
        }
    }
}
