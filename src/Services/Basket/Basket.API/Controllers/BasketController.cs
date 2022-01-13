using Basket.API.Entities;
using Basket.API.gRPC;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController: ControllerBase
    {
        private readonly IBasketRepository _repo;
        private readonly DiscountGrpcService _discountGrpcService;
        public BasketController(IBasketRepository repo, DiscountGrpcService discountGrpcService)
        {
            _repo = repo;
            _discountGrpcService = discountGrpcService;
        }
        [Route("[action]/{username}", Name = "GetBasket")]
        [HttpGet]
        [ProducesResponseType(typeof(ShoppingCart), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetBasket(string username)
        {
            var result = await _repo.GetBasket(username);
            return Ok(result ?? new ShoppingCart(username));
        }
        [Route("[action]/{username}", Name = "UpdateBasket")]
        [HttpPut]
        [ProducesResponseType(typeof(ShoppingCart), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateBasket([FromBody] ShoppingCart shoppingCart)
        {
            // TODO: Communicate with Discount.gRPC 
            // and calculate lastest prices of product into shopping cart
            // consuming discount
            foreach (var item in shoppingCart.Items)
            {
                var coupons = await _discountGrpcService.GetDiscountsByProductNameAsync(item.ProductName);
                var coupon = coupons.Coupons.OrderByDescending(o => o.Amount).FirstOrDefault(c => c.Remain > 0);
                item.Price = item.Price - (item.Price * coupon.Amount)/100; 
            }
            return Ok(await _repo.UpdateBasket(shoppingCart));
        }

        [Route("[action]/{username}", Name = "DeleteBasket")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string username)
        {
            await _repo.DeleteBasket(username);
            return Ok();
        }

      
    }
}
