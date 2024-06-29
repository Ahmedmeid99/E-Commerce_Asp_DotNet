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
    public static class ShopingCartItemDataAccess
    {
        public static  async Task <ShopingCartItemDto> GetShopingCartItemByIDAsync(int ShopingCartItemId)
        {

            ShopingCartItemDto shopingCartItemDto = new ShopingCartItemDto();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "SELECT * FROM ShopingCartItems WHERE ShopingCartItemId = @ShopingCartItemId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ShopingCartItemId", ShopingCartItemId);

                        using (SqlDataReader reader =await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {

                                // The record was found

                                shopingCartItemDto.ShopingCartId = (int)reader["ShopingCartId"];
                                shopingCartItemDto.ProductId = (int)reader["ProductId"];
                                shopingCartItemDto.Quantity = (int)reader["Quantity"];
                                shopingCartItemDto.Price = Convert.ToDecimal(reader["Price"]);
                                shopingCartItemDto.TotalPrice = Convert.ToDecimal(reader["TotalPrice"]);

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


            return shopingCartItemDto;
        }


        public static async Task<int> AddNewShopingCartItemAsync(int ShopingCartId, int ProductId, int Quantity, decimal Price, decimal TotalPrice)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {

                    await connection.OpenAsync();

                    string query = @"INSERT INTO ShopingCartItems 
                            (ShopingCartId,ProductId,Quantity,Price,TotalPrice)
                            VALUES 
                            (@ShopingCartId,@ProductId,@Quantity,@Price,@TotalPrice);
                             SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {


                        command.Parameters.AddWithValue("@ShopingCartId", ShopingCartId);
                        command.Parameters.AddWithValue("@ProductId", ProductId);
                        command.Parameters.AddWithValue("@Quantity", Quantity);
                        command.Parameters.AddWithValue("@Price", Price);
                        command.Parameters.AddWithValue("@TotalPrice", TotalPrice);

                        object result =await command.ExecuteScalarAsync();  // to get the ShopingCartItemId just added
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


        public static  async Task <bool> UpdateShopingCartItemAsync(int ShopingCartItemId, int ShopingCartId, int ProductId, int Quantity, decimal Price, decimal TotalPrice)
        {

            int rowsAffected = 0;

            try
            {

                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {

                    await connection.OpenAsync();

                    string query = @"Update ShopingCartItems  
                            set ShopingCartId = @ShopingCartId, 
                                ProductId = @ProductId, 
                                Quantity = @Quantity, 
                                Price = @Price, 
                                TotalPrice = @TotalPrice 
                                
                                where ShopingCartItemId = @ShopingCartItemId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ShopingCartItemId", ShopingCartItemId);
                        command.Parameters.AddWithValue("@ShopingCartId", ShopingCartId);
                        command.Parameters.AddWithValue("@ProductId", ProductId);
                        command.Parameters.AddWithValue("@Quantity", Quantity);
                        command.Parameters.AddWithValue("@Price", Price);
                        command.Parameters.AddWithValue("@TotalPrice", TotalPrice);

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

        public static async Task<List<clsShopingCartItem>> GetAllShopingCartItemsAsync(int ShopingCartId)
        {

            List<clsShopingCartItem> shopingCartItems = new List<clsShopingCartItem>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    string query = "SELECT * FROM ShopingCartItems WHERE ShopingCartId = @ShopingCartId"; //  ShopingCartItemsFullData

                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ShopingCartId", ShopingCartId);

                        using (SqlDataReader reader =await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                clsShopingCartItem shopingCartitem = new clsShopingCartItem
                                {
                                    ShopingCartItemId = reader.GetInt32(reader.GetOrdinal("ShopingCartItemId")),
                                    ShopingCartId = reader.GetInt32(reader.GetOrdinal("ShopingCartId")),
                                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice"))

                                };

                                shopingCartItems.Add(shopingCartitem);

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

            return shopingCartItems;

        }

        public static  async Task <bool> DeleteShopingCartItemAsync(int ShopingCartItemId)
        {
            int RowAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "DELETE FROM ShopingCartItems WHERE ShopingCartItemId = @ShopingCartItemId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ShopingCartItemId", ShopingCartItemId);

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

        public static async Task<bool> ClearShopingCartItemsAsync(int ShopingCartId)
        {
            int RowAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "DELETE FROM ShopingCartItems WHERE ShopingCartId = @ShopingCartId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ShopingCartId", ShopingCartId);

                        RowAffected = await command.ExecuteNonQueryAsync();
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

        public static  async Task <bool> IsShopingCartItemExistsAsync(int ShopingCartItemId)
        {
            bool IsFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "SELECT FOUND = 1 FROM ShopingCartItems WHERE ShopingCartItemId = @ShopingCartItemId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ShopingCartItemId", ShopingCartItemId);

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
