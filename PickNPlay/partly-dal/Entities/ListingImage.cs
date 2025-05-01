namespace PickNPlay.picknplay_dal.Entities;

public class ListingImage
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    public string ImageUrl { get; set; } = null!;
    public int Order { get; set; }

    public Listing Listing { get; set; }
}