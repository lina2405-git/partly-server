using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PickNPlay.picknplay_dal.Entities
{
    [Table("transactions")]
    [Index(nameof(BuyerId), Name = "IX_transactions_buyer_id")]
    [Index(nameof(ListingId), Name = "IX_transactions_listing_id")]
    [Index(nameof(SellerId), Name = "IX_transactions_seller_id")]
    public partial class Transaction
    {
        public Transaction()
        {
            Reviews = new HashSet<Review>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("transaction_id")]
        public int TransactionId { get; set; }
        [Column("listing_id")]
        public int ListingId { get; set; }

        [Column("amount")]
        public int Amount { get; set; }

        [Column("buyer_id")]
        public int BuyerId { get; set; }
        [Column("seller_id")]
        public int SellerId { get; set; }

        [Column("buyer_paid", TypeName = "decimal(10, 2)")]
        public decimal BuyerPaid { get; set; }

        [Column("seller_get", TypeName = "decimal(10, 2)")]
        public decimal SellerGet { get; set; }

        [Column("status")]
        [StringLength(20)]
        [Unicode(false)]
        public string? Status { get; set; }
        [Column("created_at", TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }

        [Column("session_id")]
        [StringLength(100)]
        public string? SessionId { get; set; }

        [ForeignKey(nameof(BuyerId))]
        [InverseProperty(nameof(User.TransactionBuyers))]
        public virtual User Buyer { get; set; } = null!;

        [ForeignKey(nameof(ListingId))]
        [InverseProperty("Transactions")]
        public virtual Listing Listing { get; set; } = null!;

        [ForeignKey(nameof(SellerId))]
        [InverseProperty(nameof(User.TransactionSellers))]
        public virtual User Seller { get; set; } = null!;

        [InverseProperty(nameof(Review.Transaction))]
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
