using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Constracts.Persistence;
using Ordering.Application.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public record DeleteOrderCommand(int id): IRequest;

    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<DeleteOrderCommandHandler> _logger;
        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.id);
            if(order == null)
            {
                throw new NotFoundException($"Order {order.Id} not founded");
            }
            return Unit.Value;
        }
    }
}
