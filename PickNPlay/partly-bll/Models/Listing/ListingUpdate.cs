using PickNPlay.picknplay_dal.Entities;
using System.ComponentModel.DataAnnotations;

namespace PickNPlay.picknplay_bll.Models.Listing
{
    public class ListingUpdate : IValidatableObject
    {
        public int ListingId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal SellerPrice { get; set; }
        
        public int Amount { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (CategoryId <= 0)
            {
                errors.Add(new("CategoryId cannot be less than 0"));
            }

            if (Title.Length <= 10)
            {
                errors.Add(new("Title must be more than 10 symbols long"));
            }

            if (Description.Length <= 20)
            {
                errors.Add(new("Description must be more than 20 symbols long"));
            }

            if (SellerPrice <= 0)
            {
                errors.Add(new("Price must be greater than 0"));
            }

            return errors;
        }

    }
}
