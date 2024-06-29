using System.Data;
using E_Commerce_API_Layer.Dtos;
using E_Commerce_BusniessLayer.Dtos;
using E_Commerce_DataAccessLayer;

namespace E_Commerce_BusniessLayer
{
    public class clsCustomer
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int CustomerID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Gendor { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int CountryID { get; set; }



        // Default Constractor for AddNew Mode 
        public clsCustomer()
        {
            this.CustomerID = -1;
            this.UserName = "";
            this.Password = "";
            this.Gendor = "";
            this.DateOfBirth = DateTime.Now;
            this.Phone = "";
            this.Email = "";
            this.Address = "";
            this.CountryID = -1;

            Mode = enMode.AddNew;

        }

        // Private Constractor for Update Mode 
        private clsCustomer(int CustomerID, string UserName, string Password,
                                        string Gendor, DateTime DateOfBirth, string Phone, string Email, string Address
                                                            , int CountryID)
        {

            this.CustomerID = CustomerID;
            this.UserName = UserName;
            this.Password = Password;
            this.Gendor = Gendor;
            this.DateOfBirth = DateOfBirth;
            this.Phone = Phone;
            this.Email = Email;
            this.Address = Address;
            this.CountryID = CountryID;

            Mode = enMode.Update;

        }

        private async Task<bool> _AddNewCustomer()
        {
            // add this object to database
            // in AddNew Mode 
            string HashedPassword = Global.Utility.HashingPassword(this.Password);

            this.CustomerID =await CustomerDataAccess.AddNewCustomerAsync(this.UserName, HashedPassword,
                this.Gendor, this.DateOfBirth, this.Phone, this.Email, this.Address, this.CountryID);

            return (this.CustomerID != -1);

        }
        private async Task<bool> _UpdateCustomerAsync()
        {
            // add this object to database
            // in Update Mode
            string HashedPassword = Global.Utility.HashingPassword(this.Password);
            
            return await CustomerDataAccess.UpdateCustomerAsync(this.CustomerID, this.UserName, HashedPassword,
                this.Gendor, this.DateOfBirth, this.Phone, this.Email, this.Address, this.CountryID);

        }
        // find by ID , NationalNo
        public static async Task<clsCustomer> FindAsync(int ID)
        {

            CustomerDto customerDto = await CustomerDataAccess.GetCustomerByIDAsync(ID);
            string HashedPassword = Global.Utility.HashingPassword(customerDto.Password);
            if (customerDto != null)
            {
                return new clsCustomer(ID, customerDto.UserName, HashedPassword,
                                       customerDto.Gendor, customerDto.DateOfBirth, customerDto.Phone, customerDto.Email, customerDto.Address
                                                          , customerDto.CountryID);
            }
            else
                return null; 
        }  
        public static async Task<clsCustomer> FindAsync(string UserName, string Password)
        {
            string HashedPassword = Global.Utility.HashingPassword(Password);
            Customer2Dto customer2Dto = await CustomerDataAccess.GetCustomerByUserName_PasswordAsync(UserName, HashedPassword);

            if (customer2Dto != null)
            {
                return new clsCustomer(customer2Dto.CustomerId, UserName,  Password,
                                       customer2Dto.Gendor, customer2Dto.DateOfBirth, customer2Dto.Phone, customer2Dto.Email, customer2Dto.Address
                                                          , customer2Dto.CountryID);
            }
            else
                return null; 
        }

       
        public async Task<bool> SaveAsync()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewCustomer())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return await this._UpdateCustomerAsync();

            }

            return false;
        }

        public static  async Task<bool> DeleteAsync(int ID)
        {
            return await CustomerDataAccess.DeleteCustomerAsync(ID);
        }


        public static  async Task<bool> IsExistsAsync(int ID)
        {
            return await CustomerDataAccess.IsCustomerExistsAsync(ID);
        }
        public static async Task<List<clsCustomer>> GetAllCustomersAsync()
        {
            return await CustomerDataAccess.GetAllCustomersAsync();
        }

       
    }

}