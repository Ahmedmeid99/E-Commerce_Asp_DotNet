using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce_DataAccessLayer;

namespace E_Commerce_BusniessLayer
{
    public class clsOrder
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public byte Status { get; set; }   // completed / Cancled / in the way


        public clsOrder()
        {
            this.OrderId = -1;
            this.CustomerId = -1;
            this.OrderDate = DateTime.Now;
            this.TotalAmount = 0; // to add to it orderitems price
            this.Status = 0;

            this.Mode = enMode.AddNew;
        }
        private clsOrder(int OrderId, int CustomerId ,DateTime OrderDate,decimal TotalAmount,byte Status)
        {
            this.OrderId = OrderId;
            this.CustomerId = CustomerId;
            this.OrderDate = OrderDate;
            this.TotalAmount = TotalAmount;
            this.Status = Status;

            this.Mode = enMode.Update;
        }

        private async Task<bool> _AddNewOrderAsync()
        {
            // add this object to database
            // in AddNew Mode 

            this.OrderId = await OrderDataAccess.AddNewOrderAsync(this.CustomerId, this.OrderDate, this.TotalAmount, this.Status);

            return (this.OrderId != -1);

        }
        private async Task<bool> _UpdateOrderAsync()
        {
            // add this object to database
            // in Update Mode

            return await OrderDataAccess.UpdateOrderAsync(this.OrderId, this.CustomerId, this.OrderDate, this.TotalAmount, this.Status);

        }
        // find by ID , NationalNo
        public static async Task<clsOrder> FindAsync(int OrderId)
        {
            OrderDto orderDto = await OrderDataAccess.GetOrderByIDAsync(OrderId);
            if (orderDto != null)
            {
                return new clsOrder(OrderId, orderDto.CustomerId, orderDto.OrderDate,orderDto.TotalAmount, orderDto.Status);
            }
            else
                return null;
        }
        

        public async Task<bool> SaveAsync()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewOrderAsync())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return await this._UpdateOrderAsync();

            }

            return false;
        }

        public static  async Task <bool> DeleteAsync(int ID)
        {
            return await OrderDataAccess.DeleteOrderAsync(ID);
        }

        public static async Task<List<clsOrder>> GetOrdersAsync()
        {
            return await OrderDataAccess.GetAllOrdersAsync();
        }      

        public static async Task<List<clsOrder>> GetCustomerOrdersAsync(int CustomerId)
        {
            return await OrderDataAccess.GetCustomerOrdersAsync(CustomerId);
        }


        public static  async Task <bool> IsExistsAsync(int ID)
        {
            return await OrderDataAccess.IsOrderExistsAsync(ID);
        }


    }

}
