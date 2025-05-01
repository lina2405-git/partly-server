using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PickNPlay.picknplay_dal.Entities
{
    [Table("users")]
    public partial class User
    {
        public User()
        {
            Favourites = new HashSet<Favourite>();
            Listings = new HashSet<Listing>();
            MessageReceivers = new HashSet<Message>();
            MessageSenders = new HashSet<Message>();
            Reviews = new HashSet<Review>();
            TransactionBuyers = new HashSet<Transaction>();
            TransactionSellers = new HashSet<Transaction>();
            // UserAuthProviders = new HashSet<UserAuthProvider>();
            UserRatings = new HashSet<UserRating>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("username")]
        [StringLength(50)]
        [Unicode(false)]
        public string Username { get; set; } = null!;
        
        [Column("user_image")]
        [StringLength(100)]
        public string? UserImage { get; set; }

        [Column("email")]
        [StringLength(100)]
        [Unicode(false)]
        public string? Email { get; set; }
        [Column("password_hash")]
        [StringLength(100)]
        [Unicode(false)]
        public string? PasswordHash { get; set; }

        [Column("created_at", TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        
        [Column("email_verified_at", TypeName = "datetime")]
        public DateTime? EmailVerifiedAt { get; set; }
        
        [Column("number_verified_at", TypeName = "datetime")]
        public DateTime? NumberVerifiedAt { get; set; }
        
        [Column("email_verification_token")]
        public string? VerificationToken { get; set; }

        [Column("number_verification_token")]
        public string? NumberVerificationToken { get; set; }

        [Column("password_reset_token")]
        public string? PasswordResetToken { get; set; }
        
        [Column("reset_token_expires")]
        public DateTime? ResetTokenExpires { get; set; }

        [Column("phone_number")]
        [StringLength(13)]
        public string? PhoneNumber { get; set; }
        [Column("phone_number_approved")]
        public bool? PhoneNumberApproved { get; set; }
        [Column("email_approved")]
        public bool? EmailApproved { get; set; }
        [Column("user_role_id")]
        public int? UserRoleId { get; set; }

        [Column("balance", TypeName = "decimal(10, 2)")]
        public decimal Balance { get; set; }

        [Column("profile_description", TypeName = "text")]
        public string? ProfileDescription { get; set; }

        [ForeignKey(nameof(UserRoleId))]
        [InverseProperty("Users")]
        public virtual UserRole? UserRole { get; set; }
        [InverseProperty(nameof(Favourite.User))]
        public virtual ICollection<Favourite> Favourites { get; set; }
        [InverseProperty(nameof(Listing.User))]
        public virtual ICollection<Listing> Listings { get; set; }
        [InverseProperty(nameof(Message.Receiver))]
        public virtual ICollection<Message> MessageReceivers { get; set; }
        [InverseProperty(nameof(Message.Sender))]
        public virtual ICollection<Message> MessageSenders { get; set; }
        [InverseProperty(nameof(Review.User))]
        public virtual ICollection<Review> Reviews { get; set; }
        [InverseProperty(nameof(Transaction.Buyer))]
        public virtual ICollection<Transaction> TransactionBuyers { get; set; }
        [InverseProperty(nameof(Transaction.Seller))]
        public virtual ICollection<Transaction> TransactionSellers { get; set; }
        // [InverseProperty(nameof(UserAuthProvider.User))]
        // public virtual ICollection<UserAuthProvider> UserAuthProviders { get; set; }

        [InverseProperty(nameof(UserRating.User))]
        public virtual ICollection<UserRating> UserRatings { get; set; }

        [InverseProperty(nameof(Deposit.User))]
        public virtual ICollection<Deposit> Deposits { get; set; }

    }
}
