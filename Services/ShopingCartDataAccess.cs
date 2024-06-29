using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using E_Commerce_DataAccessLayer.Globals;
using E_Commerce_BusniessLayer;
using E_Commerce_API_Layer.Dtos;

namespace E_Commerce_DataAccessLayer
{
    public static class ShopingCartDataAccess
    {
        public static  async Task <ShopingCartDto> GetShopingCartByIDAsync(int ShopingCartId)
        {

            ShopingCartDto shopingCartDto = new ShopingCartDto();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "SELECT * FROM ShopingCarts WHERE ShopingCartId = @ShopingCartId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ShopingCartId", ShopingCartId);

                        using (SqlDataReader reader =await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {

                                // The record was found

                                shopingCartDto.CustomerId = (int)reader["CustomerId"];
                                shopingCartDto.CreatedAt = (DateTime)reader["CreatedAt"];
                                shopingCartDto.UpdatedAt = (DateTime)reader["UpdatedAt"];

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


            return shopingCartDto;
        }

        public static async Task<ShopingCartCustomerDto> GetShopingCartByCustomerIDAsync(int CustomerId)
        {

            ShopingCartCustomerDto shopingCartDto = new ShopingCartCustomerDto();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "SELECT TOP 1 * FROM ShopingCarts WHERE CustomerId = @CustomerId order by UpdatedAt desc";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@CustomerId", CustomerId);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {

                                // The record was found

                                shopingCartDto.ShopingCartId = (int)reader["ShopingCartId"];
                                shopingCartDto.CreatedAt = (DateTime)reader["CreatedAt"];
                                shopingCartDto.UpdatedAt = (DateTime)reader["UpdatedAt"];

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


            return shopingCartDto;
        }

        public static async Task<int> AddNewShopingCartAsync(int CustomerId, DateTime CreatedAt, DateTime UpdatedAt)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {

                    await connection.OpenAsync();

                    string query = @"INSERT INTO ShopingCarts 
                            (CustomerId,CreatedAt,UpdatedAt)
                            VALUES 
                            (@CustomerId,@CreatedAt,@UpdatedAt);
                             SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@CustomerId", CustomerId);
                        command.Parameters.AddWithValue("@CreatedAt", CreatedAt);
                        command.Parameters.AddWithValue("@UpdatedAt", UpdatedAt);


                        object result =await command.ExecuteScalarAsync();  // to get the ShopingCartId just added
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
        public static  async Task <bool> UpdateShopingCartAsync(int ShopingCartId, int CustomerId, DateTime CreatedAt, DateTime UpdatedAt)
        {

            int rowsAffected = 0;

            try
            {

                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {

                    await connection.OpenAsync();

                    string query = @"Update ShopingCarts  
                            set CustomerId = @CustomerId, 
                                CreatedAt = @CreatedAt, 
                                UpdatedAt = @UpdatedAt
                                
                                where ShopingCartId = @ShopingCartId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ShopingCartId", ShopingCartId);
                        command.Parameters.AddWithValue("@CustomerId", CustomerId);
                        command.Parameters.AddWithValue("@CreatedAt", CreatedAt);
                        command.Parameters.AddWithValue("@UpdatedAt", UpdatedAt);

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

        // maby you need to send to ShopingcartItems
        public static async Task<List<clsShopingCart>> GetShopingCartsOfCustomerAsync(int CustomerId)
        {

            List<clsShopingCart> shopingCarts = new List<clsShopingCart>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    string query = "SELECT * FROM ShopingCarts WHERE CustomerId = @CustomerId"; //  ShopingCartsFullData

                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@CustomerId", CustomerId);

                        using (SqlDataReader reader =await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                clsShopingCart shopingCart = new clsShopingCart
                                {
                                    ShopingCartId = reader.GetInt32(reader.GetOrdinal("ShopingCartId")),
                                    CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))

                                };

                                shopingCarts.Add(shopingCart);

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

            return shopingCarts;

        }

        public static  async Task <bool> DeleteShopingCartAsync(int ShopingCartId)
        {
            int RowAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "DELETE FROM ShopingCarts WHERE ShopingCartId = @ShopingCartId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ShopingCartId", ShopingCartId);

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

        public static  async Task <bool> IsShopingCartExistsAsync(int ShopingCartId)
        {
            bool IsFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "SELECT FOUND = 1 FROM ShopingCarts WHERE ShopingCartId = @ShopingCartId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ShopingCartId", ShopingCartId);

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
