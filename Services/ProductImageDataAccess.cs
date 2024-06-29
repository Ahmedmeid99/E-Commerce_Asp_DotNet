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
    public static class ProductImageDataAccess
    {
        public static  async Task <ProductImageDto> GetProductImageByIDAsync(int ImageId)
        {

            ProductImageDto productImageDto = new ProductImageDto();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "SELECT * FROM ProductImages WHERE ImageId = @ImageId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ImageId", ImageId);

                        using (SqlDataReader reader =await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {

                                // The record was found

                                productImageDto.ProductId = (int)reader["ProductId"];
                                productImageDto.ImageURL = (string)reader["ImageURL"];
                                productImageDto.ImageOrder = Convert.ToByte(reader["ImageOrder"]);
                                

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
                return null;
            }


            return productImageDto;
        }

        public static async Task<int> AddNewProductImageAsync(int ProductId, string ImageURL, byte ImageOrder)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {

                    await connection.OpenAsync();

                    string query = @"INSERT INTO ProductImages 
                            (ProductId,ImageURL,ImageOrder)
                            VALUES 
                            (@ProductId,@ImageURL,@ImageOrder);
                             SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {


                        command.Parameters.AddWithValue("@ProductId", ProductId);
                        command.Parameters.AddWithValue("@ImageURL", ImageURL);
                        command.Parameters.AddWithValue("@ImageOrder", ImageOrder);
                        

                        object result = await command.ExecuteScalarAsync();  // to get the ImageId just added
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


        public static  async Task <bool> UpdateProductImageAsync(int ImageId, int ProductId, string ImageURL, byte ImageOrder)
        {

            int rowsAffected = 0;

            try
            {

                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {

                    await connection.OpenAsync();

                    string query = @"Update ProductImages  
                            set ProductId = @ProductId, 
                                ImageURL = @ImageURL, 
                                ImageOrder = @ImageOrder
                                
                                where ImageId = @ImageId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ImageId", ImageId);
                        command.Parameters.AddWithValue("@ImageURL", ImageURL);
                        command.Parameters.AddWithValue("@ProductId", ProductId);
                        command.Parameters.AddWithValue("@ImageOrder", ImageOrder);

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


        public static async Task<List<clsProductImage>> GetProductImagesOfProductAsync(int ProductId)
        {

            List<clsProductImage> images = new List<clsProductImage>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    string query = "SELECT * FROM ProductImages WHERE ProductId = @ProductId order by ImageOrder"; //  ProductImagesFullData

                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductId", ProductId);
                        using (SqlDataReader reader =await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                clsProductImage image = new clsProductImage
                                {
                                    ImageId = reader.GetInt32(reader.GetOrdinal("ImageId")),
                                    ImageURL = reader.GetString(reader.GetOrdinal("ImageURL")),
                                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    ImageOrder = reader.GetByte(reader.GetOrdinal("ImageOrder"))

                                };
                                images.Add(image);

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
            return images;

        }

        public static  async Task <bool> DeleteProductImageAsync(int ImageId)
        {
            int RowAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "DELETE FROM ProductImages WHERE ImageId = @ImageId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ImageId", ImageId);

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

        public static  async Task <bool> IsProductImageExistsAsync(int ImageId)
        {
            bool IsFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "SELECT FOUND = 1 FROM ProductImages WHERE ImageId = @ImageId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ImageId", ImageId);

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
