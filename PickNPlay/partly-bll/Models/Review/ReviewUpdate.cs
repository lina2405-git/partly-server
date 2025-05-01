using System.ComponentModel.DataAnnotations;

namespace PickNPlay.picknplay_bll.Models.Review
{
    public class ReviewUpdate : IValidatableObject
    {
        public int ReviewId { get; set; }
        public int TransactionId { get; set; } 
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (ReviewId <= 0)
            {
                errors.Add(new("ReviewId cannot be less than 0"));
            }

            if (TransactionId <= 0)
            {
                errors.Add(new("TransactionId cannot be less than 0"));
            }
            
            if (UserId <= 0)
            {
                errors.Add(new("UserId cannot be less than 0"));
            }
            
            if (Rating <= 0 || Rating > 5)
            {
                errors.Add(new("Rating must be between 0 and 5"));
            }

            if (Comment.Length == 0)
            {
                errors.Add(new("Comment cannot be empty"));
            }

            return errors;
        }
    }
}
