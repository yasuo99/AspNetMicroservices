using AutoMapper;
using Ordering.Application.Features.Orders.Commands.PlaceOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersByUsername;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrderVM>().ReverseMap();
            CreateMap<CreateOrderDTO, Order>().ReverseMap();
            CreateMap<UpdateOrderDTO, Order>().ReverseMap();
        }
    }
}
