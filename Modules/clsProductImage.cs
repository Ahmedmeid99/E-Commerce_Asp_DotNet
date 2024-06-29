using E_Commerce_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_BusniessLayer
{
    
    public class clsProductImage
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ImageId { get; set; }
        public int ProductId { get; set; }
        public string ImageURL { get; set; }
        public byte ImageOrder { get; set; }


        public clsProductImage()
        {
            this.ImageId = -1;
            this.ProductId = -1;
            this.ImageURL = "";
            this.ImageOrder = 0;

            this.Mode = enMode.AddNew;
        }
        private clsProductImage(int ImageId, int ProductId,string ImageURL, byte ImageOrder)
        {
            this.ImageId = ImageId;
            this.ProductId = ProductId;
            this.ImageURL = ImageURL;
            this.ImageOrder = ImageOrder;

            this.Mode = enMode.Update;
        }

        private async Task<bool> _AddNewImageAsync()
        {
            // add this object to database
            // in AddNew Mode 

            this.ImageId = await ProductImageDataAccess.AddNewProductImageAsync(this.ProductId, this.ImageURL, this.ImageOrder);

            return (this.ImageId != -1);

        }
       
        private async Task<bool> _UpdateImageAsync()
        {
            // add this object to database
            // in Update Mode

            return await ProductImageDataAccess.UpdateProductImageAsync(this.ImageId, this.ProductId, this.ImageURL, this.ImageOrder);

        }
        // find by ID , NationalNo
        public static async Task<clsProductImage> FindAsync(int ImageId)
        {
                         
            ProductImageDto productImageDto =await ProductImageDataAccess.GetProductImageByIDAsync(ImageId); 

            if (productImageDto != null)
            {
                return new clsProductImage(ImageId, productImageDto.ProductId, productImageDto.ImageURL, productImageDto.ImageOrder);
            }
            else
                return null;
        }

        public async Task<bool> SaveAsync()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewImageAsync())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return await this._UpdateImageAsync();

            }

            return false;
        }

        public static  async Task <bool> DeleteAsync(int ID)
        {
            return await ProductImageDataAccess.DeleteProductImageAsync(ID);
        }

        public static async Task<List<clsProductImage>> GetAllProductImagesAsync(int productId)
        {
            return await ProductImageDataAccess.GetProductImagesOfProductAsync(productId);
        }


        public static  async Task <bool> IsExistsAsync(int ID)
        {
            return await ProductImageDataAccess.IsProductImageExistsAsync(ID);
        }


    }

}
