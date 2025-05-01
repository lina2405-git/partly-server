using System.ComponentModel.DataAnnotations;

namespace PickNPlay.picknplay_bll.Models.Transaction
{
    public class TransactionUpdate : IValidatableObject
    {
        public int TransactionId { get; set; }
        public int ListingId { get; set; } 
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public decimal BuyerPaid { get; set; }
        public decimal SellerGet { get; set; }
        public decimal Amount { get; set; }
        public string? Status { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (TransactionId <= 0)
            {
                errors.Add(new("TransactionId cannot be less than 0"));
            }

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

            if (Amount <= 0)
            {
                errors.Add(new("Amount must be more than 0"));
            }

            return errors;
        }

    }
}
