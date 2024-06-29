using E_Commerce_BusniessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_API_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        [HttpGet] // api/OrderItems
        public async Task<IActionResult> GetAllAsync()
        {
            var OrderItems = await clsOrderItem.GetAllOrdersAsync();

            if (OrderItems.Count == 0)
                return NoContent();

            return Ok(OrderItems);
        }
       
        [HttpGet("order/{id}")] // api/OrderItems/customer/3
        public async Task<IActionResult> GetAllAsync(int id)
        {
            var OrderItems = await clsOrderItem.GetAllOrderItemsAsync(id);
            if (OrderItems.Count == 0)
                return NoContent();

            return Ok(OrderItems);
        }

        [HttpPost] // api/OrderItems/    + data will send in body
        public async Task<IActionResult> PostAsync([FromBody] OrderItemDto dto)
        {
            clsProduct Product = await clsProduct.FindAsync(dto.ProductId);

            var OrderItem = new clsOrderItem();
            OrderItem.OrderId = dto.OrderId;
            OrderItem.ProductId = dto.ProductId;
            OrderItem.Quantity = dto.Quantity;
            OrderItem.Price = Product.Price;
            OrderItem.TotalPrice = dto.Price * dto.Quantity;

            if (await OrderItem.SaveAsync())
                return Ok(OrderItem);
            else
                return Conflict("this orderItem has not been added.");
        }

        [HttpPut("{id}")] // api/OrderItems/5    + data will send in body
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] OrderItemDto dto)
        {
            var OrderItem = await clsOrderItem.FindAsync(id);

            if (OrderItem == null)
                return NotFound($"this oredrItem id : {id} not found.");

            clsProduct Product = await clsProduct.FindAsync(dto.ProductId);

            OrderItem.OrderId = dto.OrderId;
            OrderItem.ProductId = dto.ProductId;
            OrderItem.Quantity = dto.Quantity;
            OrderItem.Price = Product.Price;
            OrderItem.TotalPrice = dto.Price * dto.Quantity;

            if (await OrderItem.SaveAsync())
                return Ok(OrderItem);
            else
                return Conflict("this oredrItem has not been added.");
        }

        [HttpDelete("{id}")] // api/OrderItems/5
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var OrderItem = await clsOrderItem.FindAsync(id);

            if (OrderItem == null)
                return NotFound($"this orderItem id : {id} not found.");

            if (await clsOrderItem.DeleteAsync(id))
                return Ok(OrderItem);
            else
                return Conflict("this oredrItem exist but has not be deleted.");

        }

        [HttpGet("{id}")] // api/OrderItems/5
        public async Task<IActionResult> GetOrderItemAsync(int id)
        {
            var OrderItem = await clsOrderItem.FindAsync(id);

            if (OrderItem == null)
                return NotFound($"this orderItem id : {id} not found.");

            return Ok(OrderItem);
        }

        [HttpGet("isexists/{id}")] // api/OrderItems/isexists/5
        public async Task<IActionResult> IsOrderItemExistsAsync(int id)
        {
            if (!await clsOrderItem.IsExistsAsync(id))
                return NotFound($"this orderItem id : {id} not found.");

            return Ok(true);
        }

    }
}
