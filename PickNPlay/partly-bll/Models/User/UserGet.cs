using PickNPlay.picknplay_bll.Models.Listing;
using PickNPlay.picknplay_bll.Models.Message;
using PickNPlay.picknplay_bll.Models.Review;
using PickNPlay.picknplay_bll.Models.Transaction;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickNPlay.picknplay_bll.Models.User
{
    public class UserGet
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string? UserImage { get; set; }
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string PasswordHash { get; set; } = null!;
        public string? ProfileDescription { get; set; }
        public DateTime? CreatedAt { get; set; }
        
        public int? UserRoleId { get; set; }
        public virtual ICollection<ReviewBriefGet> Reviews { get; set; }
        public decimal Balance { get; set; }
        public double AvgRating { get; set; }
        public int TransactionQuantity { get; set; }
        public bool PhoneNumberApproved { get; set; }
        public bool EmailApproved { get; set; }
    }
}
