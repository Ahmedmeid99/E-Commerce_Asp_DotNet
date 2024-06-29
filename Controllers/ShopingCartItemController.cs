using E_Commerce_API_Layer.Dtos;
using E_Commerce_BusniessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_API_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopingCartItemController : ControllerBase
    {
        
        [HttpGet("shopingcart/{id}")] // api/ShopingCartItems/shopingcart/3
        public async Task<IActionResult> GetAllAsync(int id)
        {
            var ShopingCartItems = await clsShopingCartItem.GetAllAsync(id);

            if (ShopingCartItems.Count == 0)
                return NoContent();

            return Ok(ShopingCartItems);
        }

        [HttpPost] // api/ShopingCartItems/    + data will send in body
        public async Task<IActionResult> PostAsync([FromBody] ShopingCartItemDto2 dto)
        {
            var ShopingCartItem = new clsShopingCartItem();   // must user cant iter price or TotalPrice or Quantity(just add one by one)
            ShopingCartItem.ShopingCartId = dto.ShopingCartId;
            ShopingCartItem.ProductId = dto.ProductId;
            ShopingCartItem.Quantity = dto.Quantity;

            if (await ShopingCartItem.SaveAsync())
                return Ok(ShopingCartItem);
            else
                return Conflict("this shopingCartItemhas not been added.");
        }

        [HttpPut("{id}")] // api/ShopingCartItems/5    + data will send in body
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] ShopingCartItemDto2 dto)
        {
            var ShopingCartItem = await clsShopingCartItem.FindAsync(id);

            if (ShopingCartItem == null)
                return NotFound($"this shopingCartItem id : {id} not found.");

            ShopingCartItem.ShopingCartId = dto.ShopingCartId;
            ShopingCartItem.ProductId = dto.ProductId;
            ShopingCartItem.Quantity = dto.Quantity;

            if (await ShopingCartItem.SaveAsync())
                return Ok(ShopingCartItem);
            else
                return Conflict("this shopingCartItemhas not been added.");
        }

        [HttpPatch("{id}/increment")] // api/ShopingCartItems/5    + data will send in body
        public async Task<IActionResult> IncrementAsync(int id)
        {
            var ShopingCartItem = await clsShopingCartItem.FindAsync(id);

            if (ShopingCartItem == null)
                return NotFound($"this shopingCartItem id : {id} not found.");

            ShopingCartItem.Quantity += 1 ;

            if (await ShopingCartItem.SaveAsync())
                return Ok(ShopingCartItem);
            else
                return Conflict("this shopingCartItemhas not been incremented.");
        }

        [HttpPatch("{id}/decrement")] // api/ShopingCartItems/5    + data will send in body
        public async Task<IActionResult> DecrementAsync(int id)
        {
            var ShopingCartItem = await clsShopingCartItem.FindAsync(id);

            if (ShopingCartItem == null)
                return NotFound($"this shopingCartItem id : {id} not found.");

            ShopingCartItem.Quantity -= 1;

            if (await ShopingCartItem.SaveAsync())
                return Ok(ShopingCartItem);
            else
                return Conflict("this shopingCartItemhas not been decrement.");
        }

        [HttpDelete("{id}")] // api/ShopingCartItems/5
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var ShopingCartItem = await clsShopingCartItem.FindAsync(id);

            if (ShopingCartItem == null)
                return NotFound($"this shopingCartItem id : {id} not found.");

            if (await clsShopingCartItem.DeleteAsync(id))
                return Ok(ShopingCartItem);
            else
                return Conflict("this shopingCartItemexist but has not be deleted.");

        }

        [HttpGet("{id}")] // api/ShopingCartItems/5
        public async Task<IActionResult> GetShopingCartItemAsync(int id)
        {
            var ShopingCartItem = await clsShopingCartItem.FindAsync(id);

            if (ShopingCartItem == null)
                return NotFound($"this shopingCartItem id : {id} not found.");

            return Ok(ShopingCartItem);
        }

        [HttpGet("isexists/{id}")] // api/ShopingCartItems/isexists/5
        public async Task<IActionResult> IsShopingCartItemExistsAsync(int id)
        {
            if (!await clsShopingCartItem.IsExistsAsync(id))
                return NotFound($"this shopingCartItem id : {id} not found.");

            return Ok(true);
        }

    }
}
