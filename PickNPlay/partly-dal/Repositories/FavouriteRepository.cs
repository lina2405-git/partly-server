using Microsoft.EntityFrameworkCore;
using PickNPlay.Exceptions;
using PickNPlay.picknplay_dal.Data;
using PickNPlay.picknplay_dal.Entities;

namespace PickNPlay.picknplay_dal.Repositories
{
    public class FavouriteRepository : IFavouriteRepository
    {
        private picknplayContext _context;

        public FavouriteRepository(picknplayContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Favourite>> GetAllAsync()
        {
            return _context.Favourites.ToList();
        }

        public async Task<Favourite> GetByIdAsync(int favId)
        {
            return await _context.Favourites.FindAsync(favId);
        }

        public async Task AddAsync(Favourite favourite)
        {
            try
            {
                _context.Favourites.Add(favourite);
                await IncreaseLikes(favourite.ListingId);

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAsync(Favourite favourite)
        {
            _context.Entry(favourite).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int favId)
        {
            var favourite = await _context.Favourites.FindAsync(favId);
            if (favourite != null)
            {
                _context.Favourites.Remove(favourite);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Favourite>> GetFavsForExactUser(int userId)
        {
            var favs = await _context.Favourites
                // .Include(e => e.Listing.Game)
                .Include(e=>e.Listing)
                .Where(e => e.UserId == userId)
                .ToListAsync() ?? throw new DALException("Favs for this user were not found"); ;

            return favs;
        }

        public async Task<IEnumerable<Favourite>> AddAndReturnUpdatedList(Favourite entity)
        {
            await AddAsync(entity);
            await IncreaseLikes(entity.ListingId);
            var entities = await GetFavsForExactUser(entity.UserId);
            return entities;
        }

        public async Task<IEnumerable<Favourite>> DeleteAndReturnUpdatedList(int listingId, int userId)
        {
            var entity = await _context.Favourites
                .FirstAsync(e => e.ListingId == listingId && e.UserId == userId);

            _context.Favourites.Remove(entity);

            await DecreaseLikes(listingId);
            await _context.SaveChangesAsync(true);

            var entities = await GetFavsForExactUser(entity.UserId);
            return entities;
        }

        public async Task<bool> IsFavouriteForUser(int listingId, int userId)
        {
            return await _context.Favourites.AnyAsync(e => e.ListingId == listingId && e.UserId == userId);
        }

        public async Task<IEnumerable<int>> GetListingIdsInFavsForExactUser(int userId)
        {
            var favs = await _context.Favourites.Where(e => e.UserId == userId).ToListAsync();

            return favs.Select(e => e.ListingId);

        }

        public async Task IncreaseLikes(int listingId)
        {
            var entity = await _context.Listings.FindAsync(listingId) ?? throw new DALException("Listing was not found");
            entity.Likes += 1;
        }
        public async Task DecreaseLikes(int listingId)
        {
            var entity = await _context.Listings.FindAsync(listingId) ?? throw new DALException("Listing was not found");
            if (entity.Likes < 0)
            {
                entity.Likes = 0;
                return;
            }
            entity.Likes += 1;
        }
    }
}
