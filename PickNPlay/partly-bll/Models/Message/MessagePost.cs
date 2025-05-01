using PickNPlay.picknplay_dal.Entities;
using System.ComponentModel.DataAnnotations;

namespace PickNPlay.picknplay_bll.Models.Message
{
    public class MessagePost : IValidatableObject
    {
        public int ReceiverId { get; set; }
        public string Content { get; set; } = null!;
        public int? ListingId { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (ReceiverId <= 0)
            {
                errors.Add(new("ReceiverId cannot be less than 0"));
            }

            if (Content.Length == 0)
            {
                errors.Add(new("Message cannot be empty"));
            }

            return errors;
        }

    }
}
