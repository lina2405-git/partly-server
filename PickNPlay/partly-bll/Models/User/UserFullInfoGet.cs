using PickNPlay.picknplay_bll.Models.Listing;
using PickNPlay.picknplay_bll.Models.Message;
using PickNPlay.picknplay_bll.Models.Review;
using PickNPlay.picknplay_bll.Models.Transaction;

namespace PickNPlay.picknplay_bll.Models.User
{
    public class UserFullInfoGet
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string? Email { get; set; }
        public string? UserImage { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfileDescription { get; set; }

        public bool? PhoneNumberApproved { get; set; }
        public bool? EmailApproved { get; set; }
        public int? UserRoleId { get; set; }
        public decimal Balance { get; set; }

        public virtual ICollection<ListingGet> Listings { get; set; }
        public virtual ICollection<MessageGet> MessageReceivers { get; set; }
        public virtual ICollection<MessageGet> MessageSenders { get; set; }
        public virtual ICollection<ReviewGet> Reviews { get; set; }
        public virtual ICollection<TransactionGet> TransactionBuyers { get; set; }
        public virtual ICollection<TransactionGet> TransactionSellers { get; set; }
    }
}
