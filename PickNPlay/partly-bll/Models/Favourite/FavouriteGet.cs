using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PickNPlay.picknplay_dal.Entities;
using PickNPlay.picknplay_bll.Models.Listing;

namespace PickNPlay.picknplay_bll.Models.Favourite
{
    public class FavouriteGet
    {
        public int FavouriteId { get; set; }
        public int ListingId { get; set; }
        public int UserId { get; set; }
        public ListingGet Listing { get; set; }

    }
}
