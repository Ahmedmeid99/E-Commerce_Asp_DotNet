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
    public class clsShopingCart
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ShopingCartId { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public clsShopingCart()
        {
            this.ShopingCartId = -1;
            this.CustomerId =-1;
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;

            Mode = enMode.AddNew;
        }  
        private clsShopingCart(int ShopingCartId, int CustomerId, DateTime CreatedAt, DateTime UpdatedAt)
        {
            this.ShopingCartId = ShopingCartId;
            this.CustomerId =CustomerId;
            this.CreatedAt =CreatedAt;
            this.UpdatedAt =UpdatedAt;

            Mode = enMode.Update;
        }

        private async Task<bool> _AddNewShopingCartAsync()
        {
            // add this object to database
            // in AddNew Mode 

            this.ShopingCartId = await ShopingCartDataAccess.AddNewShopingCartAsync(this.CustomerId, this.CreatedAt,this.UpdatedAt);

            return (this.ShopingCartId != -1);

        }

        private async Task<bool> _UpdateShopingCartAsync()
        {
            // add this object to database
            // in Update Mode

            return await ShopingCartDataAccess.UpdateShopingCartAsync(this.ShopingCartId,this.CustomerId, this.CreatedAt, this.UpdatedAt);

        }

        public static async Task <clsShopingCart> FindAsync(int ShopingCartId)
        {
            ShopingCartDto shopingCartDto =await ShopingCartDataAccess.GetShopingCartByIDAsync(ShopingCartId);
            if (shopingCartDto != null)
            {
                return new clsShopingCart(ShopingCartId, shopingCartDto.CustomerId, shopingCartDto.CreatedAt, shopingCartDto.UpdatedAt);
            }
            else
                return null;
        }

        public static async Task<clsShopingCart> FindByCustomerIDAsync(int CustomerID)
        {
            ShopingCartCustomerDto shopingCartDto = await ShopingCartDataAccess.GetShopingCartByCustomerIDAsync(CustomerID);
            if (shopingCartDto != null)
            {
                return new clsShopingCart(shopingCartDto.ShopingCartId, CustomerID, shopingCartDto.CreatedAt, shopingCartDto.UpdatedAt);
            }
            else
                return null;
        }

        public async Task<bool> SaveAsync()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewShopingCartAsync())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return await this._UpdateShopingCartAsync();

            }

            return false;
        }

        public static  async Task <bool> DeleteAsync(int ID)
        {
            return await ShopingCartDataAccess.DeleteShopingCartAsync(ID);
        }              
        
        public static async Task <bool> ClearAsync(int shopingCartId)
        {
            if (await clsShopingCartItem.ClearCartItemsAsync(shopingCartId))
            {
                if (!await clsShopingCart.DeleteAsync(shopingCartId))
                    return false;
                return true;
            }

            return false;
        }

        public static async Task<List<clsShopingCart>> GetAllShopingCartsOfCustomerAsync(int CustomerId)
        {
            return await ShopingCartDataAccess.GetShopingCartsOfCustomerAsync(CustomerId);
        }


        public static  async Task <bool> IsExistsAsync(int ID)
        {
            return await ShopingCartDataAccess.IsShopingCartExistsAsync(ID);
        }



    }
}
