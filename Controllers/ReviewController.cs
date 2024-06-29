using E_Commerce_BusniessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_API_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        [HttpGet("product/{id}")] // api/Reviews/product/3
        public async Task<IActionResult> GetAllProductReviewsAsync(int id)
        {
            var Reviews = await clsReview.GetProductReviewsAsync(id);

            if (Reviews.Count == 0)
                return NoContent();

            return Ok(Reviews);
        } 
        
        [HttpGet("customer/{id}")] // api/Reviews/customer/3
        public async Task<IActionResult> GetAllCustomerReviewsAsync(int id)
        {
            var Reviews = await clsReview.GetCustomerReviewsAsync(id);

            if (Reviews.Count == 0)
                return NoContent();

            return Ok(Reviews);
        }

        [HttpPost] // api/Reviews/    + data will send in body
        public async Task<IActionResult> PostAsync([FromBody] ReviewDto dto)
        {
            var Review = new clsReview();
            Review.CustomerId = dto.CustomerId;
            Review.ProductId = dto.ProductId;
            Review.ReviewText = dto.ReviewText;
            Review.Rating = dto.Rating;
            Review.ReviewDate = dto.ReviewDate;

            if (await Review.SaveAsync())
                return Ok(Review);
            else
                return Conflict("this review has not been added.");
        }

        [HttpPut("customer/{customerId}/Review/{ReviewId}")] // api/Reviews/customer/1006/5    + data will send in body
        public async Task<IActionResult> UpdateAsync(int customerId, int ReviewId, [FromBody] ReviewDto dto)
        {
            var Review = await clsReview.FindAsync(ReviewId);

            if (Review == null)
                return NotFound($"this review id : {ReviewId} not found.");

            Review.CustomerId = dto.CustomerId;
            Review.ProductId = dto.ProductId;
            Review.ReviewText = dto.ReviewText;
            Review.Rating = dto.Rating;
            Review.ReviewDate = dto.ReviewDate;
            
            if(customerId != Review.CustomerId)
                return Conflict("you have not the permission to update this review");

            if (await Review.SaveAsync())
                return Ok(Review);
            else
                return Conflict("this review has not been updated.");
        }

        [HttpDelete("customer/{customerId}/Review/{ReviewId}")] // api/Reviews/5
        public async Task<IActionResult> DeleteAsync(int customerId, int ReviewId)
        {
            var Review = await clsReview.FindAsync(ReviewId);

            if (Review == null)
                return NotFound($"this review id : {ReviewId} not found.");

            if (customerId != Review.CustomerId)
                return Conflict("you have not the permission to delete this review");

            if (await clsReview.DeleteAsync(ReviewId))
                return Ok(Review);
            else
                return Conflict("this review exist but has not be deleted.");

        }

        [HttpGet("{id}")] // api/Reviews/5
        public async Task<IActionResult> GetReviewAsync(int id)
        {
            var Review = await clsReview.FindAsync(id);

            if (Review == null)
                return NotFound($"this review id : {id} not found.");

            return Ok(Review);
        }

        [HttpGet("isexists/{id}")] // api/Reviews/isexists/5
        public async Task<IActionResult> IsReviewExistsAsync(int id)
        {
            if (!await clsReview.IsExistsAsync(id))
                return NotFound($"this review id : {id} not found.");

            return Ok(true);
        }

    }
}
