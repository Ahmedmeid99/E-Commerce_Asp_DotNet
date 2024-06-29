
namespace E_Commerce_BusniessLayer
{
    public class OrderDto
    {
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public byte Status { get; set; }   // completed / Cancled / in the way

    }

}
