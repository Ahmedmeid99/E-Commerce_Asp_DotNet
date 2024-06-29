using E_Commerce_BusniessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_API_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopingCartController : ControllerBase
    {
        [HttpGet("customer/{id}")] // api/ShopingCarts/customer/3
        public async Task<IActionResult> GetCustomerShopingCartsAsync(int id)
        {
            var ShopingCart = await clsShopingCart.FindByCustomerIDAsync(id);

            if (ShopingCart == null)
                return NotFound($"this shopingCart id : {id} not found.");

            return Ok(ShopingCart);
        }

        [HttpGet("customer/{id}/All")] // api/ShopingCarts/customer/3
        public async Task<IActionResult> GetAllCustomerShopingCartsAsync(int id)
        {
            var ShopingCarts = await clsShopingCart.GetAllShopingCartsOfCustomerAsync(id);

            if (ShopingCarts.Count == 0)
                return NoContent();

            return Ok(ShopingCarts);
        }

        [HttpPost] // api/ShopingCarts/    + data will send in body
        public async Task<IActionResult> PostAsync([FromBody] ShopingCartDto dto)
        {
            var ShopingCart = new clsShopingCart();
            ShopingCart.CustomerId = dto.CustomerId;
            ShopingCart.CreatedAt = dto.CreatedAt;
            ShopingCart.UpdatedAt = dto.UpdatedAt;

            if (await ShopingCart.SaveAsync())
                return Ok(ShopingCart);
            else
                return Conflict("this shopingCart has not been added.");
        }

        [HttpPut("{id}")] // api/ShopingCarts/5    + data will send in body
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] ShopingCartDto dto)
        {
            var ShopingCart = await clsShopingCart.FindAsync(id);

            if (ShopingCart == null)
                return NotFound($"this shopingCart id : {id} not found.");

            ShopingCart.CustomerId = dto.CustomerId;
            ShopingCart.CreatedAt = dto.CreatedAt;
            ShopingCart.UpdatedAt = dto.UpdatedAt;

            if (await ShopingCart.SaveAsync())
                return Ok(ShopingCart);
            else
                return Conflict("this shopingCart has not been added.");
        }

        [HttpDelete("{id}")] // api/ShopingCarts/5
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var ShopingCart = await clsShopingCart.FindAsync(id);

            if (ShopingCart == null)
                return NotFound($"this shopingCart id : {id} not found.");

            if (await clsShopingCart.DeleteAsync(id))
                return Ok(ShopingCart);
            else
                return Conflict("this shopingCart exist but has not be deleted.");

        }

        [HttpGet("{id}")] // api/ShopingCarts/5
        public async Task<IActionResult> GetShopingCartAsync(int id)
        {
            var ShopingCart = await clsShopingCart.FindAsync(id);

            if (ShopingCart == null)
                return NotFound($"this shopingCart id : {id} not found.");

            return Ok(ShopingCart);
        }

        [HttpGet("isexists/{id}")] // api/ShopingCarts/isexists/5
        public async Task<IActionResult> IsShopingCartExistsAsync(int id)
        {
            if (!await clsShopingCart.IsExistsAsync(id))
                return NotFound($"this shopingCart id : {id} not found.");

            return Ok(true);
        }

    }
}
