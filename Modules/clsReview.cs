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
    
    public class clsReview
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int ReviewId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string ReviewText { get; set; }
        public decimal Rating { get; set; }
        public DateTime ReviewDate { get; set; }


        public clsReview()
        {
            this.ReviewId = -1;
            this.CustomerId = -1;
            this.ProductId = -1;
            this.ReviewText = "";
            this.Rating = 0;
            this.ReviewDate = DateTime.Now;

            Mode = enMode.AddNew;
        }
        private clsReview(int ReviewId, int CustomerId, int ProductId, string ReviewText, decimal Rating, DateTime ReviewDate)
        {
            this.ReviewId = ReviewId;
            this.CustomerId = CustomerId;
            this.ProductId = ProductId;
            this.ReviewText = ReviewText;
            this.Rating = Rating;
            this.ReviewDate = ReviewDate;

            Mode = enMode.Update;
        }

        private async Task<bool> _AddNewReviewAsync()
        {
            // add this object to database
            // in AddNew Mode 

            this.ReviewId = await ReviewDataAccess.AddNewReviewAsync(this.CustomerId, this.ProductId, this.ReviewText , this.Rating ,this.ReviewDate);

            return (this.ReviewId != -1);

        }
        private async Task<bool> _UpdateReviewAsync()
        {
            // add this object to database
            // in Update Mode

            return await ReviewDataAccess.UpdateReviewAsync(this.ReviewId, this.CustomerId, this.ProductId, this.ReviewText, this.Rating, this.ReviewDate);

        }
        // find by ID , NationalNo
        public static async Task<clsReview> FindAsync(int ReviewId)
        {
            ReviewDto reviewDto = await ReviewDataAccess.GetReviewByIDAsync(ReviewId);
            if(reviewDto != null)
            {
                return new clsReview(ReviewId, reviewDto.CustomerId, reviewDto.ProductId,reviewDto.ReviewText,reviewDto.Rating,reviewDto.ReviewDate);
            }
            else
                return null;
        }

        public async Task<bool> SaveAsync()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewReviewAsync())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return await this._UpdateReviewAsync();

            }

            return false;
        }

        public static  async Task <bool> DeleteAsync(int ID)
        {
            return await ReviewDataAccess.DeleteReviewAsync(ID);
        }

        public static async Task<List<ReviewFullInfoDto>> GetProductReviewsAsync(int ProductId)
        {
            return await ReviewDataAccess.GetReviewsOfProductAsync(ProductId);
        }   
        
        public static async Task<List<ReviewFullInfoDto>> GetCustomerReviewsAsync(int CustomerId)
        {
            return await ReviewDataAccess.GetReviewsOfCustomerAsync(CustomerId);
        }


        public static  async Task <bool> IsExistsAsync(int ID)
        {
            return await ReviewDataAccess.IsReviewExistsAsync(ID);
        }




    }

}
