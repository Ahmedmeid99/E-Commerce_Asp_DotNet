using E_Commerce_BusniessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_API_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        [HttpGet] // api/ProductCategories
        public async Task<IActionResult> GetAllAsync()
        {
            var ProductCategories = await clsProductCategory.GetAllCategoriesAsync();
            
            if (ProductCategories.Count == 0)
                return NoContent();

            return Ok(ProductCategories);
        }

        [HttpPost] // api/ProductCategories/    + data will send in body
        public async Task<IActionResult> PostAsync([FromBody] ProductCategoryDto dto)
        {
            var ProductCategory = new clsProductCategory();
            ProductCategory.CategoryName = dto.CategoryName;

            if (await ProductCategory.SaveAsync())
                return Ok(ProductCategory);
            else
                return Conflict("this productCategory has not been added.");
        }

        [HttpPut("{id}")] // api/ProductCategories/5    + data will send in body
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] ProductCategoryDto dto)
        {
            var ProductCategory = await clsProductCategory.FindAsync(id);

            if (ProductCategory == null)
                return NotFound($"this productCategory id : {id} not found.");
            
            ProductCategory.CategoryName = dto.CategoryName;

            if (await ProductCategory.SaveAsync())
                return Ok(ProductCategory);
            else
                return Conflict("this productCategory has not been added.");
        }

        [HttpDelete("{id}")] // api/ProductCategories/5
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var ProductCategory = await clsProductCategory.FindAsync(id);

            if (ProductCategory == null)
                return NotFound($"this productCategory id : {id} not found.");

            if (await clsProductCategory.DeleteAsync(id))
                return Ok(ProductCategory);
            else
                return Conflict("this productCategory exist but has not be deleted.");

        }

        [HttpGet("{id}")] // api/ProductCategories/5
        public async Task<IActionResult> GetProductCategoryAsync(int id)
        {
            var ProductCategory = await clsProductCategory.FindAsync(id);

            if (ProductCategory == null)
                return NotFound($"this productCategory id : {id} not found.");

            return Ok(ProductCategory);
        }

        [HttpGet("isexists/{id}")] // api/ProductCategories/isexists/5
        public async Task<IActionResult> IsProductCategoryExistsAsync(int id)
        {
            if (!await clsProductCategory.IsExistsAsync(id))
                return NotFound($"this productCategory id : {id} not found.");

            return Ok(true);
        }

    }
}
