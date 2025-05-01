using PickNPlay.picknplay_bll.Models;
using PickNPlay.picknplay_dal.Entities;

namespace PickNPlay.picknplay_dal.Interfaces
{
    public interface IListingRepository : IRepository<Listing>
    {
        Task<IEnumerable<Listing>> Filter(
            int? userId,
            int? currentUserId,
            int? gameId,
            int? categoryId,
            decimal? minPrice,
            decimal? maxPrice,
            int? providerId,
            string? keyword,
            int page,
            int size);
        Task ActivateListing(int listingId);
        Task<PaginationInfo> GetPaginationInfo(int page, int size);
        Task<int> GetListingNumberByFilters(int? idCategory = null, int? idGame = null, int? idPlatform = null);
        object GetAmountOfCreatedByMonth();
        object GetAmountOfCreatedByYear();
        Task<int> Count();
    }
}