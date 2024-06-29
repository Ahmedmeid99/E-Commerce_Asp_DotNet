namespace E_Commerce_API_Layer.Dtos
{
    public class ReviewFullInfoDto
    {
        public int ReviewId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int ProductId { get; set; }
        public string ReviewText { get; set; }
        public decimal Rating { get; set; }
        public DateTime ReviewDate { get; set; }
    }

}