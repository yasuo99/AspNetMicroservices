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
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;
        public BasketService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<BasketModel> GetBasket(string username)
        {
            var response = await _httpClient.GetAsync($"/api/v1/Basket/GetBasket/{username}");

            BasketModel basket = await response.ReadContentAs<BasketModel>();

            return basket;
        }
    }
}
