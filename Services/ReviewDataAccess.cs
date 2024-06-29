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
    public static class ReviewDataAccess
    {
        public static  async Task <ReviewDto> GetReviewByIDAsync(int ReviewId)
        {

            ReviewDto reviewDto = new ReviewDto();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "SELECT * FROM Reviews WHERE ReviewId = @ReviewId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ReviewId", ReviewId);

                        using (SqlDataReader reader =await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {

                                // The record was found

                                reviewDto.CustomerId = (int)reader["CustomerId"];
                                reviewDto.ProductId = (int)reader["ProductId"];
                                reviewDto.ReviewText = (string)reader["ReviewText"];
                                reviewDto.Rating = Convert.ToDecimal(reader["Rating"]);
                                reviewDto.ReviewDate = (DateTime)reader["ReviewDate"];
                                
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


            return reviewDto;
        }

        public static async Task<int> AddNewReviewAsync(int CustomerId, int ProductId, string ReviewText, decimal Rating, DateTime ReviewDate)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {

                    await connection.OpenAsync();
                                                               
                    string query = @"INSERT INTO Reviews 
                            (CustomerId,ProductId,ReviewText,Rating,ReviewDate)
                            VALUES 
                            (@CustomerId,@ProductId,@ReviewText,@Rating,@ReviewDate);
                             SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@CustomerId", CustomerId);
                        command.Parameters.AddWithValue("@ProductId", ProductId);
                        command.Parameters.AddWithValue("@ReviewText", ReviewText);
                        command.Parameters.AddWithValue("@Rating", Rating);
                        command.Parameters.AddWithValue("@ReviewDate", ReviewDate);


                        object result = await command.ExecuteScalarAsync();  // to get the ReviewId just added
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

        public static  async Task <bool> UpdateReviewAsync(int ReviewId, int CustomerId, int ProductId, string ReviewText, decimal Rating, DateTime ReviewDate)
        {

            int rowsAffected = 0;

            try
            {

                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {

                    await connection.OpenAsync();

                    string query = @"Update Reviews  
                            set CustomerId = @CustomerId, 
                                ProductId = @ProductId, 
                                ReviewText = @ReviewText,
                                Rating = @Rating ,
                                ReviewDate = @ReviewDate 
                                
                                where ReviewId = @ReviewId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ReviewId", ReviewId);
                        command.Parameters.AddWithValue("@CustomerId", CustomerId);
                        command.Parameters.AddWithValue("@ProductId", ProductId);
                        command.Parameters.AddWithValue("@ReviewText", ReviewText);
                        command.Parameters.AddWithValue("@Rating", Rating);
                        command.Parameters.AddWithValue("@ReviewDate", ReviewDate);

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

        public static async Task<List<ReviewFullInfoDto>> GetReviewsOfCustomerAsync(int CustomerId)
        {

            List<ReviewFullInfoDto> reviews = new List<ReviewFullInfoDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    string query = @"SELECT Reviews.ReviewId, Customers.CustomerId,Customers.UserName as CustomerName, Reviews.ProductId, Reviews.ReviewText, Reviews.Rating, Reviews.ReviewDate " +
                                    "FROM Reviews INNER JOIN " +
                                    "Customers ON Reviews.CustomerId = Customers.CustomerId WHERE Customers.CustomerId = @CustomerId order by Reviews.ReviewDate desc"; 

                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@CustomerId", CustomerId);

                        using (SqlDataReader reader =await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                ReviewFullInfoDto review = new ReviewFullInfoDto
                                {
                                    ReviewId = reader.GetInt32(reader.GetOrdinal("ReviewId")),
                                    CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                    CustomerName = reader.GetString(reader.GetOrdinal("CustomerName")),
                                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    ReviewText = reader.GetString(reader.GetOrdinal("ReviewText")),
                                    Rating = reader.GetDecimal(reader.GetOrdinal("Rating")),
                                    ReviewDate = reader.GetDateTime(reader.GetOrdinal("ReviewDate"))

                                };

                                reviews.Add(review);

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

            return reviews;

        }
       
        public static async Task<List<ReviewFullInfoDto>> GetReviewsOfProductAsync(int ProductId)
        {

            List<ReviewFullInfoDto> reviews = new List<ReviewFullInfoDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    string query = @"SELECT Reviews.ReviewId, Customers.CustomerId,Customers.UserName as CustomerName, Reviews.ProductId, Reviews.ReviewText, Reviews.Rating, Reviews.ReviewDate "+
                                    "FROM Reviews INNER JOIN "+
                                    "Customers ON Reviews.CustomerId = Customers.CustomerId WHERE Reviews.ProductId = @ProductId order by Reviews.ReviewDate desc"; 

                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ProductId", ProductId);

                        using (SqlDataReader reader =await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                ReviewFullInfoDto review = new ReviewFullInfoDto
                                {
                                    ReviewId = reader.GetInt32(reader.GetOrdinal("ReviewId")),
                                    CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                    CustomerName = reader.GetString(reader.GetOrdinal("CustomerName")),
                                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    ReviewText = reader.GetString(reader.GetOrdinal("ReviewText")),
                                    Rating = reader.GetDecimal(reader.GetOrdinal("Rating")),
                                    ReviewDate = reader.GetDateTime(reader.GetOrdinal("ReviewDate"))

                                };

                                reviews.Add(review);

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

            return reviews;

        }

        public static  async Task <bool> DeleteReviewAsync(int ReviewId)
        {
            int RowAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "DELETE FROM Reviews WHERE ReviewId = @ReviewId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReviewId", ReviewId);

                        RowAffected = await command.ExecuteNonQueryAsync();
                    }

                }
            }
            catch (Exception Ex)
            {
                // Catch Error
            }

            return (RowAffected > 0);
        }

        public static  async Task <bool> IsReviewExistsAsync(int ReviewId)
        {
            bool IsFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "SELECT FOUND = 1 FROM Reviews WHERE ReviewId = @ReviewId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReviewId", ReviewId);

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
