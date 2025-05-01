using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using PickNPlay.picknplay_dal.Entities;
using PickNPlay.picknplay_dal.Interfaces;

namespace PickNPlay.picknplay_dal.Data
{
    public interface IFavouriteRepository : IRepository<Favourite>
    {
        Task<IEnumerable<Favourite>> GetFavsForExactUser(int userId);
        Task<IEnumerable<int>> GetListingIdsInFavsForExactUser(int userId);
        Task<IEnumerable<Favourite>> AddAndReturnUpdatedList(Favourite entity);
        Task<IEnumerable<Favourite>> DeleteAndReturnUpdatedList(int listingId, int userId);
        Task<bool> IsFavouriteForUser(int listingId, int userId);
        
    }
}