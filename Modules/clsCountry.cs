using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce_DataAccessLayer;

namespace E_Commerce_BusniessLayer
{
    public class clsCountry
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }

        enum enMode { AddNew = 1, Update = 2 }
        enMode Mode = enMode.AddNew;

        // default constractor 
        public clsCountry()
        {
            CountryID = -1;
            CountryName = "";

            Mode = enMode.AddNew;
        }

        // private constractor 
        public clsCountry(int CountryID, string CountryName)
        {
            this.CountryID = CountryID;
            this.CountryName = CountryName;

            Mode = enMode.Update;
        }



        public static async Task<clsCountry> FindAsync(int ID)
        {
            // call DataAccess function 


            CountryDto countryDto = await CountryDataAccess.GetCountryByIDAsync(ID);

            if (countryDto != null)
                return new clsCountry(ID, countryDto.CountryName);
            else
                return null;
        }

        public static async Task<bool> IsCountyExistsAsync(string Name)
        {
            return await CountryDataAccess.IsCountyExistsAsync(Name);
        }

        public static async Task<List<clsCountry>> GetAllCountriesAsync()
        {
            return await CountryDataAccess.GetAllCountriesAsync();
        }
    }
}
