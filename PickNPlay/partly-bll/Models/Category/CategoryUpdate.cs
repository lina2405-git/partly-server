namespace PickNPlay.picknplay_bll.Models.Category
{
    public class CategoryUpdate
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string CategoryImageUrl { get; set; }
    }
}
