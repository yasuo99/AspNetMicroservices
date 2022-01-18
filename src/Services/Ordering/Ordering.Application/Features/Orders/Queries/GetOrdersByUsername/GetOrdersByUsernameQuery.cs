using AutoMapper;
using MediatR;
using Ordering.Application.Constracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersByUsername
{
    public record GetOrdersByUsernameQuery(string username): IRequest<List<OrderVM>>;


    public class GetOrdersByUsernameQueryHandler : IRequestHandler<GetOrdersByUsernameQuery, List<OrderVM>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public GetOrdersByUsernameQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public async Task<List<OrderVM>> Handle(GetOrdersByUsernameQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetOrdersByUsername(request.username);
            cancellationToken.ThrowIfCancellationRequested();
            return _mapper.Map<List<OrderVM>>(orders);
        }
    }
}
