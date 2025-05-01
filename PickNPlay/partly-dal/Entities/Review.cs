using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PickNPlay.picknplay_dal.Entities
{
    [Table("reviews")]
    [Index(nameof(TransactionId), Name = "IX_reviews_transaction_id")]
    [Index(nameof(UserId), Name = "IX_reviews_user_id")]
    public partial class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("review_id")]
        public int ReviewId { get; set; }
        [Column("transaction_id")]
        public int TransactionId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("rating")]
        public int Rating { get; set; }
        [Column("comment", TypeName = "text")]
        public string? Comment { get; set; }
        [Column("created_at", TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }

        [ForeignKey(nameof(TransactionId))]
        [InverseProperty("Reviews")]
        public virtual Transaction Transaction { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Reviews")]
        public virtual User User { get; set; } = null!;
    }
}
