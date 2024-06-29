
namespace E_Commerce_BusniessLayer
{
    public class ProductDto
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public int CategoryId { get; set; }
        public int LikesCount { get; set; }
        public int FavoritesCount { get; set; }

    }
}
