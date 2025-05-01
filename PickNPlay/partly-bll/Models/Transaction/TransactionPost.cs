using PickNPlay.picknplay_dal.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickNPlay.picknplay_bll.Models.Transaction
{
    public class TransactionPost : IValidatableObject
    {
        public int ListingId { get; set; }
        public int Amount { get; set; }
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public decimal BuyerPaid { get; set; }
        public decimal SellerGet { get; set; }



        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (ListingId <= 0)
            {
                errors.Add(new("ListingId cannot be less than 0"));
            }

            if (BuyerId <= 0)
            {
                errors.Add(new("BuyerId cannot be less than 0"));
            }

            if (SellerId <= 0)
            {
                errors.Add(new("SellerId cannot be less than 0"));
            }

            if (BuyerPaid <= 0)
            {
                errors.Add(new("Amount that buyer paid must be more than 0"));
            }

            return errors;
        }

    }
}
