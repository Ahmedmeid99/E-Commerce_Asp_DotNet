using Microsoft.AspNetCore.Mvc;
using System.Data;
using E_Commerce_BusniessLayer;
using E_Commerce_API_Layer.Dtos;

namespace E_Commerce_API_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet] // api/products
        public async Task<IActionResult> GetAllAsync()
        {
            var products = await clsProduct.GetAllProductsAsync();
            
            if (products.Count == 0)
                return NoContent();

            return Ok(products);
        }

        [HttpPost] // api/products/    + data will send in body
        public async Task<IActionResult> PostAsync([FromBody] ProductDto dto)
        {
            var product = new clsProduct();
            product.ProductName = dto.ProductName;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.QuantityInStock = dto.QuantityInStock;
            product.CategoryId = dto.CategoryId;
            product.LikesCount = dto.LikesCount;
            product.FavoritesCount = dto.FavoritesCount;

            if (await product.SaveAsync())
                return Ok(product);
            else
                return Conflict("this product has not been added.");
        }

        [HttpPut("{id}")] // api/products/5    + data will send in body
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] ProductDto dto)
        {
            var product =await clsProduct.FindAsync(id);

            if (product == null)
                return NotFound($"this product id : {id} not found.");

            product.ProductName = dto.ProductName;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.QuantityInStock = dto.QuantityInStock;
            product.CategoryId = dto.CategoryId;
            product.LikesCount = dto.LikesCount;
            product.FavoritesCount = dto.FavoritesCount;

            if (await product.SaveAsync())
                return Ok(product);
            else
                return Conflict("this product has not been added.");
        }

        [HttpDelete("{id}")] // api/products/5
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var product =await clsProduct.FindAsync(id);

            if (product == null)
                return NotFound($"this product id :  {id} not found.");

            if (await clsProduct.DeleteAsync(id))
                return Ok(product);
            else
                return Conflict("this product exist but has not be deleted.");

        }




        [HttpGet("{id}")] // api/products/5
        public async Task<IActionResult> GetProductAsync(int id)
        {
            var product =await clsProduct.FindFullInfoAsync(id);

            if (product == null)
                return NotFound($"this product id : {id} not found.");

            return Ok(product);
        }

        [HttpGet("isexists/{id}")] // api/Customers/isexists/5
        public async Task<IActionResult> IsCustomerExistsAsync(int id)
        {
            if (!await clsProduct.IsExistsAsync(id))
                return NotFound($"this product id : {id} not found.");

            return Ok(true);
        }


        [HttpGet("category/{categoryId}")] // api/products/category/5
        public async Task<IActionResult> GetAllInCategoryAsync(int categoryId)
        {
            var products = await clsProduct.GetProductsAsync(categoryId);
            
            if (products.Count == 0)
                return NoContent();

            return Ok(products);
        }

        [HttpGet("category/{categoryId}/top")] // api/products/category/5/top?count=10
        public async Task<IActionResult> GetTopAsync(int categoryId, [FromQuery] int count)
        {
            var products = await clsProduct.GetTopAsync(categoryId, count);

            if (products.Count == 0)
                return NoContent();

            return Ok(products);
        }  
        
        [HttpGet("category/{categoryId}/related")] // api/products/category/5/related?excludedProduct=2007&count=10
        public async Task<IActionResult> GetRelatedAsync(int categoryId,[FromQuery] int excludedProduct, [FromQuery] int count)
        {
            var products = await clsProduct.GetRelatedAsync(categoryId, excludedProduct, count);

            if (products.Count == 0)
                return NoContent();

            return Ok(products);
        }

        [HttpGet("category/{categoryId}/page")] // api/products/category/3/page/5?pageNumber=1&pageSize=10
        public IEnumerable<ProductFullInfoDto>  GetAllPaginatedAsync(int categoryId,[FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var products = clsProduct.PaginateAsync(categoryId, pageNumber, pageSize);

            return products;
        }

        [HttpGet("category/{categoryId}/inrange")] // api/products/category/5/inrange?min=1&max=10
        public async Task<IActionResult> GetAllInRangeAsync(int categoryId, [FromQuery] int min, [FromQuery] int max)
        {                                                          
            var products = await clsProduct.InRangeAsync(categoryId, min, max);   // min price , max price

            if (products.Count == 0)
                return NoContent();

            return Ok(products);
        } 
        
        [HttpGet("category/{categoryId}/search")] // api/products/category/5/search?trem=pro
        public async Task<IActionResult> SearchAsync(int categoryId, [FromQuery] string term)
        {
            var products = await clsProduct.SearchAsync(categoryId,term);

            if (products.Count == 0)
                return NoContent();

            return Ok(products);
        }
        
        [HttpGet("searchglobal")] // api/products/searchglobal?trem=pro
        public async Task<IActionResult> SearchAsync([FromQuery] string term)
        {
            var products = await clsProduct.SearchAsync(term);

            if (products.Count == 0)
                return NoContent();

            return Ok(products);
        }

        [HttpGet("category/{categoryId}/count")] // api/products/category/5/count
        public async Task<IActionResult> GetCuntAsync(int categoryId)
        {
            int productsCount = await clsProduct.CountAsync(categoryId);
            
            if (productsCount != -1)
                return Ok(productsCount);

            return NotFound("Not fount");
        }

        [HttpGet("category/count")] // api/products/category/count
        public async Task<IActionResult> GetCountAsync()
        {
            int productsCount = await clsProduct.CountAsync();

            if(productsCount != -1)
                return Ok(productsCount);

            return NotFound("Not fount");
        }


    };
}