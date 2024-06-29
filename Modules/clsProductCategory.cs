using E_Commerce_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_BusniessLayer
{
    public class clsProductCategory
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        

        public clsProductCategory()
        {
            this.CategoryId = -1;
            this.CategoryName = "";

            Mode = enMode.AddNew;
        }
      
        private clsProductCategory(int CategoryId, string CategoryName )
        {
            this.CategoryId = CategoryId;
            this.CategoryName = CategoryName;

            Mode = enMode.Update;
        }

        private async Task<bool> _AddNewCategoryAsync()
        {
            // add this object to database
            // in AddNew Mode 

            this.CategoryId = await ProductCategoryDataAccess.AddNewProductCategoryAsync(this.CategoryName);

            return (this.CategoryId != -1);

        }
       
        private async Task<bool> _UpdateCategoryAsync()
        {
            // add this object to database
            // in Update Mode

            return await ProductCategoryDataAccess.UpdateProductCategoryAsync(this.CategoryId, this.CategoryName);

        }

        public static async Task<clsProductCategory> FindAsync(int CategoryId)
        {
            ProductCategoryDto productCategoryDto =await ProductCategoryDataAccess.GetProductCategoryByIDAsync(CategoryId);

            if (productCategoryDto != null)
            {
                return new clsProductCategory(CategoryId, productCategoryDto.CategoryName);
            }
            else
                return null;
        }


        public async Task<bool> SaveAsync()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewCategoryAsync())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return await this._UpdateCategoryAsync();

            }

            return false;
        }

        public static  async Task <bool> DeleteAsync(int ID)
        {
            return await ProductCategoryDataAccess.DeleteProductCategoryAsync(ID);
        }

        public static async Task<List<clsProductCategory>> GetAllCategoriesAsync()
        {
            return await ProductCategoryDataAccess.GetAllProductCategoriesAsync();
        }


        public static  async Task <bool> IsExistsAsync(int ID)
        {
            return await ProductCategoryDataAccess.IsProductCategoryExistsAsync(ID);
        }




    }

}
