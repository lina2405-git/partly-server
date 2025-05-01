using System.ComponentModel.DataAnnotations;

namespace PickNPlay.picknplay_bll.Models.Game
{
    /// <summary>
    /// model to update the game.
    /// validatable:
    /// GameName.Length must be < 50,
    /// GameId must be greater than zero
    /// </summary>
    public class GameUpdate : IValidatableObject
    {
        
        public int GameId { get; set; }
        public string GameName { get; set; } = null!;
        public string? GameLogoUrl { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (GameName.Length >= 50)
            {
                errors.Add(new("Game name must be less than 50"));
            }
            
            if (GameId <= 0)
            {
                errors.Add(new("GameId must be more than 0"));
            }

            return errors;
        }

    }
}
