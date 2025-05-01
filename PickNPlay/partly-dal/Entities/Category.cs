using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickNPlay.picknplay_dal.Entities
{
    [Table("categories")]
    public partial class Category
    {
        public Category()
        {
            Listings = new HashSet<Listing>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("category_id")]
        public int CategoryId { get; set; }
        
        public string CategoryImageUrl { get; set; }
        
        
        [Column("category_name")]
        [StringLength(30)]
        public string CategoryName { get; set; } = null!;
        //[Column("game_id")]
        //public int? GameId { get; set; }

        //[ForeignKey(nameof(GameId))]
        //[InverseProperty("Categories")]
        //public virtual Game? Game { get; set; }
        [InverseProperty(nameof(Listing.Category))]
        public virtual ICollection<Listing> Listings { get; set; }
    }
}
