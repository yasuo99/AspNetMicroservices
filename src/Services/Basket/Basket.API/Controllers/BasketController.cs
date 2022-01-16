using AutoMapper;
using Basket.API.Entities;
using Basket.API.gRPC;
using Basket.API.Repositories;
using EventBus.Messages.Events;
using MassTransit;
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
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEnpoint;
        public BasketController(IBasketRepository repo, DiscountGrpcService discountGrpcService, IMapper mapper, IPublishEndpoint publishEnpoint)
        {
            _repo = repo;
            _discountGrpcService = discountGrpcService;
            _mapper = mapper;
            _publishEnpoint = publishEnpoint;
        }
        [Route("[action]/{username}", Name = "GetBasket")]
        [HttpGet]
        [ProducesResponseType(typeof(ShoppingCart), (int) HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetBasket(string username)
        {
            var result = await _repo.GetBasket(username);
            return Ok(result ?? new ShoppingCart(username));
        }
        [Route("[action]/{username}", Name = "UpdateBasket")]
        [HttpPut]
        [ProducesResponseType(typeof(ShoppingCart), (int) HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
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
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteBasket(string username)
        {
            await _repo.DeleteBasket(username);
            return Ok();
        }
        [HttpPost("[action]", Name = "CheckOut")]
        [ProducesResponseType((int) HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            // get existing basket with total price
            // create basketcheckoutevent - set totalprice on basketcheckout eventmessage
            // send checkout event to rabbitmq
            // remove the basket

            // get existing basket with total price
            var basket = await _repo.GetBasket(basketCheckout.Username);
            if(basket == null)
            {
                return BadRequest();
            }

            // send checkout event to rabbitmq
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;
            await _publishEnpoint.Publish(eventMessage);

            // remove the basket
            await _repo.DeleteBasket(basket.Username);
            return Accepted();
        }
    }
}
