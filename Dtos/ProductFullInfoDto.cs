namespace E_Commerce_API_Layer.Dtos
{
    public class ProductFullInfoDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public int CategoryId  { get; set; }
        public string CategoryName { get; set; }
        public int LikesCount { get; set; }
        public int FavoritesCount { get; set; }
        public string ImageURL { get; set; }
        public byte ImageOrder { get; set; }
    }
}
