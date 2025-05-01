using PickNPlay.picknplay_dal.Entities;
namespace PickNPlay.picknplay_bll.Models.Category
{
    public class CategoryGet
    {
        public int CategoryId { get; set; }
        /// <summary>
        /// the name of category
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryImageUrl { get; set; }

    }

}
