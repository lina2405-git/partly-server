using PickNPlay.picknplay_bll.Models.Category;
using PickNPlay.picknplay_bll.Models.Listing;

namespace PickNPlay.picknplay_bll.Models.Game
{
    /// <summary>
    /// model to get info about the game including related listings and categories
    /// </summary>
    public class GameWithListingsAndCategoriesGet
    {
        public int GameId { get; set; }
        public string GameName { get; set; } = null!;
        public string? GameLogoUrl { get; set; }

        /// <summary>
        /// collection of listings 
        /// </summary>
        public IEnumerable<ListingGet> Listings { get; set; }

        /// <summary>
        /// collection of categories
        /// </summary>
        public IEnumerable<CategoryGet> Categories { get; set; }

    }
}
