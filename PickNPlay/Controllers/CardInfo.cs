using System.ComponentModel.DataAnnotations;

namespace PickNPlay.Controllers
{
    public class CardInfo : IValidatableObject
    {
        [MinLength(16)]
        public string CardNumber { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int CVC { get; set; }

        [MinLength(3)]
        public string FirstName { get; set; }
        
        [MinLength(3)]
        public string LastName { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            DateTime now = DateTime.Now;

            if (Year < now.Year)
            {
                errors.Add(new("Year cannot be less than current"));
            }

            if (Year == now.Year && Month < now.Month)
            {
                errors.Add(new("Invalid month and year"));
            }

            return errors;
        }
    }
}