using PickNPlay.picknplay_bll.Models.Listing;

namespace PickNPlay.picknplay_bll.Models.Message
{
    public class ListingWithLastMessage
    {
        public ListingWithLastMessage()
        {
            Listing = new();
        }
        public ListingForMessagesGet Listing { get; set; }
        public MessageGet? LastMessage { get; set; }

    }
}
