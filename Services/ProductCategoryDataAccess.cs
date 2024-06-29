using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using E_Commerce_DataAccessLayer.Globals;
using E_Commerce_BusniessLayer;

namespace E_Commerce_DataAccessLayer
{
    public static class ProductCategoryDataAccess
    {
        public static  async Task <ProductCategoryDto> GetProductCategoryByIDAsync(int CategoryId)
        {

            ProductCategoryDto productCategoryDto = new ProductCategoryDto();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "SELECT * FROM ProductCategory WHERE CategoryId = @CategoryId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@CategoryId", CategoryId);

                        using (SqlDataReader reader =await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {

                                // The record was found
                                productCategoryDto.CategoryName = (string)reader["CategoryName"];

                                reader.Close();
                            }
                            else
                            {
                                // The record was not found
                                return null;
                            }

                        }


                    }
                    connection.Close();

                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }


            return productCategoryDto;
        }

        public static async Task<int> AddNewProductCategoryAsync(string CategoryName)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {

                    await connection.OpenAsync();

                    string query = @"INSERT INTO ProductCategory 
                            (CategoryName)
                            VALUES 
                            (@CategoryName);
                             SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {


                        command.Parameters.AddWithValue("@CategoryName", CategoryName);

                        object result = await command.ExecuteScalarAsync();  // to get the CategoryId just added
                        connection.Close();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            return insertedID;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            return -1;
        }


        public static  async Task <bool> UpdateProductCategoryAsync(int CategoryId, string CategoryName)
        {

            int rowsAffected = 0;

            try
            {

                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {

                    await connection.OpenAsync();

                    string query = @"Update ProductCategory  
                            set CategoryName = @CategoryName
                                
                                where CategoryId = @CategoryId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@CategoryId", CategoryId);
                        command.Parameters.AddWithValue("@CategoryName", CategoryName);
                        
                        rowsAffected =await command.ExecuteNonQueryAsync();
                    }   
                }


            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }

            return (rowsAffected > 0);
        }


        public static async Task<List<clsProductCategory>> GetAllProductCategoriesAsync()
        {

            List<clsProductCategory> categories = new List<clsProductCategory>(); 
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    string query = "SELECT * FROM ProductCategory "; //  ProductCategoryFullData

                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        using (SqlDataReader reader =await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                clsProductCategory category = new clsProductCategory
                                {
                                    CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                    CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                                    
                                };

                                categories.Add(category);

                            }
                            reader.Close();
                        }
                    }

                    connection.Close();

                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }

            return categories;

        }

        public static  async Task <bool> DeleteProductCategoryAsync(int CategoryId)
        {
            int RowAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "DELETE FROM ProductCategory WHERE CategoryId = @CategoryId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CategoryId", CategoryId);

                        RowAffected =await command.ExecuteNonQueryAsync();
                    }

                    connection.Close();
                }
            }
            catch (Exception Ex)
            {
                // Catch Error
            }

            return (RowAffected > 0);
        }

        public static  async Task <bool> IsProductCategoryExistsAsync(int CategoryId)
        {
            bool IsFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "SELECT FOUND = 1 FROM ProductCategory WHERE CategoryId = @CategoryId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CategoryId", CategoryId);

                        using (SqlDataReader reader =await command.ExecuteReaderAsync())
                        {
                            IsFound = reader.HasRows;

                            reader.Close();
                        }
                    }

                    connection.Close();
                }

            }
            catch (Exception Ex)
            {
                IsFound = false;
            }


            return IsFound;
        }

    }
}
