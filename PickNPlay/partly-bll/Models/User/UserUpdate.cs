using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PickNPlay.picknplay_bll.Models.User
{
    public class UserUpdate : IValidatableObject
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? UserImage { get; set; }
        public string PasswordHash { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public int? UserRoleId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (UserId <= 0)
            {
                errors.Add(new("UserId cannot be less than 0"));
            }

            if (Username.Length <= 5)
            {
                errors.Add(new("BuyerId cannot be less than 5"));
            }

            if (Email.Length < 4 )
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


            return errors;
        }

    }
}
