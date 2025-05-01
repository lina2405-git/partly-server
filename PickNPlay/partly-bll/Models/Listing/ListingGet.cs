using PickNPlay.picknplay_bll.Models.Category;
using PickNPlay.picknplay_bll.Models.Game;
using PickNPlay.picknplay_bll.Models.Message;
using PickNPlay.picknplay_bll.Models.Provider;
using PickNPlay.picknplay_bll.Models.Transaction;
using PickNPlay.picknplay_bll.Models.User;
using PickNPlay.picknplay_dal.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickNPlay.picknplay_bll.Models.Listing
{
    public class ListingGet
    {
        public int ListingId { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal SellerPrice { get; set; }
        public decimal FinalPrice { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Status { get; set; }
        public int Amount { get; set; }
        public bool isFavourite { get; set; } = false;

        public int Likes { get; set; }
        public int Views { get; set; }


        public virtual UserGet User { get; set; } = null!;
        public virtual CategoryGet Category { get; set; } = null!;
        public virtual IEnumerable<MessageGet> Messages { get; set; }

        public virtual IEnumerable<ListingImageGet> ListingImages { get; set; } = null!;
    }
}
