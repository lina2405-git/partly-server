using PickNPlay.picknplay_bll.Models.Category;
using PickNPlay.picknplay_bll.Models.Game;
using PickNPlay.picknplay_bll.Models.Provider;
using PickNPlay.picknplay_bll.Models.User;

namespace PickNPlay.picknplay_bll.Models.Listing
{
    public class ListingNameAndPriceGet
    {
        public int ListingId { get; set; }
        public string Title { get; set; } = null!;
        public decimal FinalPrice { get; set; }
        
        public List<ListingImageGet> ListingImages { get; set; }

    }
}
