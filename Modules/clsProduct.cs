using E_Commerce_API_Layer.Dtos;
using E_Commerce_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_BusniessLayer
{
    public class clsProduct
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public int CategoryId { get; set; }
        public int LikesCount { get; set; }
        public int FavoritesCount { get; set; }

        public clsProduct( )
        {
            this.ProductId = -1;
            this.ProductName = "";
            this.Description = "";
            this.Price = -1;
            this.QuantityInStock = -1;
            this.CategoryId = -1;
            this.LikesCount = -1;
            this.FavoritesCount = -1;

            Mode = enMode.AddNew;
        }
        private clsProduct(int ProductId, string ProductName,string Description,decimal Price,int QuantityInStock,int CategoryId,int LikesCount, int FavoritesCount)
        {
            this.ProductId = ProductId;
            this.ProductName =ProductName;
            this.Description =Description;
            this.Price =Price;
            this.QuantityInStock =QuantityInStock;
            this.CategoryId = CategoryId;
            this.LikesCount = LikesCount;
            this.FavoritesCount = FavoritesCount;

            Mode = enMode.Update;
        }

        private async Task<bool> _AddNewProductAsync()
        {
            // add this object to database
            // in AddNew Mode 

            this.ProductId =await ProductDataAccess.AddNewProductAsync(this.ProductName, this.Description, this.Price, this.QuantityInStock, this.CategoryId, this.LikesCount, this.FavoritesCount);

            return (this.ProductId != -1);

        }
        private async Task<bool> _UpdateProductAsync()
        {
            // add this object to database
            // in Update Mode

            return await ProductDataAccess.UpdateProductAsync(this.ProductId, this.ProductName, this.Description, this.Price, this.QuantityInStock, this.CategoryId, this.LikesCount, this.FavoritesCount);

        }
        // find by ID , NationalNo
        public static async Task<clsProduct> FindAsync(int ProductId)
        {
            ProductDto productdto =await ProductDataAccess.GetProductByIDAsync(ProductId);

            if(productdto != null)
                return new clsProduct(ProductId, productdto.ProductName, productdto.Description, productdto.Price, productdto.QuantityInStock, productdto.CategoryId, productdto.LikesCount, productdto.FavoritesCount);
            else
                return null;
        }  
        public static async Task<ProductFullInfoDto> FindFullInfoAsync(int ProductId)
        {
            ProductFullInfoDto productdto =await ProductDataAccess.GetFullProductInfoByIDAsync(ProductId);

            if (productdto != null)
                return productdto;
            else
                return null;
        }



        public async Task<bool> SaveAsync()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewProductAsync())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return await this._UpdateProductAsync();

            }

            return false;
        }

        public static async Task<bool> DeleteAsync(int ID)
        {
            return await ProductDataAccess.DeleteProductAsync(ID);
        }

        public static async Task<List<ProductFullInfoDto>> GetAllProductsAsync()
        {
            return await ProductDataAccess.GetAllProductsAsync();
        }    

        public static async Task<List<ProductFullInfoDto>> GetProductsAsync(int CategoryId)
        {
            return await ProductDataAccess.GetProductsInCategoryAsync(CategoryId);
        } 
        
        
        public static async Task<List<ProductFullInfoDto>> GetTopAsync(int CategoryId,int Count)
        {
            return await ProductDataAccess.GetSomeProductsInCategoryAsync(CategoryId, Count);
        }   
        
        public static async Task<List<ProductFullInfoDto>> GetRelatedAsync(int CategoryId,int ExcludedProduct,int Count)
        {
            return await ProductDataAccess.GetSomeProductsInCategoryAsync(CategoryId, ExcludedProduct, Count);
        } 
        
        public static IEnumerable<ProductFullInfoDto> PaginateAsync(int CategoryId,int pageNumber,int ItemPerPage)
        {
            var products= ProductDataAccess.GetProductsInCategory(CategoryId);
            
            var productsInPage = products.Skip((pageNumber - 1) * ItemPerPage).Take(ItemPerPage);   

            return productsInPage;
        }  

        public static async Task<List<ProductFullInfoDto>> InRangeAsync(int CategoryId,int Min,int Max)
        {
            return await ProductDataAccess.GetProductsInCategoryInRangeAsync(CategoryId, Min, Max);
        }   
        
        public static async Task<List<ProductFullInfoDto>> SearchAsync(int CategoryId,string productName)
        {
            return await ProductDataAccess.GetProductsInCategoryByNameAsync(CategoryId, productName);
        }  
        public static async Task<List<ProductFullInfoDto>> SearchAsync(string productName)
        {
            return await ProductDataAccess.GetProductsInCategoryByNameAsync(productName);
        }


        public static async Task<bool> IsExistsAsync(int ID)
        {
            return await ProductDataAccess.IsProductExistsAsync(ID);
        }

        public static async Task<int> CountAsync()
        {
            return await ProductDataAccess.ProductsCountAsync();
        }

        public static async Task<int> CountAsync(int CategoryID)
        {
            return await ProductDataAccess.ProductsCountAsync(CategoryID);
        }


    }
}
