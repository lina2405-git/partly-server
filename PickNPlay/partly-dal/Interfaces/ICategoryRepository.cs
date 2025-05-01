using PickNPlay.picknplay_dal.Entities;
using PickNPlay.picknplay_dal.Interfaces;

namespace PickNPlay.picknplay_dal.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<object>> GetPopularCategory(int term);
        
    }
}