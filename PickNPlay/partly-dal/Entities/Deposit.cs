using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickNPlay.picknplay_dal.Entities
{
    [Table("deposits")]
    public class Deposit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("deposit_id")]
        public int DepositId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; } 

        [Column("deposit_amount", TypeName = "decimal(10, 2)")]
        public decimal DepositAmount { get; set; }

        [Column("session_id")]
        public string SessionId { get; set; } = null!;

        [Column("deposit_status")]
        public string Status { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

    }
}
