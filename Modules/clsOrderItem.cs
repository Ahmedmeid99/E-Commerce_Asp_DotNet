using E_Commerce_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_BusniessLayer
{
    public class clsOrderItem
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }


        public clsOrderItem()
        {
            this.OrderItemId = -1;
            this.OrderId = -1;
            this.ProductId = -1;
            this.Quantity = -1;
            this.Price = 0;
            this.TotalPrice = 0;  

            Mode = enMode.AddNew;

        }
        private clsOrderItem(int OrderItemId, int OrderId,int ProductId,int Quantity,decimal Price, decimal TotalPrice)
        {
            this.OrderItemId = OrderItemId;
            this.OrderId = OrderId;
            this.ProductId = ProductId;
            this.Quantity = Quantity;
            this.Price = Price;
            this.TotalPrice = TotalPrice;

            Mode = enMode.Update;
        }

        private async Task<bool> _AddNewOrderItemAsync()
        {
            // add this object to database
            // in AddNew Mode 

            clsProduct Product = await clsProduct.FindAsync(this.ProductId);
            this.Price = Product.Price;

            decimal TotalPrice = this.Quantity * this.Price;
            this.OrderItemId =await OrderItemDataAccess.AddNewOrderItemAsync(this.OrderId, this.ProductId, this.Quantity, this.Price, TotalPrice);

            return (this.OrderItemId != -1);

        }
     
        private async Task<bool> _UpdateOrderItemAsync()
        {
            // add this object to database
            // in Update Mode

            clsProduct Product = await clsProduct.FindAsync(this.ProductId);
            this.Price = Product.Price;

            decimal TotalPrice = this.Quantity * this.Price;
            return await OrderItemDataAccess.UpdateOrderItemAsync(this.OrderItemId, this.OrderId, this.ProductId, this.Quantity, this.Price, TotalPrice);

        }
        // find by ID , NationalNo
        public static async Task<clsOrderItem> FindAsync(int OrderItemId)
        {
            OrderItemDto orderItemDto = await OrderItemDataAccess.GetOrderItemByIDAsync(OrderItemId);
            if (orderItemDto != null)
            {
                return new clsOrderItem(OrderItemId, orderItemDto.OrderId, orderItemDto.ProductId, orderItemDto.Quantity, orderItemDto.Price, orderItemDto.TotalPrice);
            }
            else
                return null;
        }


        public async Task<bool> SaveAsync()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewOrderItemAsync())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return await this._UpdateOrderItemAsync();

            }

            return false;
        }

        public static  async Task <bool> DeleteAsync(int ID)
        {
            return await OrderItemDataAccess.DeleteOrderItemAsync(ID);
        }

        public static async Task<List<clsOrderItem>> GetAllOrdersAsync()
        {
            return await OrderItemDataAccess.GetOrderItemAsync();
        }      

        public static async Task<List<clsOrderItem>> GetAllOrderItemsAsync(int OrderId)
        {
            return await OrderItemDataAccess.GetOrderItemsOfOrderAsync(OrderId);
        }


        public static  async Task <bool> IsExistsAsync(int ID)
        {
            return await OrderItemDataAccess.IsOrderItemExistsAsync(ID);
        }


    }

}
