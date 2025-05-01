using PickNPlay.picknplay_bll.Models.User;

namespace PickNPlay.picknplay_bll.Models.Deposit
{
    public class DepositUpdate
    {
        public int DepositId { get; set; }

        public int UserId { get; set; }

        public decimal DepositAmount { get; set; }

        public string SessionId { get; set; } = null!;

    }
}
