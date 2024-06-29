using System;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using E_Commerce_DataAccessLayer.Globals;
using System.Data;
using E_Commerce_BusniessLayer;
namespace E_Commerce_DataAccessLayer
{
    public static class OrderDataAccess
    {
        public static  async Task <OrderDto> GetOrderByIDAsync(int OrderId)
        {

            OrderDto orderDto = new OrderDto();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "SELECT * FROM Orders WHERE OrderId = @OrderId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@OrderId", OrderId);

                        using (SqlDataReader reader =await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {

                                // The record was found

                                orderDto.CustomerId = (int)reader["CustomerId"];
                                orderDto.OrderDate = (DateTime)reader["OrderDate"];
                                orderDto.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                                orderDto.Status = Convert.ToByte(reader["Status"]);
                                reader.Close();
                            }
                            else
                            {
                                // The record was not found
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


            return orderDto;
        }
      
        public static async Task<int> AddNewOrderAsync(int CustomerId, DateTime OrderDate, decimal TotalAmount, byte Status)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {

                    await connection.OpenAsync();

                    string query = @"INSERT INTO Orders 
                            (CustomerId,OrderDate,TotalAmount,Status)
                            VALUES 
                            (@CustomerId,@OrderDate,@TotalAmount,@Status);
                             SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {


                        command.Parameters.AddWithValue("@CustomerId", CustomerId);
                        command.Parameters.AddWithValue("@OrderDate", OrderDate);
                        command.Parameters.AddWithValue("@TotalAmount", TotalAmount);
                        command.Parameters.AddWithValue("@Status", Status);

                        object result = command.ExecuteScalar();  // to get the OrderId just added
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


        public static  async Task <bool> UpdateOrderAsync(int OrderId, int CustomerId, DateTime OrderDate, decimal TotalAmount, byte Status)
        {

            int rowsAffected = 0;

            try
            {

                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {

                    await connection.OpenAsync();

                    string query = @"Update Orders  
                            set CustomerId = @CustomerId, 
                                OrderDate = @OrderDate, 
                                TotalAmount = @TotalAmount, 
                                Status = @Status
                                
                                where OrderId = @OrderId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@OrderId", OrderId);
                        command.Parameters.AddWithValue("@CustomerId", CustomerId);
                        command.Parameters.AddWithValue("@OrderDate", OrderDate);
                        command.Parameters.AddWithValue("@TotalAmount", TotalAmount);
                        command.Parameters.AddWithValue("@Status", Status);
                        
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


        public static async Task<List<clsOrder>> GetAllOrdersAsync()
        {
            List<clsOrder> orders = new List<clsOrder>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    
                    await connection.OpenAsync();
                    
                    string query = "SELECT * FROM Orders"; //  OrdersFullData

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {                        
                        using (SqlDataReader reader =await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                clsOrder order = new clsOrder
                                {
                                    OrderId = reader.GetInt32(reader.GetOrdinal("OrderId")),
                                    CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                    OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                                    TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                                    Status = reader.GetByte(reader.GetOrdinal("Status"))
                                };

                                orders.Add(order);

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
                return null;
            }
            
            return orders;

        }
       
        public static async Task<List<clsOrder>> GetCustomerOrdersAsync(int CustomerId)
        {            
            List<clsOrder> orders = new List<clsOrder>();
            string query = "SELECT * FROM Orders where CustomerId = @CustomerId"; //  OrdersFullData
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {

                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@CustomerId", CustomerId);

                        using (SqlDataReader reader =await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                clsOrder order = new clsOrder
                                {
                                    OrderId = reader.GetInt32(reader.GetOrdinal("OrderId")),
                                    CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                    OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                                    TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                                    Status = reader.GetByte(reader.GetOrdinal("Status"))
                                };

                                orders.Add(order);

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
                return null;
            }
            
            return orders;

        }

        public static  async Task <bool> DeleteOrderAsync(int OrderId)
        {
            int RowAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "DELETE FROM Orders WHERE OrderId = @OrderId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrderId", OrderId);

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

        public static  async Task <bool> IsOrderExistsAsync(int OrderId)
        {
            bool IsFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "SELECT FOUND = 1 FROM Orders WHERE OrderId = @OrderId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrderId", OrderId);

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
