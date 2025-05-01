using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PickNPlay.picknplay_bll.Models.User
{
    public class UserPost : IValidatableObject
    {
        public string Username { get; set; } = null!;

        [EmailAddress]
        public string? Email { get; set; } = null!;
        //public string? PhoneNumber { get; set; }

        [Required]
        public string PasswordHash { get; set; } = null!;
        
        [Required, Compare("PasswordHash")]
        public string ConfirmPasswordHash { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            //if (UserId <= 0)
            //{
            //    errors.Add(new("UserId cannot be less or equal than 0"));
            //}

            if (Username.Length <= 5)
            {
                errors.Add(new("Username cannot be less than 5"));
            }

            if (Email.Length < 4)
            {
                errors.Add(new("Email cannot be less than 4 symbols long"));
            }

            if (!Email.Contains('@') || !Email.Contains('.'))
            {
                errors.Add(new("Email must contain '@' and '.' "));
            }

            if (PasswordHash.Length < 8)
            {
                errors.Add(new("Password must be more than 8"));
            }

            if (!Regex.IsMatch(PasswordHash, @"(?=.*[A-Z])(?=.*\W)(?=(?:.*\d){3,})"))
            {
                errors.Add(new("Password must contain: 1) at least one downscale symbol;\n2) at least one upperscale symbol;\n3)at least three digits"));
            }

            //if (!Regex.IsMatch(PhoneNumber, @"^(\+?\d{1,2}\s?)?(\(?\d{3}\)?[\s.-]?)?\d{3}[\s.-]?\d{4}$"))
            //{
            //    errors.Add(new("Phone number is invalid"));
            //}

            return errors;
        }

    }
}
