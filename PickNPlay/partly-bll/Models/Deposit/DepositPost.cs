using System.ComponentModel.DataAnnotations;

namespace PickNPlay.picknplay_bll.Models.Deposit
{
    public class DepositPost : IValidatableObject
    {
        public int UserId { get; set; }
        public decimal DepositAmount { get; set; }
        public string SessionId { get; set; } = null!;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (SessionId == null || string.IsNullOrEmpty(SessionId))
            {
                errors.Add(new("Session id cannot bu null or empty"));
            }

            if (DepositAmount <= 0)
            {
                errors.Add(new("Deposit amount cannot be less or equal 0"));
            }

            if (UserId > 0)
            {
                errors.Add(new("User id cannot be less than 0"));
            }

            return errors;

        }
    }
}
