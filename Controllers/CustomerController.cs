using E_Commerce_API_Layer.Dtos;
using E_Commerce_BusniessLayer;
using E_Commerce_BusniessLayer.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_API_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [HttpGet] // api/Customers
        public async Task<IActionResult> GetAllAsync()
        {
            var Customers = await clsCustomer.GetAllCustomersAsync();
            
            if (Customers.Count == 0)
                return NoContent();

            return Ok(Customers);
        }

        [HttpPost] // api/Customers/    + data will send in body
        public async Task<IActionResult> PostAsync([FromBody] CustomerDto dto)
        {
            var Customer = new clsCustomer();
            Customer.UserName = dto.UserName;
            Customer.Password  = dto.Password;
            Customer.Gendor  = dto.Gendor;
            Customer.DateOfBirth  = dto.DateOfBirth;
            Customer.Phone  = dto.Phone;
            Customer.Email  = dto.Email;
            Customer.Address  = dto.Address;
            Customer.CountryID  = dto.CountryID;


            if (await clsCustomer.FindAsync(Customer.UserName, Customer.Password) != null)
                return Conflict($"this customer name: {Customer.UserName} or password: {Customer.Password} already exists");

            if (await Customer.SaveAsync())
                return Ok(Customer);
            else
                return Conflict("this customer has not been added.");
        }

        [HttpPut("{id}")] // api/Customers/5    + data will send in body
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] CustomerDto dto)
        {
            var Customer = await clsCustomer.FindAsync(id);

            if (Customer == null)
                return NotFound($"this customer not found.");          

            Customer.UserName = dto.UserName;
            Customer.Password = dto.Password;
            Customer.Gendor = dto.Gendor;
            Customer.DateOfBirth = dto.DateOfBirth;
            Customer.Phone = dto.Phone;
            Customer.Email = dto.Email;
            Customer.Address = dto.Address;
            Customer.CountryID = dto.CountryID;

            if (await Customer.SaveAsync())
                return Ok(Customer);
            else
                return Conflict(@"this customer has not been Updated.");
        }

        [HttpDelete("{id}")] // api/Customers/5
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var Customer = await clsCustomer.FindAsync(id);

            if (Customer == null)
                return NotFound($"this customer not found.");

            if (await clsCustomer.DeleteAsync(id))
                return Ok(Customer);
            else
                return Conflict("this customer exist but has not be deleted.");

        }


        [HttpGet("{id}")] // api/Customers/5
        public async Task<IActionResult> GetCustomerAsync(int id)
        {
            var Customer = await clsCustomer.FindAsync(id);

            if (Customer == null)
                return NotFound($"this customer not found.");

            return Ok(Customer);
        }   
        
        [HttpGet("isexists/{id}")] // api/Customers/isexists/5
        public async Task<IActionResult> IsCustomerExistsAsync(int id)
        {
            bool IsExists = await clsCustomer.IsExistsAsync(id);

            if (IsExists)
                return Ok(true);
            
            return NotFound($"this customer not found.");
        }  
        
        [HttpPost("login")] // api/Customers/login + data in body
        public async Task<IActionResult> GetLogedInCustomerAsync([FromBody] LoginDto dto)
        {
            var Customer = await clsCustomer.FindAsync(dto.username, dto.password);

            if (Customer == null)
                return NotFound($"this username or password is incorrect.");

            return Ok(Customer);
        }

       
    }
}
