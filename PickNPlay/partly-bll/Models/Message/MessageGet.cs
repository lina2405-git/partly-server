using PickNPlay.picknplay_bll.Models.User;

namespace PickNPlay.picknplay_bll.Models.Message
{
    public class MessageGet
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Content { get; set; } = null!;
        public bool isRead { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool isSenderCurrentUser { get; set; }
        public virtual UserReallyBriefInfoWithoutReviews Receiver { get; set; } = null!;
        public virtual UserReallyBriefInfoWithoutReviews Sender { get; set; } = null!;
    }
}
