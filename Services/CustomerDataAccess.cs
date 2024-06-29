using System;
using System.Data;
using System.Data.SqlClient;
using E_Commerce_DataAccessLayer.Globals;
using E_Commerce_BusniessLayer;
using E_Commerce_API_Layer.Dtos;
using E_Commerce_BusniessLayer.Dtos;

namespace E_Commerce_DataAccessLayer
{
    public static class CustomerDataAccess
    {
        public static  async Task <CustomerDto> GetCustomerByIDAsync(int ID)
        {

            CustomerDto customerDto = new CustomerDto();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "SELECT * FROM Customers WHERE CustomerId = @CustomerId";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@CustomerId", ID);

                        using (SqlDataReader reader =await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {

                                // The record was found
                                customerDto.UserName = (string)reader["UserName"];
                                customerDto.Password = (string)reader["Password"];
                                customerDto.Email = (string)reader["Email"];
                                customerDto.Phone = (string)reader["Phone"];
                                customerDto.Address = (string)reader["Address"];
                                customerDto.DateOfBirth = (DateTime)reader["DateOfBirth"];
                                customerDto.CountryID = (int)reader["CountryID"];
                                customerDto.Gendor = (string)reader["Gendor"];

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
            

            return customerDto;
        }

        public static async Task<Customer2Dto> GetCustomerByUserName_PasswordAsync( string UserName, string Password)
        {
            Customer2Dto customer2Dto = new Customer2Dto();
            
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "SELECT * FROM Customers WHERE UserName = @UserName and Password = @Password";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@UserName", UserName);
                        command.Parameters.AddWithValue("@Password", Password);

                        using (SqlDataReader reader =await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {

                                // The record was found
                                customer2Dto.CustomerId = (int)reader["CustomerId"];
                                customer2Dto.Email = (string)reader["Email"];
                                customer2Dto.Phone = (string)reader["Phone"];
                                customer2Dto.Address = (string)reader["Address"];
                                customer2Dto.DateOfBirth = (DateTime)reader["DateOfBirth"];
                                customer2Dto.CountryID = (int)reader["CountryID"];
                                customer2Dto.Gendor = (string)reader["Gendor"];
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
            

            return customer2Dto;
        }

       
        public static async Task<int> AddNewCustomerAsync(string UserName, string Password,
               string Gendor, DateTime DateOfBirth, string Phone, string Email, string Address, int CountryId)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {

                    await connection.OpenAsync();

                    string query = @"INSERT INTO Customers 
                            (UserName,Password,Gendor,DateOfBirth,Phone,Email,Address,CountryId)
                            VALUES 
                            (@UserName,@Password, @Gendor,@DateOfBirth,@Phone,@Email,@Address,@CountryId);
                             SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {


                        command.Parameters.AddWithValue("@UserName", UserName);
                        command.Parameters.AddWithValue("@Password", Password);
                        command.Parameters.AddWithValue("@Gendor", Gendor);
                        command.Parameters.AddWithValue("@Email", Email);
                        command.Parameters.AddWithValue("@Phone", Phone);
                        command.Parameters.AddWithValue("@Address", Address);
                        command.Parameters.AddWithValue("@CountryId", CountryId);
                        command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);

                        object result = await command.ExecuteScalarAsync();  // to get the CustomerId just added
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


        public static  async Task <bool> UpdateCustomerAsync(int CustomerId, string UserName, string Password,
               string Gendor, DateTime DateOfBirth, string Phone, string Email, string Address, int CountryId)
        {

            int rowsAffected = 0;

            try
            {

                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {

                    await connection.OpenAsync();

                    string query = @"Update Customers  
                            set UserName = @UserName, 
                                Password = @Password, 
                                Gendor = @Gendor, 
                                Email = @Email, 
                                Phone = @Phone, 
                                Address = @Address, 
                                DateOfBirth = @DateOfBirth,
                                CountryId = @CountryId

                                where CustomerId = @CustomerId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@CustomerId", CustomerId);
                        command.Parameters.AddWithValue("@UserName", UserName);
                        command.Parameters.AddWithValue("@Password", Password);
                        command.Parameters.AddWithValue("@Gendor", Gendor);
                        command.Parameters.AddWithValue("@Email", Email);
                        command.Parameters.AddWithValue("@Phone", Phone);
                        command.Parameters.AddWithValue("@Address", Address);
                        command.Parameters.AddWithValue("@CountryId", CountryId);
                        command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);

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

        
        [Obsolete("dont use this method in E-commerce")]
        public static async Task<List<clsCustomer>> GetAllCustomersAsync()
        {
            List<clsCustomer> customers = new List<clsCustomer>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    string query = "SELECT * FROM Customers"; //  ProductFullData

                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {


                        using (SqlDataReader reader =await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                clsCustomer customer = new clsCustomer
                                {
                                    CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                                    UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                    Password = reader.GetString(reader.GetOrdinal("Password")),
                                    Gendor = reader.GetString(reader.GetOrdinal("Gendor")),
                                    DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                    Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    CountryID = reader.GetInt32(reader.GetOrdinal("CountryID"))
                                };

                                customers.Add(customer);

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

            return customers;
        }

        public static  async Task <bool> DeleteCustomerAsync(int CustomerId)
        {
            int RowAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "DELETE FROM Customers WHERE CustomerId = @CustomerId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerId", CustomerId);

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

        public static  async Task <bool> IsCustomerExistsAsync(int CustomerId)
        {
            bool IsFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "SELECT FOUND = 1 FROM Customers WHERE CustomerId = @CustomerId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerId", CustomerId);

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