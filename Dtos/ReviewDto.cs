
namespace E_Commerce_BusniessLayer
{
    public class ReviewDto
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string ReviewText { get; set; }
        public decimal Rating { get; set; }
        public DateTime ReviewDate { get; set; }


    }

}
