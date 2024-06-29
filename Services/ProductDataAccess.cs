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
    public static class ProductDataAccess
    {

        public static async Task<ProductDto> GetProductByIDAsync(int ProductId)
        {

            ProductDto dto = new ProductDto(); 
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                   await connection.OpenAsync();

                    string query = "SELECT * FROM ProductCatalog WHERE ProductId = @ProductId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ProductId", ProductId);

                        using (SqlDataReader  reader =await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {

                                // The record was found
                                dto.ProductName = (string)reader["ProductName"];
                                dto.Description = (string)reader["Description"];
                                dto.Price = Convert.ToDecimal(reader["Price"]);
                                dto.QuantityInStock = (int)reader["QuantityInStock"];
                                dto.CategoryId = (int)reader["CategoryId"];
                                dto.LikesCount = (int)reader["LikesCount"];
                                dto.FavoritesCount = (int)reader["FavoritesCount"];

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


            return dto;
        }

        public static async Task<ProductFullInfoDto> GetFullProductInfoByIDAsync(int ProductId)
        {

            ProductFullInfoDto dto = new ProductFullInfoDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "SELECT * FROM ProductFullInfo WHERE ProductId = @ProductId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ProductId", ProductId);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {

                                // The record was found
                                dto.ProductId = (int)reader["ProductId"];
                                dto.ProductName = (string)reader["ProductName"];
                                dto.Description = (string)reader["Description"];
                                dto.Price = Convert.ToDecimal(reader["Price"]);
                                dto.QuantityInStock = (int)reader["QuantityInStock"];
                                dto.CategoryId = (int)reader["CategoryId"];
                                dto.CategoryName = (string)reader["CategoryName"];
                                dto.LikesCount = (int)reader["LikesCount"];
                                dto.FavoritesCount = (int)reader["FavoritesCount"];
                                dto.ImageURL = (string)reader["ImageURL"];
                                dto.ImageOrder = Convert.ToByte(reader["ImageOrder"]);
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


            return dto;
        }



        public static async Task<int> AddNewProductAsync(string ProductName,string Description,decimal Price,int QuantityInStock,int CategoryId ,int LikesCount,int FavoritesCount)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {

                    await connection.OpenAsync();

                    string query = @"INSERT INTO ProductCatalog 
                            (ProductName,Description,Price,QuantityInStock,CategoryId,LikesCount,FavoritesCount)
                            VALUES 
                            (@ProductName,@Description,@Price,@QuantityInStock,@CategoryId,@LikesCount,@FavoritesCount);
                             SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {


                        command.Parameters.AddWithValue("@ProductName", ProductName);
                        command.Parameters.AddWithValue("@Description", Description);
                        command.Parameters.AddWithValue("@Price", Price);
                        command.Parameters.AddWithValue("@QuantityInStock", QuantityInStock);
                        command.Parameters.AddWithValue("@CategoryId", CategoryId);
                        command.Parameters.AddWithValue("@LikesCount", LikesCount);
                        command.Parameters.AddWithValue("@FavoritesCount", FavoritesCount);

                        object result = command.ExecuteScalar();  // to get the CategoryId just added
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


        public static async Task<bool> UpdateProductAsync(int ProductId, string ProductName,string Description,decimal Price,int QuantityInStock,int CategoryId ,int LikesCount,int FavoritesCount)
        {

            int rowsAffected = 0;

            try
            {

                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {

                    await connection.OpenAsync();

                    string query = @"Update ProductCatalog  
                            set ProductName = @ProductName, 
                                Description = @Description, 
                                Price = @Price, 
                                QuantityInStock = @QuantityInStock, 
                                CategoryId = @CategoryId, 
                                LikesCount = @LikesCount, 
                                FavoritesCount = @FavoritesCount 
                                
                                where ProductId = @ProductId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ProductId", ProductId);
                        command.Parameters.AddWithValue("@ProductName", ProductName);
                        command.Parameters.AddWithValue("@Description", Description);
                        command.Parameters.AddWithValue("@Price", Price);
                        command.Parameters.AddWithValue("@QuantityInStock", QuantityInStock);
                        command.Parameters.AddWithValue("@CategoryId", CategoryId);
                        command.Parameters.AddWithValue("@LikesCount", LikesCount);
                        command.Parameters.AddWithValue("@FavoritesCount", FavoritesCount);

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

        public static async Task<List<ProductFullInfoDto>> GetProductsInCategoryByNameAsync(string term)
        {

            List<ProductFullInfoDto> Products = new List<ProductFullInfoDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    string query = @"SELECT * FROM ProductFullInfo WHERE ProductName LIKE @StartWith or ProductName LIKE @EndWirh or ProductName LIKE @Contain"; //  ProductFullData

                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ProductName", term);
                        command.Parameters.AddWithValue("@StartWith", $"%{term}");
                        command.Parameters.AddWithValue("@EndWirh", $"{term}%");
                        command.Parameters.AddWithValue("@Contain", $"%{term}%");

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                ProductFullInfoDto Product = new ProductFullInfoDto
                                {
                                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    QuantityInStock = reader.GetInt32(reader.GetOrdinal("QuantityInStock")),
                                    CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                                    LikesCount = reader.GetInt32(reader.GetOrdinal("LikesCount")),
                                    FavoritesCount = reader.GetInt32(reader.GetOrdinal("FavoritesCount")),
                                    ImageURL = reader.GetString(reader.GetOrdinal("ImageURL")),
                                    ImageOrder = reader.GetByte(reader.GetOrdinal("ImageOrder"))

                                };

                                Products.Add(Product);

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

            return Products;

        }

        public static async Task<List<ProductFullInfoDto>> GetProductsInCategoryByNameAsync(int CategoryId,string term)
        {

            List<ProductFullInfoDto> Products = new List<ProductFullInfoDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    string query = @"SELECT * FROM ProductFullInfo WHERE CategoryId = @CategoryId and (ProductName LIKE @StartWith or ProductName LIKE @EndWirh or ProductName LIKE @Contain) "; //  ProductFullData

                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@CategoryId", CategoryId);
                        command.Parameters.AddWithValue("@ProductName", term);
                        command.Parameters.AddWithValue("@StartWith", $"%{term}");
                        command.Parameters.AddWithValue("@EndWirh", $"{term}%");
                        command.Parameters.AddWithValue("@Contain", $"%{term}%");

                        using (SqlDataReader  reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                ProductFullInfoDto Product = new ProductFullInfoDto
                                {
                                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    QuantityInStock = reader.GetInt32(reader.GetOrdinal("QuantityInStock")),
                                    CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                                    LikesCount = reader.GetInt32(reader.GetOrdinal("LikesCount")),
                                    FavoritesCount = reader.GetInt32(reader.GetOrdinal("FavoritesCount")),
                                    ImageURL = reader.GetString(reader.GetOrdinal("ImageURL")),
                                    ImageOrder = reader.GetByte(reader.GetOrdinal("ImageOrder"))

                                };

                                Products.Add(Product);

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

            return Products;

        }
       
        public static async Task<List<ProductFullInfoDto>> GetSomeProductsInCategoryAsync(int CategoryId, int Count)
        {

            List<ProductFullInfoDto> products = new List<ProductFullInfoDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    string query = $"SELECT Top {Count} * FROM ProductFullInfo WHERE CategoryId = @CategoryId"; //  ProductFullData

                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@CategoryId", CategoryId);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                ProductFullInfoDto Product = new ProductFullInfoDto
                                {
                                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    QuantityInStock = reader.GetInt32(reader.GetOrdinal("QuantityInStock")),
                                    CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                                    LikesCount = reader.GetInt32(reader.GetOrdinal("LikesCount")),
                                    FavoritesCount = reader.GetInt32(reader.GetOrdinal("FavoritesCount")),
                                    ImageURL = reader.GetString(reader.GetOrdinal("ImageURL")),
                                    ImageOrder = reader.GetByte(reader.GetOrdinal("ImageOrder"))

                                };

                                products.Add(Product);

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

            return products;

        }
        public static async Task<List<ProductFullInfoDto>> GetSomeProductsInCategoryAsync(int CategoryId,int ExcludedProduct, int Count)
        {

            List<ProductFullInfoDto> products = new List<ProductFullInfoDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    string query = $"SELECT Top {Count} * FROM ProductFullInfo WHERE CategoryId = @CategoryId and ProductId != @ExcludedProduct ORDER BY NEWID()"; //  ProductFullData

                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@CategoryId", CategoryId);
                        command.Parameters.AddWithValue("@ExcludedProduct", ExcludedProduct);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                ProductFullInfoDto Product = new ProductFullInfoDto
                                {
                                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    QuantityInStock = reader.GetInt32(reader.GetOrdinal("QuantityInStock")),
                                    CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                                    LikesCount = reader.GetInt32(reader.GetOrdinal("LikesCount")),
                                    FavoritesCount = reader.GetInt32(reader.GetOrdinal("FavoritesCount")),
                                    ImageURL = reader.GetString(reader.GetOrdinal("ImageURL")),
                                    ImageOrder = reader.GetByte(reader.GetOrdinal("ImageOrder"))

                                };

                                products.Add(Product);

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

            return products;

        }
        
        public static async Task<List<ProductFullInfoDto>> GetAllProductsAsync()
        {

            List<ProductFullInfoDto> products = new List<ProductFullInfoDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    string query = "SELECT * FROM ProductFullInfo"; //  ProductFullData

                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                ProductFullInfoDto Product = new ProductFullInfoDto
                                {
                                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    QuantityInStock = reader.GetInt32(reader.GetOrdinal("QuantityInStock")),
                                    CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                                    LikesCount = reader.GetInt32(reader.GetOrdinal("LikesCount")),
                                    FavoritesCount = reader.GetInt32(reader.GetOrdinal("FavoritesCount")),
                                    ImageURL = reader.GetString(reader.GetOrdinal("ImageURL")),
                                    ImageOrder = reader.GetByte(reader.GetOrdinal("ImageOrder"))

                                };

                                products.Add(Product);

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

            return products;

        }
        public static async Task<List<ProductFullInfoDto>> GetProductsInCategoryAsync(int CategoryId)
        {

            List<ProductFullInfoDto> products = new List<ProductFullInfoDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    string query = "SELECT * FROM ProductFullInfo WHERE CategoryId = @CategoryId "; //  ProductFullData

                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CategoryId", CategoryId);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                ProductFullInfoDto Product = new ProductFullInfoDto
                                {
                                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    QuantityInStock = reader.GetInt32(reader.GetOrdinal("QuantityInStock")),
                                    CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                                    LikesCount = reader.GetInt32(reader.GetOrdinal("LikesCount")),
                                    FavoritesCount = reader.GetInt32(reader.GetOrdinal("FavoritesCount")),
                                    ImageURL = reader.GetString(reader.GetOrdinal("ImageURL")),
                                    ImageOrder = reader.GetByte(reader.GetOrdinal("ImageOrder"))

                                };

                                products.Add(Product);

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

            return products;

        }
        public static List<ProductFullInfoDto> GetProductsInCategory(int CategoryId)
        {

            List<ProductFullInfoDto> Products = new List<ProductFullInfoDto>(); 

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    string query = @"SELECT * FROM ProductFullInfo WHERE CategoryId = @CategoryId"; //  ProductFullData

                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@CategoryId", CategoryId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductFullInfoDto Product = new ProductFullInfoDto
                                {
                                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    QuantityInStock = reader.GetInt32(reader.GetOrdinal("QuantityInStock")),
                                    CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                                    LikesCount = reader.GetInt32(reader.GetOrdinal("LikesCount")),
                                    FavoritesCount = reader.GetInt32(reader.GetOrdinal("FavoritesCount")),
                                    ImageURL = reader.GetString(reader.GetOrdinal("ImageURL")),
                                    ImageOrder = reader.GetByte(reader.GetOrdinal("ImageOrder"))

                                };

                                Products.Add(Product);

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
            return Products;

        }

        public static async Task<List<ProductFullInfoDto>> GetProductsInCategoryInRangeAsync(int CategoryId,int Min,int Max)
        {

            List<ProductFullInfoDto> products = new List<ProductFullInfoDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    string query = "SELECT * FROM ProductFullInfo WHERE CategoryId = @CategoryId and (Price between @Min and @Max) "; //  ProductFullData

                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@CategoryId", CategoryId);
                        command.Parameters.AddWithValue("@Min", Min);
                        command.Parameters.AddWithValue("@Max", Max);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                ProductFullInfoDto Product = new ProductFullInfoDto
                                {
                                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    QuantityInStock = reader.GetInt32(reader.GetOrdinal("QuantityInStock")),
                                    CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                                    LikesCount = reader.GetInt32(reader.GetOrdinal("LikesCount")),
                                    FavoritesCount = reader.GetInt32(reader.GetOrdinal("FavoritesCount")),
                                    ImageURL = reader.GetString(reader.GetOrdinal("ImageURL")),
                                    ImageOrder = reader.GetByte(reader.GetOrdinal("ImageOrder"))

                                };

                                products.Add(Product);

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

            return products;

        }

        public static async Task<bool> DeleteProductAsync(int ProductId)
        {
            int RowAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "DELETE FROM ProductCatalog WHERE ProductId = @ProductId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductId", ProductId);

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

        public static async Task<int> ProductsCountAsync()
        {
            int Count = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "SELECT COUNT(*) FROM ProductCatalog";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        Count = (int)await command.ExecuteScalarAsync();
                    }

                    connection.Close();
                }

            }
            catch (Exception Ex)
            {
            }

            return Count;
        }

        public static async Task<int> ProductsCountAsync(int CategoryId)
        {
            int Count = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "SELECT COUNT(*) FROM ProductCatalog WHERE CategoryId = @CategoryId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CategoryId", CategoryId);
                       
                        Count = (int) await command.ExecuteScalarAsync();
                    }

                    connection.Close();
                }

            }
            catch (Exception Ex)
            {
            }

            return Count;
        }

        public static async Task<bool> IsProductExistsAsync(int ProductId)
        {
            bool IsFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(ConfigurationHelper.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    string query = "SELECT FOUND = 1 FROM ProductCatalog WHERE ProductId = @ProductId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductId", ProductId);

                        using (SqlDataReader  reader = await command.ExecuteReaderAsync())
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
