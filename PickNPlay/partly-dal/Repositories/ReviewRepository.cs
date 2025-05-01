using Microsoft.EntityFrameworkCore;
using PickNPlay.picknplay_dal.Data;
using PickNPlay.picknplay_dal.Entities;

public class ReviewRepository : IReviewRepository
{
    private readonly picknplayContext _context;

    public ReviewRepository(picknplayContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Review>> GetAllAsync()
    {
        return await _context.Reviews.ToListAsync();
    }

    public async Task<Review> GetByIdAsync(int id)
    {
        return await _context.Reviews.FindAsync(id);
    }

    public async Task AddAsync(Review entity)
    {
        await _context.Reviews.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Review entity)
    {
        _context.Reviews.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Reviews.FindAsync(id);
        if (entity != null)
        {
            _context.Reviews.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Review>> Filter(int? userId, int page, int size)
    {
        IQueryable<Review> reviews = _context.Reviews.Include(e => e.Transaction.Listing);

        if (userId != null)
        {
            reviews = reviews.Where(x => x.UserId == userId);
        }

        reviews = reviews.OrderByDescending(e => e.ReviewId)
                .Skip(size * (page - 1))
                .Take(size);


        return await reviews.ToListAsync();

    }
}
