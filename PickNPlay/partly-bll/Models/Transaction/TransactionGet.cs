using PickNPlay.picknplay_bll.Models.Listing;
using PickNPlay.picknplay_bll.Models.Review;
using PickNPlay.picknplay_bll.Models.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickNPlay.picknplay_bll.Models.Transaction
{
    public class TransactionGet
    {
        public int TransactionId { get; set; }
        public int ListingId { get; set; }
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public decimal Amount { get; set; }
        public decimal BuyerPaid { get; set; }
        public decimal SellerGet { get; set; }
        public string? SessionId { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int TransactionQuantity { get; set; }
        public virtual ListingGet Listing { get; set; } = null!;

    }
}
