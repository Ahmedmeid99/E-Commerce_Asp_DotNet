using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using E_Commerce_DataAccessLayer.Globals;
using E_Commerce_BusniessLayer;
namespace E_Commerce_BusniessLayer
{
    public static class CountryDataAccess 
    {
        public static  async Task <bool> IsCountyExistsAsync(string Name)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection"));
            string query = "SELECT FOUND = 1 FROM Countries WHERE CountryName= @CountryName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryName", Name);
            try
            {
                await connection.OpenAsync();
                SqlDataReader reader =await command.ExecuteReaderAsync();
                IsFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception Ex)
            {
                IsFound = false;
            }
            finally
            {
                connection.Close();
            }

            return IsFound;

        }


        public static async Task<List<clsCountry>> GetAllCountriesAsync()
        {
            List<clsCountry> countries = new List<clsCountry>();

            try
            {

                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    string query = "SELECT * FROM Countries";
                    
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader =await command.ExecuteReaderAsync())
                        {
                            while(await reader.ReadAsync())
                            {
                                clsCountry Country = new clsCountry
                                {
                                    CountryID = reader.GetInt32(reader.GetOrdinal("CountryID")),
                                    CountryName = reader.GetString(reader.GetOrdinal("CountryName"))
                                };

                                countries.Add(Country);

                            }

                            reader.Close();
                        }

                    }
                    connection.Close();
                }




            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            
            return countries;
        }


        public static  async Task <CountryDto> GetCountryByIDAsync(int CountryID)
        {
            CountryDto countryDto = new CountryDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    String query = "SELECT * FROM  Countries Where CountryID = @CountryID";
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@CountryID", CountryID);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {


                            if (await reader.ReadAsync())
                            {
                                countryDto.CountryName = (string)reader["CountryName"];

                            }
                            else
                            {
                                return null;
                            }
                            reader.Close();
                        }
                    }
                    connection.Close();
                }

            }
            catch (Exception Ex)
            {
                // throw An Error
                // IsFound = false;
                return null;
            }


            return countryDto;
        }


    }
}
