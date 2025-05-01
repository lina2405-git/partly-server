using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace PickNPlay.picknplay_dal.Entities
{
    [Table("listings")]
    [Index(nameof(CategoryId), Name = "IX_listings_category_id")]
    // [Index(nameof(GameId), Name = "IX_listings_game_id")]
    [Index(nameof(UserId), Name = "IX_listings_user_id")]
    public partial class Listing
    {
        public Listing()
        {
            Favourites = new HashSet<Favourite>();
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("listing_id")]
        public int ListingId { get; set; }
        
        [Column("user_id")]
        public int UserId { get; set; }
        
        // [Column("game_id")]
        // public int GameId { get; set; }
        
        [Column("category_id")]
        public int CategoryId { get; set; }
        
        [Column("title")]
        [StringLength(255)]
        [Unicode(false)]
        public string Title { get; set; } = null!;

        [Column("description", TypeName = "text")]
        public string Description { get; set; } = null!;

        [Column("seller_price", TypeName = "decimal(10, 2)")]
        public decimal SellerPrice { get; set; }

        [Column("final_price", TypeName = "decimal(10, 2)")]
        public decimal FinalPrice { get; set; }

        [Column("created_at", TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }

        [Column("status")]
        [StringLength(20)]
        [Unicode(false)]
        public string? Status { get; set; }

        // [Column("provider_id")]
        // public int? ProviderId { get; set; }

        [Column("amount")]
        public int Amount { get; set; }

        [NotMapped]
        public bool isFavourite { get; set; }

        [ForeignKey(nameof(CategoryId))]
        [InverseProperty("Listings")]
        public virtual Category Category { get; set; } = null!;

        // [ForeignKey(nameof(GameId))]
        // [InverseProperty("Listings")]
        // public virtual Game Game { get; set; } = null!;

        // [ForeignKey(nameof(ProviderId))]
        // [InverseProperty(nameof(AccountProvider.Listings))]
        // public virtual AccountProvider? Provider { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("Listings")]
        public virtual User User { get; set; } = null!;

        [InverseProperty(nameof(Favourite.Listing))]
        public virtual ICollection<Favourite> Favourites { get; set; }

        [InverseProperty(nameof(Transaction.Listing))]
        public virtual ICollection<Transaction> Transactions { get; set; }
        
        [InverseProperty(nameof(Message.Listing))]
        public virtual ICollection<Message> Messages { get; set; }
        
        public virtual ICollection<ListingImage> ListingImages { get; set; }
        
        [Column("likes_amount")]
        public int Likes { get; set; }
        
        [Column("views_amount")]
        public int Views { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Listing other = (Listing)obj;
            return ListingId == other.ListingId;
        }

        public override int GetHashCode()
        {
            return ListingId;
        }

    }
}
