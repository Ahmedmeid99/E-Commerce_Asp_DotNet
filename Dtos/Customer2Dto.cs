﻿namespace E_Commerce_API_Layer.Dtos
{
    public class Customer2Dto
    {
        public int CustomerId { get; set; }
        public string Gendor { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int CountryID { get; set; }
    }
}
