using Microsoft.EntityFrameworkCore;

using PickNPlay.picknplay_dal.Data;
using PickNPlay.picknplay_dal.Entities;

namespace PickNPlay.picknplay_dal.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly picknplayContext _context;

        public CategoryRepository(picknplayContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<IEnumerable<object>> GetPopularCategory(int term)
        {
            var termin = DateTime.Now.AddDays(-term);

            var results = await _context.Categories
                .Join(
                    _context.Listings,
                    c => c.CategoryId,
                    l => l.CategoryId,
                    (c, l) => new { Category = c, Listing = l }
                )
                .Join(
                    _context.Transactions,
                    cl => cl.Listing.ListingId,
                    t => t.Listing.ListingId,
                    (cl, t) => new { cl.Category, Transaction = t }
                )
                .Where(ct => ct.Transaction.Status == "Completed" && ct.Transaction.CreatedAt >= termin)
                .GroupBy(ct => ct.Category.CategoryName)
                .Select(g => new
                {
                    CategoryName = g.Key,
                    SuccessfulTransactionsCount = g.Count()
                })
                .OrderByDescending(r => r.SuccessfulTransactionsCount)
                .ToListAsync();
            return results;
        }

        public async Task AddAsync(Category entity)
        {
            var created = await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category entity)
        {
            _context.Categories.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FirstAsync(e => e.CategoryId == id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

    }

}
