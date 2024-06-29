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
    public static class OrderItemDataAccess
    {
        public static async Task <OrderItemDto> GetOrderItemByIDAsync(int OrderItemId)
        {
            OrderItemDto orderItemDto = new OrderItemDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "SELECT * FROM OrderItems WHERE OrderItemId = @OrderItemId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@OrderItemId", OrderItemId);

                        using (SqlDataReader reader =await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {

                                // The record was found

                                orderItemDto.OrderId = (int)reader["OrderId"];
                                orderItemDto.ProductId = (int)reader["ProductId"];
                                orderItemDto.Quantity = (int)reader["Quantity"];
                                orderItemDto.Price = Convert.ToDecimal(reader["Price"]);
                                orderItemDto.TotalPrice = Convert.ToDecimal(reader["TotalPrice"]);
                                
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


            return orderItemDto;
        }


        public static async Task<int> AddNewOrderItemAsync( int OrderId, int ProductId, int Quantity, decimal Price, decimal TotalPrice)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {

                    await connection.OpenAsync();

                    string query = @"INSERT INTO OrderItems 
                            (OrderId,ProductId,Quantity,Price,TotalPrice)
                            VALUES 
                            (@OrderId,@ProductId,@Quantity,@Price,@TotalPrice);
                             SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {


                        command.Parameters.AddWithValue("@OrderId", OrderId);
                        command.Parameters.AddWithValue("@ProductId", ProductId);
                        command.Parameters.AddWithValue("@Quantity", Quantity);
                        command.Parameters.AddWithValue("@Price", Price);
                        command.Parameters.AddWithValue("@TotalPrice", TotalPrice);

                        object result = command.ExecuteScalar();  // to get the OrderItemId just added
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

        public static  async Task <bool> UpdateOrderItemAsync(int OrderItemId, int OrderId, int ProductId, int Quantity, decimal Price, decimal TotalPrice)
        {

            int rowsAffected = 0;

            try
            {

                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {

                    await connection.OpenAsync();

                    string query = @"Update OrderItems  
                            set OrderId = @OrderId, 
                                ProductId = @ProductId, 
                                Quantity = @Quantity, 
                                Price = @Price, 
                                TotalPrice = @TotalPrice
                                
                                where OrderItemId = @OrderItemId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@OrderItemId", OrderItemId);
                        command.Parameters.AddWithValue("@OrderId", OrderId);
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
       
        public static async Task<List<clsOrderItem>> GetOrderItemAsync()
        {

            List<clsOrderItem> orderItems = new List<clsOrderItem>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    string query = "SELECT * FROM OrderItems order by OrderId"; //  OrderItemsFullData

                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        using (SqlDataReader reader =await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                clsOrderItem orderItem = new clsOrderItem
                                {
                                    OrderItemId = reader.GetInt32(reader.GetOrdinal("OrderItemId")),
                                    OrderId = reader.GetInt32(reader.GetOrdinal("OrderId")),
                                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice")) 
                                };

                                orderItems.Add(orderItem);

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

            return orderItems;

        }
        public static async Task<List<clsOrderItem>> GetOrderItemsOfOrderAsync(int OrderId)
        {

            List<clsOrderItem> orderItems = new List<clsOrderItem>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    string query = "SELECT * FROM OrderItems WHERE OrderId = @OrderId"; //  OrderItemsFullData

                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@OrderId", OrderId);

                        using (SqlDataReader reader =await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                clsOrderItem orderItem = new clsOrderItem
                                {
                                    OrderItemId = reader.GetInt32(reader.GetOrdinal("OrderItemId")),
                                    OrderId = reader.GetInt32(reader.GetOrdinal("OrderId")),
                                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice")) 
                                };

                                orderItems.Add(orderItem);

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

            return orderItems;

        }

        public static  async Task <bool> DeleteOrderItemAsync(int OrderItemId)
        {
            int RowAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "DELETE FROM OrderItems WHERE OrderItemId = @OrderItemId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrderItemId", OrderItemId);

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

        public static  async Task <bool> IsOrderItemExistsAsync(int OrderItemId)
        {
            bool IsFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "SELECT FOUND = 1 FROM OrderItems WHERE OrderItemId = @OrderItemId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrderItemId", OrderItemId);

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
