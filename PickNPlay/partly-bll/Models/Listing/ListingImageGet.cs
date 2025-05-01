namespace PickNPlay.picknplay_bll.Models.Listing;

public class ListingImageGet
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    public string ImageUrl { get; set; } = null!;
    public int Order { get; set; }
}