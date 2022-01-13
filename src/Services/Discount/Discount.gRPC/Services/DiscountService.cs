using AutoMapper;
using Discount.gRPC.Protos;
using Discount.gRPC.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.gRPC.Services
{
    public class DiscountService: DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly ICouponRepository _couponRepository;
        private readonly ILogger<DiscountService> _logger;
        private readonly IMapper _mapper;
        public DiscountService(ICouponRepository couponRepository, ILogger<DiscountService> logger, IMapper mapper)
        {
            _couponRepository = couponRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public override async Task<ListCouponModel> GetCoupons(GetDiscountsRequest request, ServerCallContext context)
        {
            var coupons = await _couponRepository.GetCoupons(request.ProductName);
            return new ListCouponModel {
                Coupons = { _mapper.Map<List<CouponModel>>(coupons) }
            };
        }
        public override async Task<CouponModel> GetCoupon(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon =  await _couponRepository.GetCoupon(request.Id);
            if(coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Coupon with id={request.Id} not found"));
            }

            var couponModel = _mapper.Map<CouponModel>(coupon);

            return couponModel;
        }
    }
}
