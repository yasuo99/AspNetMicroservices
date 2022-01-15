using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.PlaceOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersByUsername;
using Ordering.Application.Models;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController: ControllerBase
    {
        private readonly IMediator _mediator;
        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("{username}", Name = "GetOrders")]
        [ProducesResponseType(typeof(Response<List<OrderVM>>), (int) HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetUserOrders(string username, CancellationToken cancellationToken)
        {
            var query = new GetOrdersByUsernameQuery(username);

            var orders = await _mediator.Send(query, cancellationToken);

            return Ok(orders);
        }
        [HttpPost(Name = "PlaceOrder")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PlaceOrder([FromBody] CreateOrderDTO createOrderDTO, CancellationToken cancellationToken)
        {
            var command = new PlaceOrderCommand(createOrderDTO);
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result); 
        }
        [HttpPut("{id}", Name = "UpdateOrder")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDTO updateOrder, CancellationToken cancellationToken)
        {
            if(id != updateOrder.Id)
            {
                return BadRequest();
            }
            var command = new UpdateOrderCommand(id, updateOrder);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
        [HttpDelete("{id}", Name = "DeleteOrder")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var command = new DeleteOrderCommand(id);

            await _mediator.Send(command);

            return NoContent();
        }

    }
}
