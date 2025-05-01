using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace YourNamespaceHere.Controllers
{
    public class ResetModel : IValidatableObject
    {
        public string ResetToken { get; set; }
        public string NewPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (NewPassword.Length < 8)
            {
                errors.Add(new("Password must be more than 8"));
            }

            if (!Regex.IsMatch(NewPassword, @"(?=.*[A-Z])(?=.*\W)(?=(?:.*\d){3,})"))
            {
                errors.Add(new("Password must contain: 1) at least one downscale symbol;\n2) at least one upperscale symbol;\n3)at least three digits"));
            }

            return errors;

        }
    }
}