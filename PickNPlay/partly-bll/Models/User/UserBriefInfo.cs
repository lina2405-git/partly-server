using PickNPlay.picknplay_bll.Models.Review;

namespace PickNPlay.picknplay_bll.Models.User
{
    public class UserBriefInfoWithReviews
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string? UserImage { get; set; }

        public string Email { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public int? UserRoleId { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<ReviewGet> Reviews { get; set; }

    }
}
