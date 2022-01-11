using Discount.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
    public interface ICouponRepository
    {
        Task<IEnumerable<Coupon>> GetCoupons(string productName);
        Task<Coupon> GetCoupon(int id);
        Task<bool> CreateCoupon(Coupon coupon);
        Task<bool> UpdateCoupon(Coupon coupon);
        Task<bool> DeleteCouponByProductName(string productName);
        Task<bool> DeleteCoupon(int id);
    }
}
