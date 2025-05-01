namespace PickNPlay.picknplay_bll.Models.Game
{
    /// <summary>
    /// model to get basic info about game 
    /// </summary>
    public class GameGet
    {
        /// <summary>
        /// game's id
        /// </summary>
        public int GameId { get; set; }
        /// <summary>
        /// game's name
        /// </summary>
        public string GameName { get; set; } = null!;
        /// <summary>
        /// game's logo url
        /// </summary>
        public string? GameLogoUrl { get; set; }

        /// <summary>
        /// the number of listings related to this game
        /// </summary>
        public int ListingNumber { get; set; }

    }
}
