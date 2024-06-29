using E_Commerce_BusniessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_API_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        [HttpGet] // api/Countrys
        public async Task<IActionResult> GetAllAsync()
        {
            var Countrys = await clsCountry.GetAllCountriesAsync();
            if (Countrys.Count == 0)
                return NoContent();

            return Ok(Countrys);
        }

        

        [HttpGet("{id}")] // api/Countrys/5
        public async Task<IActionResult> GetCountryAsync(int id)
        {
            var Country = await clsCountry.FindAsync(id);

            if (Country == null)
                return NotFound($"this id {id} not found.");

            return Ok(Country);
        }

        [HttpGet("isexists/{name}")] // api/Countrys/isexists/egypt
        public async Task<IActionResult> IsCountryExistsAsync(string name)
        {
            bool IsFound = await clsCountry.IsCountyExistsAsync(name);

            if (!IsFound)
                return NotFound($"this Country : {name} not found.");

            return Ok(true);
        }
    }
}
