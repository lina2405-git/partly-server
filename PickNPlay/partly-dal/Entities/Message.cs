using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PickNPlay.picknplay_dal.Entities
{
    [Table("messages")]
    [Index(nameof(ReceiverId), Name = "IX_messages_receiver_id")]
    [Index(nameof(SenderId), Name = "IX_messages_sender_id")]
    public partial class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("message_id")]
        public int MessageId { get; set; }
        [Column("sender_id")]
        public int SenderId { get; set; }

        [Column("listing_id")]
        public int? ListingId { get; set; }

        [Column("receiver_id")]
        public int ReceiverId { get; set; }
        [Column("content", TypeName = "text")]
        public string Content { get; set; } = null!;
        [Column("isRead")]
        public bool isRead { get; set; }

        [Column("created_at", TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }

        [ForeignKey(nameof(ReceiverId))]
        [InverseProperty(nameof(User.MessageReceivers))]
        public virtual User Receiver { get; set; } = null!;

        [ForeignKey(nameof(SenderId))]
        [InverseProperty(nameof(User.MessageSenders))]
        public virtual User Sender { get; set; } = null!;
        
        [ForeignKey(nameof(ListingId))]
        [InverseProperty(nameof(Entities.Listing.Messages))]
        public virtual Listing? Listing { get; set; }
    }
}
