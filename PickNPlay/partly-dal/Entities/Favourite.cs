using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PickNPlay.picknplay_dal.Entities
{
    [Table("favourites")]
    public partial class Favourite
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("favourite_id")]
        public int FavouriteId { get; set; }
        [Column("listing_id")]
        public int ListingId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }

        [ForeignKey(nameof(ListingId))]
        [InverseProperty("Favourites")]
        public virtual Listing Listing { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Favourites")]
        public virtual User User { get; set; } = null!;
    }
}
