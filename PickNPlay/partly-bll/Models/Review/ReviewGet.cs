using PickNPlay.picknplay_bll.Models.Transaction;
using PickNPlay.picknplay_bll.Models.User;

namespace PickNPlay.picknplay_bll.Models.Review
{
    public class ReviewGet
    {
        public int ReviewId { get; set; }
        public int TransactionId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual TransactionGet Transaction { get; set; } = null!;
        public virtual UserGet User { get; set; } = null!;

    }
}
