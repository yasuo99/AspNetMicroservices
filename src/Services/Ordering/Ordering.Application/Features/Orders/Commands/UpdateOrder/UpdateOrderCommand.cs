using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Constracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public record UpdateOrderCommand(int id, UpdateOrderDTO updateOrder): IRequest;
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateOrderCommandHandler> _logger;
        public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<UpdateOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.id);
            if(order == null)
            {
                throw new NotFoundException($"Order {order.Id} not founded");
            }
            _mapper.Map(request.updateOrder, order);

            await _orderRepository.UpdateAsync(order);

            _logger.LogInformation($"Order {order.Id} is updated");
            return Unit.Value;
        }
    }
}
