// using Microsoft.EntityFrameworkCore;
// using PickNPlay.Exceptions;
// using PickNPlay.picknplay_dal.Data;
// using PickNPlay.picknplay_dal.Entities;
// using PickNPlay.picknplay_dal.Interfaces;
//
// namespace PickNPlay.picknplay_dal.Repositories
// {
//     public class ProviderRepository : IProviderRepository
//     {
//         private readonly picknplayContext _context;
//
//         public ProviderRepository(picknplayContext context)
//         {
//             _context = context;
//         }
//         public async Task AddAsync(AccountProvider entity)
//         {
//             await _context.AccountProviders.AddAsync(entity);
//             await _context.SaveChangesAsync();
//         }
//
//         public async Task DeleteAsync(int id)
//         {
//             var provider = await _context.AccountProviders.FirstAsync(e => e.ProviderId == id);
//             if (provider != null)
//             {
//                 _context.AccountProviders.Remove(provider);
//                 await _context.SaveChangesAsync();
//             }
//         }
//
//         public async Task EditUrl(int id, string newUrl)
//         {
//             var entity = _context.AccountProviders.Find(id);
//             if (entity != null)
//             {
//                 entity.ProviderLogoUrl = newUrl;
//                 await _context.SaveChangesAsync(true);
//             }
//             else
//             {
//                 throw new DALException($"the record with id={id} was not found");
//             }
//         }
//
//         public async Task<IEnumerable<AccountProvider>> Filter(int page, int pageSize)
//         {
//             return await _context.AccountProviders
//                 .Skip(pageSize * (page - 1))
//                 .Take(pageSize).ToListAsync();
//         }
//
//         public async Task<IEnumerable<AccountProvider>> GetAllAsync()
//         {
//             return await _context.AccountProviders.ToListAsync();
//         }
//
//         public async Task<AccountProvider?> GetByIdAsync(int id)
//         {
//             return await _context.AccountProviders.FindAsync(id);
//
//         }
//
//         public async Task UpdateAsync(AccountProvider entity)
//         {
//             _context.AccountProviders.Update(entity);
//             await _context.SaveChangesAsync();
//         }
//         public async Task<IEnumerable<object>> GetPopularProvider(int term)
//         {
//             var termin = DateTime.Now.AddDays(-term);
//
//             var results = await _context.AccountProviders
//                 .Join(
//                     _context.Listings,
//                     c => c.ProviderId,
//                     l => l.ProviderId,
//                     (c, l) => new { Category = c, Listing = l }
//                 )
//                 .Join(
//                     _context.Transactions,
//                     cl => cl.Listing.ListingId,
//                     t => t.Listing.ListingId,
//                     (cl, t) => new { cl.Category, Transaction = t }
//                 )
//                 .Where(ct => ct.Transaction.Status == "Completed" && ct.Transaction.CreatedAt >= termin)
//                 .GroupBy(ct => ct.Category.ProviderName)
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
