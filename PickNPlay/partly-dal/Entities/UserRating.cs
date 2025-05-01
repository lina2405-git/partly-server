using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PickNPlay.picknplay_dal.Entities
{
    [Table("user_rating")]
    public partial class UserRating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("rating_id")]
        public int RatingId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("rating")]
        public int Rating { get; set; }
        [Column("number_of_reviews")]
        public int NumberOfReviews { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("UserRatings")]
        public virtual User User { get; set; } = null!;
    }
}
