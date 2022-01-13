using Discount.gRPC.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.API.gRPC
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
        {
            _discountProtoService = discountProtoService ?? throw new ArgumentNullException(nameof(discountProtoService));
        }
        public async Task<ListCouponModel> GetDiscountsByProductNameAsync(string productName)
        {
            return await _discountProtoService.GetCouponsAsync(new GetDiscountsRequest { ProductName = productName });
        }
    }
}
