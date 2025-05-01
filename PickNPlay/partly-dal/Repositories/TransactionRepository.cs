using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PickNPlay.Exceptions;
using PickNPlay.picknplay_dal.Data;
using PickNPlay.picknplay_dal.Entities;

namespace PickNPlay.picknplay_dal.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly picknplayContext _context;

        private int Commision;

        public TransactionRepository(picknplayContext context)
        {
            _context = context;
        }

        public int TransactionCount(int id)
        {
            var count = _context.Transactions
                .Where(t => t.TransactionId == id)
                .Where(t => t.Status == "Completed")
                .Count();
            return count;
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task<Transaction> GetByIdAsync(int id)
        {
            return await _context.Transactions.FindAsync(id);
        }


        public object GetTransactionsCountByMonth()
        {
              var quantity = _context.Transactions
                 .Where(e => e.CreatedAt.Value.Year == DateTime.Now.Year &&
                             e.CreatedAt.Value.Month == DateTime.Now.AddMonths(-1).Month)
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
             return quantity;
        }

        public object GetTransactionsCountByYear()
        {
            var quantity = _context.Transactions
                .Where(e => e.CreatedAt >= DateTime.Now.AddYears(-1))
                .GroupBy(e => new
                {
                    e.CreatedAt.Value.Year,
                    e.CreatedAt.Value.Month,
                })
                .Select(e => new
                {
                    e.Key.Year,
                    e.Key.Month,
                })
                .ToList();

            return quantity;
                
        }

        public object AvgTransactions()
        {
            var totalSellerGet = _context.Transactions.Sum(t => t.SellerGet);
            var totalAmount = _context.Transactions.Sum(t => t.Amount);
            var totalBuyerPaid = _context.Transactions.Sum(t => t.BuyerPaid);

            var avgTotal = totalAmount == 0 ? 0 : (decimal)totalSellerGet / totalAmount;
            var avgCommission = totalSellerGet - totalBuyerPaid;

            var result = new
            {
                AvgTotal = avgTotal,
                AvgCommission = avgCommission
            };

            return result;
        }

        public object StatsCommissionsMonth()
        {
            var previousMonth = DateTime.Now.AddMonths(-1).Month;
            var currentYear = DateTime.Now.Year;

            var transactions = _context.Transactions
                .Where(e => e.CreatedAt.HasValue &&
                            e.CreatedAt.Value.Year == currentYear &&
                            e.CreatedAt.Value.Month == previousMonth)
                .ToList(); 

            var result = transactions
                .GroupBy(e => new
                {
                    e.CreatedAt.Value.Year,
                    e.CreatedAt.Value.Month,
                    e.CreatedAt.Value.Day
                })
                .Select(g => new
                {
                    Date = new DateOnly(g.Key.Year, g.Key.Month, g.Key.Day),
                    AvgCommission = g.Sum(e => e.SellerGet) - g.Sum(e => e.BuyerPaid)
                })
                .OrderBy(e => e.Date)
                .ToList();

            return result;
        }

        public object StatsCommissionsYear()
        {
            var quantity = _context.Transactions
                .Where(e => e.CreatedAt.HasValue &&  e.CreatedAt >= DateTime.Now.AddYears(-1))
                .GroupBy(e => new
                {
                    e.CreatedAt.Value.Year,
                    e.CreatedAt.Value.Month,
                })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    AvgCommission = g.Sum(e => e.SellerGet) - g.Sum(e => e.BuyerPaid)
                })
                .OrderBy(e => e.Month)
                .ToList();
            return quantity;
        }

        public async Task AddAsync(Transaction entity)
        {
            var buyer = await _context.Users.FindAsync(entity.BuyerId)
                ?? throw new DALException($"User with id={entity.BuyerId} was not found");

            var seller = await _context.Users.FindAsync(entity.SellerId)
                ?? throw new DALException($"User with id={entity.BuyerId} was not found");


            var listing = await _context.Listings.FindAsync(entity.ListingId)
                                ?? throw new DALException($"Listing with id={entity.ListingId} was not found");


            buyer.Balance = buyer.Balance - entity.BuyerPaid;

            listing.Amount = listing.Amount - entity.Amount;

            await _context.Transactions.AddAsync(entity);
            await _context.Messages.AddAsync(new()
            {
                Content = $"[Service message] \nThis user purchased your listing \"{listing.Title}\" in a quantity of {entity.Amount} pieces. Please prepare the item and respond to the buyer. \nBuyer, thank you for your purchase! The seller will be in touch with you soon.",
                CreatedAt = DateTime.UtcNow,
                isRead = false,
                ListingId = entity.ListingId,
                SenderId = buyer.UserId,
                ReceiverId = seller.UserId
            });

            _context.Users.UpdateRange(buyer, seller);
            _context.Listings.UpdateRange(listing);

            await _context.SaveChangesAsync(true);
        }

        public async Task UpdateAsync(Transaction entity)
        {
            _context.Transactions.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Transactions.FindAsync(id) ?? throw new DALException($"Transaction was not found");

            _context.Transactions.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<Transaction?> SetStatusToCompleted(int transactionId)
        {
            var transaction = await _context.Transactions.Include(e => e.Seller).FirstAsync(e => e.TransactionId == transactionId)
                ?? throw new DALException($"Transaction was not found");
            var seller = transaction.Seller;

            seller.Balance += transaction.SellerGet;
            transaction.Status = "Completed";

            _context.Transactions.Update(transaction);
            _context.Users.Update(seller);

            await _context.SaveChangesAsync(true);
            return transaction;
        }

        public async Task<Transaction?> SetStatusToCancelled(int transactionId)
        {
            var transaction = await _context.Transactions.Include(e => e.Buyer).FirstAsync(e => e.TransactionId == transactionId)
                ?? throw new DALException($"Transaction was not found");
            var buyer = transaction.Buyer;

            buyer.Balance += transaction.BuyerPaid;
            transaction.Status = "Cancelled";

            _context.Transactions.Update(transaction);
            _context.Users.Update(buyer);

            await _context.SaveChangesAsync(true);
            return transaction;
        }

        public async Task<IEnumerable<Transaction>?> Filter(int? listingId,
                                                            int? amountMoreThan,
                                                            int? amountLessThan,
                                                            int? buyerId,
                                                            int? sellerId,
                                                            string? status,
                                                            DateTime? after,
                                                            DateTime? before,
                                                            int pageNumber = 1,
                                                            int pageSize = 10)
        {
            IQueryable<Transaction> transactions = _context.Transactions
                .Include(e => e.Listing).OrderByDescending(e => e.CreatedAt);

            if (listingId.HasValue)
            {
                transactions = transactions.Where(e => e.ListingId == listingId);
            }

            if (amountMoreThan.HasValue)
            {
                transactions = transactions.Where(e => e.Amount > amountMoreThan);
            }

            if (amountLessThan.HasValue)
            {
                transactions = transactions.Where(e => e.Amount < amountLessThan);
            }

            if (buyerId.HasValue)
            {
                transactions = transactions.Where(e => e.BuyerId == buyerId);
            }

            if (sellerId.HasValue)
            {
                transactions = transactions.Where(e => e.SellerId == sellerId);
            }

            if (!string.IsNullOrEmpty(status))
            {
                transactions = transactions.Where(e => e.Status == status);
            }

            if (after.HasValue)
            {
                transactions = transactions.Where(e => e.CreatedAt >= after);
            }

            if (before.HasValue)
            {
                transactions = transactions.Where(e => e.CreatedAt <= before);
            }

            transactions = transactions
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return await transactions.ToListAsync();
        }

    }
}
