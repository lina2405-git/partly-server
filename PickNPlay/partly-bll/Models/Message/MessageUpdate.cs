using System.ComponentModel.DataAnnotations;

namespace PickNPlay.picknplay_bll.Models.Message
{
    public class MessageUpdate : IValidatableObject
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Content { get; set; } = null!;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (SenderId <= 0)
            {
                errors.Add(new("SenderId cannot be less than 0"));
            }

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
