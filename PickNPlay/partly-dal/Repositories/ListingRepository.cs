using Microsoft.EntityFrameworkCore;
using PickNPlay.Exceptions;
using PickNPlay.picknplay_bll.Models;
using PickNPlay.picknplay_dal.Data;
using PickNPlay.picknplay_dal.Entities;
using PickNPlay.picknplay_dal.Interfaces;

namespace PickNPlay.picknplay_dal.Repositories
{
    public class ListingRepository : IListingRepository
    {
        private readonly picknplayContext _context;

        public ListingRepository(picknplayContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Listing>> GetAllAsync()
        {
            return await _context.Listings.ToListAsync();
        }

        public async Task<Listing> GetByIdAsync(int id)
        {
            //доделать
            await IncreaseViews(id);

            return await _context.Listings
                // .Include(e => e.Game)
                // .Include(e => e.Provider)
                .Include(e => e.Category)
                .Include(e => e.User)
                .Include(e=>e.ListingImages)
                .FirstAsync(e => e.ListingId == id);
        }

        public async Task<int> GetListingNumberByFilters(int? idCategory = null, int? idGame = null, int? idPlatform = null)
        {
            IQueryable<Listing> query = _context.Listings;

            if (idCategory.HasValue)
            {
                query = query.Where(l => l.CategoryId == idCategory);
            }

            // if (idGame.HasValue)
            // {
            //     query = query.Where(l => l.GameId == idGame);
            // }

            // if (idPlatform.HasValue)
            // {
            //     query = query.Where(l => l.ProviderId == idPlatform);
            // }

            int listinsQuantity = await query.CountAsync();

            return listinsQuantity;
        }


        public object GetAmountOfCreatedByMonth()
        {

            var dateAndAmount = _context.Listings
                             .Where(e => e.CreatedAt >= DateTime.Now.AddMonths(-1))
                             .GroupBy(e => new
                             {
                                 e.CreatedAt.Value.Year,
                                 e.CreatedAt.Value.Month,
                                 e.CreatedAt.Value.Day,
                             })
                             .Select(e => new
                             {
                                 Date = new DateOnly(e.Key.Year, e.Key.Month, e.Key.Day),
                                 Amount = e.Count()
                             })
                             .ToList();

            return new
            {
                TotalAmount = dateAndAmount.Sum(e=>e.Amount),
                AmountByDays = dateAndAmount
            };
        }
        public object GetAmountOfCreatedByYear()
        {
            var dateAndAmount = _context.Listings
                             .Where(e => e.CreatedAt >= DateTime.Now.AddYears(-1))
                             .GroupBy(e => new
                             {
                                 e.CreatedAt.Value.Year,
                                 e.CreatedAt.Value.Month,
                                 
                             })
                             .Select(e => new
                             {
                                 e.Key.Month,
                                 Amount = e.Count()
                             })
                             .ToList();

            return new
            {
                TotalAmount = dateAndAmount.Sum(e => e.Amount),
                AmountByMonths = dateAndAmount
            };
        }


        public async Task AddAsync(Listing entity)
        {
            await _context.Listings.AddAsync(entity);

            foreach (var image in entity.ListingImages)
            {
                await _context.ListingsImages.AddAsync(image);
            }
            
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Listing entity)
        {
            var trackedEntity = await _context.Listings.FindAsync(entity.ListingId);
            
            var createdDate = trackedEntity.CreatedAt;
            
            entity.CreatedAt = createdDate;
            entity.Views = trackedEntity.Views;
            entity.Likes = trackedEntity.Likes;

            if (trackedEntity != null)
            {
                _context.Entry(trackedEntity).State = EntityState.Detached;
            }

            _context.Listings.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Listings.FindAsync(id);
            if (entity != null)
            {
                entity.Status = "Deactivated";
                await _context.SaveChangesAsync(true);
            }
        }

        public async Task<IEnumerable<Listing>> Filter(
            int? userId,
            int? currentUserId,
            int? gameId,
            int? categoryId,
            decimal? minPrice,
            decimal? maxPrice,
            int? providerId,
            string? keyword,
            int page = 1,
            int size = 30)
        {
            IQueryable<Listing> listings = _context.Listings
                // .Include(e => e.Game)
                // .Include(e => e.Provider)
                .Include(e => e.Category)
                .Include(e=>e.ListingImages)
                .Where(e => e.Status != "Deactivated");


            if (userId.HasValue)
            {
                listings = listings.Where(e => e.UserId == userId);
            }

            //if (currentUserId.HasValue)
            //{
            //    listings = listings.Where(e => e.UserId != currentUserId);
            //}

            // if (gameId.HasValue)
            // {
            //     listings = listings.Where(e => e.GameId == gameId);
            // }

            if (categoryId.HasValue)
            {
                listings = listings.Where(e => e.CategoryId == categoryId);
            }

            if (maxPrice.HasValue)
            {
                listings = listings.Where(e => e.FinalPrice <= maxPrice);
            }

            if (minPrice.HasValue)
            {
                listings = listings.Where(e => e.FinalPrice >= minPrice);
            }

            // if (providerId.HasValue)
            // {
            //     listings = listings.Where(e => e.ProviderId == providerId);
            // }

            if (!string.IsNullOrEmpty(keyword))
            {
                listings = listings.Where(e => EF.Functions.Like(e.Title, $"%{keyword}%")
                                            || EF.Functions.Like(e.Description, $"%{keyword}%"));
            }


            listings = listings
                .OrderByDescending(e => e.CreatedAt)
                .Skip(size * (page - 1))
                .Take(size);


            return await listings.ToListAsync();

        }

        public async Task IncreaseViews(int id)
        {
            var entity = _context.Listings.Find(id) ?? throw new DALException("Fisting with such id was not found");
            entity.Views++;

            await _context.SaveChangesAsync(true);
        }

        public Task ActivateListing(int listingId)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationInfo> GetPaginationInfo(int page, int size)
        {
            int totalListings = await _context.Listings.CountAsync();
            int amountOfPages = (int)Math.Ceiling((double)totalListings / size);
            PaginationInfo info = new PaginationInfo(page, size, amountOfPages);

            return info;
        }

        public async Task<int> Count()
        {
            return await _context.Listings.CountAsync();
        }

    }
}
