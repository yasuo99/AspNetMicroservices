using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Discount.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CouponController : ControllerBase
    {
        private readonly ICouponRepository _repo;
        public CouponController(ICouponRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("[action]/{productName}", Name = "GetCoupons")]
        [ProducesResponseType(typeof(IEnumerable<Coupon>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCouponsByProductName(string productName)
        {
            return Ok(await _repo.GetCoupons(productName));
        }

        [HttpGet("{id}", Name = "GetCoupon")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCoupon(int id)
        {
            var coupon = await _repo.GetCoupon(id);

            if (coupon == null)
            {
                return NotFound();
            }
            return Ok(coupon);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> CreateCoupon([FromBody] Coupon coupon)
        {
            var result = await _repo.CreateCoupon(coupon);

            if (!result)
            {
                return NoContent();
            }
            return CreatedAtAction("GetCoupon", new { id = coupon.Id }, coupon);
        }
        [HttpPut("{id}", Name = "UpdateCoupon")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateCoupon(int id, [FromBody] Coupon coupon)
        {
            if (id != coupon.Id)
            {
                return BadRequest();
            }
            var result = await _repo.UpdateCoupon(coupon);

            if (!result)
            {
                return NotFound();
            }
            return Ok(coupon);
        }
        [HttpDelete("{id}", Name = "DeleteCoupon")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteCoupon(int id)
        {
            var coupon = await _repo.GetCoupon(id);
            if(coupon == null)
            {
                return NotFound();
            }
            return Ok(await _repo.DeleteCoupon(id));
        }
        [HttpDelete("[action]/{productName}", Name = "DeleteCouponByProductName")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteCouponByProductName(string productName)
        {
            return Ok(await _repo.DeleteCouponByProductName(productName));
        }
    }
}
