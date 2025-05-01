using PickNPlay.picknplay_dal.Entities;
using PickNPlay.picknplay_dal.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PickNPlay.picknplay_dal.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<Transaction?> SetStatusToCompleted(int transactionId);
        Task<Transaction?> SetStatusToCancelled(int transactionId);
        Task<IEnumerable<Transaction>?> Filter(int? listingId,
                                               int? amountMoreThan,
                                               int? amountLessThan,
                                               int? buyerId,
                                               int? sellerId,
                                               string? status,
                                               DateTime? after,
                                               DateTime? before,
                                               int pageNumber = 1,
                                               int pageSize = 10);
        object GetTransactionsCountByMonth();
        object GetTransactionsCountByYear();
        object AvgTransactions();
        object StatsCommissionsMonth();
        object StatsCommissionsYear();
        int TransactionCount(int id);

    }
}