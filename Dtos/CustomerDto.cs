
namespace E_Commerce_BusniessLayer.Dtos
{
    public class CustomerDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Gendor { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int CountryID { get; set; }

    }

}