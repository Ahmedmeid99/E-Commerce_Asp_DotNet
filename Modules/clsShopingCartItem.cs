using E_Commerce_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_BusniessLayer
{
    public class clsShopingCartItem
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ShopingCartItemId { get; set; }
        public int ShopingCartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }

        public clsShopingCartItem()
        {
            this.ShopingCartItemId = -1;
            this.ShopingCartId = -1;
            this.ProductId = -1;
            this.Quantity = -1;
            this.Price = -1;
            this.TotalPrice = -1;

            Mode = enMode.AddNew;
        } 
        private clsShopingCartItem(int ShopingCartItemId, int ShopingCartId,int ProductId,int Quantity,decimal Price,decimal TotalPrice)
        {
            this.ShopingCartItemId = ShopingCartItemId;
            this.ShopingCartId = ShopingCartId;
            this.ProductId = ProductId;
            this.Quantity = Quantity;
            this.Price = Price;
            this.TotalPrice = TotalPrice;

            Mode = enMode.Update;
        }

        private async Task<bool> _AddNewShopingCartItemAsync()
        {
            // add this object to database
            // in AddNew Mode 

            clsProduct Product = await clsProduct.FindAsync(this.ProductId);
            if(Product == null)
                return false;

            this.Price = Product.Price;
            this.TotalPrice = this.Quantity * this.Price;
            this.ShopingCartItemId = await ShopingCartItemDataAccess.AddNewShopingCartItemAsync(this.ShopingCartId, this.ProductId, this.Quantity, this.Price, this.TotalPrice);

            return (this.ShopingCartItemId != -1);

        }
        private async Task<bool> _UpdateShopingCartItemAsync()
        {
            // add this object to database
            // in Update Mode

            clsProduct Product = await clsProduct.FindAsync(this.ProductId);
            if (Product == null)
                return false;

            this.Price = Product.Price;
            this.TotalPrice = this.Quantity * this.Price;
            return await ShopingCartItemDataAccess.UpdateShopingCartItemAsync(this.ShopingCartItemId,this.ShopingCartId, this.ProductId, this.Quantity, this.Price, this.TotalPrice);

        }

        // find by ID , NationalNo
        public static async Task<clsShopingCartItem> FindAsync(int ShopingCartItemId)
        {
            ShopingCartItemDto shopingCartItemDto = await ShopingCartItemDataAccess.GetShopingCartItemByIDAsync(ShopingCartItemId);
            if (shopingCartItemDto != null)
            {
                return new clsShopingCartItem(ShopingCartItemId, shopingCartItemDto.ShopingCartId, shopingCartItemDto.ProductId, shopingCartItemDto.Quantity, shopingCartItemDto.Price, shopingCartItemDto.TotalPrice);
            }
            else
                return null;
        }


        public async Task<bool> SaveAsync()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewShopingCartItemAsync())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return await this._UpdateShopingCartItemAsync();

            }

            return false;
        }

        public static  async Task <bool> DeleteAsync(int ID)
        {
            return await ShopingCartItemDataAccess.DeleteShopingCartItemAsync(ID);
        }               
        
        public static  async Task <bool> ClearCartItemsAsync(int ShopingCartId)
        {
            return await ShopingCartItemDataAccess.ClearShopingCartItemsAsync(ShopingCartId);
        }

        public static async Task<List<clsShopingCartItem>> GetAllAsync(int ShopingCartId)
        {
            return await ShopingCartItemDataAccess.GetAllShopingCartItemsAsync(ShopingCartId);
        }


        public static  async Task <bool> IsExistsAsync(int ID)
        {
            return await ShopingCartItemDataAccess.IsShopingCartItemExistsAsync(ID);
        }


    }
}
