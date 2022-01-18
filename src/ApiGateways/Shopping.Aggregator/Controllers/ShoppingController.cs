using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ShoppingController: ControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;
        public ShoppingController(ICatalogService catalogService, IBasketService basketService, IOrderService orderService)
        {
            _catalogService = catalogService;
            _basketService = basketService;
            _orderService = orderService;
        }
        [HttpGet("{username}", Name = "GetShopping")]
        [ProducesResponseType(typeof(ShoppingModel),(int) HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetShopping(string username)
        {
            // TODO: 
            // Get basket with username
            var basket = await _basketService.GetBasket(username);
            // Iterate basket items and consume products with basket item productId member
            foreach (var item in basket.Items)
            {
                var product = await _catalogService.GetCatalog(item.ProductId);

                // Map product related members into basketitem dto with extend columns
                item.ProductName = product.Name;
                item.Category = product.Category;
                item.Summary = product.Summary;
                item.Description = product.Description;
                item.ImageFile = product.ImageFile;
            }
            // Consume ordering microservice to retrive order list
            var orders = await _orderService.GetOrdersByUsername(username);
            // Return root ShoppingModel dto class with including all response

            var shoppingModel = new ShoppingModel
            {
                Username = username,
                BasketWithProducts = basket,
                Orders = orders
            };
            return Ok(shoppingModel);
        }
    }
}
