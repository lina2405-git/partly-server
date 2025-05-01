using PickNPlay.picknplay_bll.Models.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickNPlay.picknplay_bll.Models.Deposit
{
    public class DepositGet
    {
        public int DepositId { get; set; }
        public int UserId { get; set; }
        public decimal DepositAmount { get; set; }
        public string SessionId { get; set; } = null!;
        public virtual UserGet User { get; set; } = null!;
    }
}
