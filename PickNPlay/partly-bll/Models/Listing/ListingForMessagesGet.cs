using PickNPlay.picknplay_bll.Models.User;
using PickNPlay.picknplay_dal.Entities;

namespace PickNPlay.picknplay_bll.Models.Listing
{
    public class ListingForMessagesGet
    {
        public int ListingId { get; set; }
        
        public string Title { get; set; }
        public decimal FinalPrice { get; set; }
        public virtual UserGet User { get; set; } = null!;
        public int UnreadMessageCount { get; set; }

        public List<ListingImageGet> ListingImages { get; set; }
    }
}
