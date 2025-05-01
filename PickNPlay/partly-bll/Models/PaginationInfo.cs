namespace PickNPlay.picknplay_bll.Models
{
    public class PaginationInfo
    {
        public PaginationInfo(int currentPage, int pageSize, int totalAmountOfPages)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalAmountOfPages = totalAmountOfPages;
        }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalAmountOfPages { get; set; }
    }
}