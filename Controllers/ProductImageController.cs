using E_Commerce_BusniessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_API_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        [HttpGet("product/{id}")] // api/ProductImages/product/3
        public async Task<IActionResult> GetProductImagesAsync(int id)
        {
            var ProductImages = await clsProductImage.GetAllProductImagesAsync(id);

            if (ProductImages == null)
                return NotFound($"this productImage of product : {id} has not been founded");

            if(ProductImages.Count == 0)
                return NoContent();
            

            return Ok(ProductImages);
        }

        [HttpPost] // api/ProductImages/    + data will send in body
        public async Task<IActionResult> PostAsync([FromBody] ProductImageDto dto)
        {
            var ProductImage = new clsProductImage();
            ProductImage.ProductId = dto.ProductId;
            ProductImage.ImageURL = dto.ImageURL;
            ProductImage.ImageOrder = dto.ImageOrder;

            if (await ProductImage.SaveAsync())
                return Ok(ProductImage);
            else
                return Conflict("this ProductImage has not been added.");
        }

        [HttpPut("{id}")] // api/ProductImages/5    + data will send in body
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] ProductImageDto dto)
        {
            var ProductImage = await clsProductImage.FindAsync(id);

            if (ProductImage == null)
                return NotFound($"this productImage id :  {id} not found.");

            ProductImage.ProductId = dto.ProductId;
            ProductImage.ImageURL = dto.ImageURL;
            ProductImage.ImageOrder = dto.ImageOrder;

            if (await ProductImage.SaveAsync())
                return Ok(ProductImage);
            else
                return Conflict("this productImage has not been added.");
        }

        [HttpDelete("{id}")] // api/ProductImages/5
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var ProductImage = await clsProductImage.FindAsync(id);

            if (ProductImage == null)
                return NotFound($"this productImage id :  {id} not found.");

            if (await clsProductImage.DeleteAsync(id))
                return Ok(ProductImage);
            else
                return Conflict("this productImage exist but has not be deleted.");

        }

        [HttpGet("{id}")] // api/ProductImages/5
        public async Task<IActionResult> GetProductImageAsync(int id)
        {
            var ProductImage = await clsProductImage.FindAsync(id);

            if (ProductImage == null)
                return NotFound($"this productImage id :  {id} not found.");

            return Ok(ProductImage);
        }

        [HttpGet("isexists/{id}")] // api/ProductImages/isexists/5
        public async Task<IActionResult> IsProductImageExistsAsync(int id)
        {
            if (!await clsProductImage.IsExistsAsync(id))
                return NotFound($"this productImage id :  {id} not found.");

            return Ok(true);
        }

    }
}
