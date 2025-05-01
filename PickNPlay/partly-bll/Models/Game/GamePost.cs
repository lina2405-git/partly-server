using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PickNPlay.picknplay_bll.Models.Game
{
    /// <summary>
    /// model to post the game.
    /// validatable:
    /// Game name must be less than 50.
    /// url should be validated too.
    /// </summary>
    public class GamePost : IValidatableObject
    {
        public string GameName { get; set; } = null!;
        public string? GameLogoUrl { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if(GameName.Length >= 50)
            {
                errors.Add(new("Game name must be less than 50"));
            }

            return errors;
        }

    }
}
