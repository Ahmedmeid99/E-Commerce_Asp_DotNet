using E_Commerce_BusniessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_API_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpGet] // api/Orders
        public async Task<IActionResult> GetAllAsync()
        {
            var Orders = await clsOrder.GetOrdersAsync();
            if (Orders.Count == 0)
                return NoContent();

            return Ok(Orders);
        }
       
        [HttpGet("customer/{id}")] // api/Orders/customer/3
        public async Task<IActionResult> GetCustomerOrdersAsync(int id)
        {
            var Orders = await clsOrder.GetCustomerOrdersAsync(id);
            if (Orders.Count == 0)
                return NoContent();

            return Ok(Orders);
        }

        [HttpPost] // api/Orders/    + data will send in body
        public async Task<IActionResult> AddOrderAsync([FromBody] OrderDto dto)
        {                        
            var Order = new clsOrder();
            Order.CustomerId = dto.CustomerId;
            Order.OrderDate = dto.OrderDate;
            Order.TotalAmount = dto.TotalAmount;
            Order.Status = dto.Status;

            if (await Order.SaveAsync())
                return Ok(Order);
            else
                return Conflict("this order has not been added.");
        }
        [HttpPost("{ShopingCartId}")] // api/Orders/    + data will send in body
        public async Task<IActionResult> AddOrderFromSopingCartAsync(int ShopingCartId)
        {
            var ShopingCart = await clsShopingCart.FindAsync(ShopingCartId);
            
            if(ShopingCart==null)
               return NotFound("this ShopingCart not found");

            var Order = new clsOrder();
            decimal TotalAmount = 0;

            Order.CustomerId = ShopingCart.CustomerId;
            Order.OrderDate = DateTime.Now;
            Order.Status = 1;

            if (!await Order.SaveAsync())
                  return Conflict("this order has not been added.");

            var ShopingCartItems = await clsShopingCartItem.GetAllAsync(ShopingCartId);
            if (ShopingCartItems.Count == 0)
                return NoContent();

            foreach (var item in ShopingCartItems)
            {
                // fill OrderItem from  item
                var OrderItem = new clsOrderItem();
                OrderItem.OrderId = Order.OrderId;
                OrderItem.ProductId = item.ProductId;
                OrderItem.Quantity = item.Quantity;
                OrderItem.Price = item.Price;
                OrderItem.TotalPrice = item.TotalPrice;

                if (!await OrderItem.SaveAsync())
                    return Conflict($"this orderItem for product :{OrderItem.ProductId} has not been added.");

                // update (subtrack) Product QuantityInStock from product Quantity
                var Product = await clsProduct.FindAsync(item.ProductId);
                
                Product.QuantityInStock -= OrderItem.Quantity;
                
                if (!await Product.SaveAsync())
                    return Conflict($"this Product :{item.ProductId} has not been updated its QuantityInStock.");


                // after success saving OrderItem ->
                // sum its TotalPrice to athers to calculate TotalAmount of Order
                TotalAmount += OrderItem.TotalPrice;
            }

            Order.TotalAmount = TotalAmount;

            if (await Order.SaveAsync())
            {
                // clear shopingCart , shopingCartItems
                if(!await clsShopingCart.ClearAsync(ShopingCartId))
                    return Conflict("this ShopingCart has not been deleted.");

                return Ok(Order);
            }
            else
                return Conflict("this order has not been added.");

            
        }
        [HttpPut("{id}")] // api/Orders/5    + data will send in body
        public async Task<IActionResult> UpdateOrderAsync(int id, [FromBody] OrderDto dto)
        {
            var Order = await clsOrder.FindAsync(id);

            if (Order == null)
                return NotFound($"this order : {id} not found.");
           
            Order.CustomerId = dto.CustomerId;
            Order.OrderDate = dto.OrderDate;
            Order.TotalAmount = dto.TotalAmount;
            Order.Status = dto.Status;

            if (await Order.SaveAsync())
                return Ok(Order);
            else
                return Conflict("this oder has not been added.");
        }

        [HttpDelete("{id}")] // api/Orders/5
        public async Task<IActionResult> DeleteOrderAsync(int id)
        {
            var Order = await clsOrder.FindAsync(id);

            if (Order == null)
                return NotFound($"this order id : {id} not found.");

            if (await clsOrder.DeleteAsync(id))
                return Ok(Order);
            else
                return Conflict("this order exist but has not been deleted.");

        }

        [HttpGet("{id}")] // api/Orders/5
        public async Task<IActionResult> GetOrderAsync(int id)
        {
            var Order = await clsOrder.FindAsync(id);

            if (Order == null)
                return NotFound($"this id {id} not found.");

            return Ok(Order);
        }

        [HttpGet("isexists/{id}")] // api/Orders/isexists/5
        public async Task<IActionResult> IsOrderExistsAsync(int id)
        {
            if (!await clsOrder.IsExistsAsync(id))
                return NotFound($"this id {id} not found.");

            return Ok(true);
        }

    }
}
