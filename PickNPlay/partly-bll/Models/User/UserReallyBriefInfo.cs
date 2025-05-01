using PickNPlay.picknplay_bll.Models.Review;

namespace PickNPlay.picknplay_bll.Models.User
{
    public class UserReallyBriefInfo
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? UserImage { get; set; }

        public DateTime? CreatedAt { get; set; }
        public int? UserRoleId { get; set; }
        public decimal Balance { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<ReviewGet> Reviews { get; set; }
    }
}
