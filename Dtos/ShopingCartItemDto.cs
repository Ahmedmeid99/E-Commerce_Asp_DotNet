
namespace E_Commerce_BusniessLayer
{
    public class ShopingCartItemDto
    {
        public int ShopingCartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
