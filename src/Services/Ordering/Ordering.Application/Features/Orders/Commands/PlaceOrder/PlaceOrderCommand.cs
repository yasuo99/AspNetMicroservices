using AutoMapper;
using Microsoft.Extensions.Logging;
using Ordering.Application.Constracts.Infrastructure;
using Ordering.Application.Constracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.PlaceOrder
{
    public record PlaceOrderCommand(CreateOrderDTO createOrder): IRequestWrapper<int>;
    public class PlaceOrderCommandHandler : IHandlerWrapper<PlaceOrderCommand, int>
    {
        private readonly ILogger<PlaceOrderCommandHandler> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public PlaceOrderCommandHandler(ILogger<PlaceOrderCommandHandler> logger, IOrderRepository orderRepository, IMapper mapper, IEmailService emailService)
        {
            _logger = logger;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _emailService = emailService;
        }
        public async Task<Response<int>> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Order>(request.createOrder);
            if(order.PaymentMethod == Domain.Constants.PaymentMethod.CREDIT_CARD)
            {
                // Simulate the payment
            }
            var newOrder = await _orderRepository.AddAsync(order);

            _logger.LogInformation($"Order {newOrder.Id} of user {order.Username} created successfully");

            await SendMail(newOrder);

            return Response.Created("Order placed ", newOrder.Id);
        }

        private async Task SendMail(Order newOrder)
        {
            var email = new Email
            {
                To = newOrder.Email,
                Body = @$"Your order {newOrder.Id} has been placed
                        Thank you for choosing us!
                        Best Regards,",
                Subject = "Order successfully"
            };

            try
            {
                await _emailService.SendEmailAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Order {newOrder.Id} failed due to an error with the mail service: {ex.Message}");
            }
        }
    }
}
