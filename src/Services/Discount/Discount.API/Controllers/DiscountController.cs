using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Discount.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountController(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository ?? throw new ArgumentNullException(nameof(discountRepository));
        }

        [HttpGet("productName", Name = "GetDiscount")]
        [ProducesResponseType(typeof(IEnumerable<Coupon>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> GetDiscount(string productName)
        {
            var discount = await _discountRepository.GetDiscount(productName);
            return Ok(discount);
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<Coupon>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> CreateDiscount(Coupon coupon)
        {
            await _discountRepository.CreateDiscount(coupon);

            return CreatedAtRoute("GetDiscount", new { id = coupon.Id }, coupon);
        }

        [HttpPut]
        [ProducesResponseType(typeof(IEnumerable<Coupon>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> UpdateDiscount(Coupon coupon)
        {
            return Ok(await _discountRepository.UpdateDiscount(coupon));
        }

        [HttpDelete("{productName}", Name = "DeleteDiscount")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<Coupon>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> DeleteDiscount(string productName)
        {
            return Ok(await _discountRepository.DeleteDiscount(productName));
        }
    }
}
