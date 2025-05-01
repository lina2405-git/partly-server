// using Microsoft.EntityFrameworkCore;
// using PickNPlay.picknplay_dal.Data;
// using PickNPlay.picknplay_dal.Entities;
//
// namespace PickNPlay.picknplay_dal.Repositories
// {
//     public class GameRepository : IGameRepository
//     {
//         private readonly picknplayContext _context;
//
//         public GameRepository(picknplayContext context)
//         {
//             _context = context;
//         }
//
//         public async Task<IEnumerable<Game>> GetAllAsync()
//         {
//             return _context.Games.ToList();
//         }
//
//         public async Task<Game> GetByIdAsync(int gameId)
//         {
//             return await _context.Games.FindAsync(gameId);
//         }
//
//         public async Task AddAsync(Game game)
//         {
//             _context.Games.Add(game);
//             await _context.SaveChangesAsync();
//         }
//
//         public async Task UpdateAsync(Game game)
//         {
//             _context.Entry(game).State = EntityState.Modified;
//             await _context.SaveChangesAsync();
//         }
//
//         public async Task DeleteAsync(int gameId)
//         {
//             var game = await _context.Games.FindAsync(gameId);
//             if (game != null)
//             {
//                 _context.Games.Remove(game);
//                 await _context.SaveChangesAsync();
//             }
//         }
//
//         public async Task<IEnumerable<Game>> Filter(int page = 1, int size = 10)
//         {
//             IQueryable<Game> games = _context.Games.AsQueryable();
//
//             return await games
//                 .Skip(size * (page - 1))
//                 .Take(size)
//                 .OrderBy(x => x.GameId)
//                 .ToListAsync();
//         }
//
//         public async Task<int> GetNumberOfListingsForGameAsync(int gameId)
//         {
//             return await _context.Listings.Where(e => e.GameId == gameId).CountAsync();
//         }
//
//         public async Task<Game> GetGameWithListingsAsync(int gameId)
//         {
//             var entity = await _context.Games
//                 .Include(e => e.Listings)
//                 .FirstAsync(e => e.GameId == gameId);
//             return entity;
//         }
//         public async Task<IEnumerable<object>> GetPopularGame(int term)
//         {
//             var termin = DateTime.Now.AddDays(-term);
//
//             var results = await _context.Games
//                 .Join(
//                     _context.Listings,
//                     c => c.GameId,
//                     l => l.GameId,
//                     (c, l) => new { Category = c, Listing = l }
//                 )
//                 .Join(
//                     _context.Transactions,
//                     cl => cl.Listing.ListingId,
//                     t => t.Listing.ListingId,
//                     (cl, t) => new { cl.Category, Transaction = t }
//                 )
//                 .Where(ct => ct.Transaction.Status == "Completed" && ct.Transaction.CreatedAt >= termin)
//                 .GroupBy(ct => ct.Category.GameName)
//                 .Select(g => new
//                 {
//                     CategoryName = g.Key,
//                     SuccessfulTransactionsCount = g.Count()
//                 })
//                 .OrderByDescending(r => r.SuccessfulTransactionsCount)
//                 .ToListAsync();
//             return results;
//         }
//     }
// }
//
