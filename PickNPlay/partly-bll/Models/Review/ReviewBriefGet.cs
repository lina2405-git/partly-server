using PickNPlay.picknplay_bll.Models.Listing;
using PickNPlay.picknplay_bll.Models.Transaction;
using PickNPlay.picknplay_bll.Models.User;

namespace PickNPlay.picknplay_bll.Models.Review
{
    public class ReviewBriefGet
    {
        public int ReviewId { get; set; }
        public int TransactionId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime? CreatedAt { get; set; }
        public ListingNameAndPriceGet ListingNameAndPrice { get; set; }
    }
}
