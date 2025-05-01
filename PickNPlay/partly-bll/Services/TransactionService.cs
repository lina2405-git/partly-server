using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PickNPlay.Exceptions;
using PickNPlay.picknplay_bll.Models.Transaction;
using PickNPlay.picknplay_dal.Data;
using PickNPlay.picknplay_dal.Entities;
using PickNPlay.picknplay_dal.Repositories;

namespace PickNPlay.picknplay_bll.Services
{
    public class TransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _transactionRepository = unitOfWork.TransactionRepository;
            _userRepository = unitOfWork.UserRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TransactionGet>> GetAllAsync()
        {
            var transactions = await _transactionRepository.GetAllAsync();
            return transactions.Select(_mapper.Map<TransactionGet>);
        }

        public async Task<TransactionGet> GetByIdAsync(int id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            var transactionGet = _mapper.Map<TransactionGet>(transaction);
            if (transactionGet != null)
            {
                transactionGet.TransactionQuantity = _transactionRepository.TransactionCount(id);
            }
            return transactionGet;
        }

        public async Task AddAsync(TransactionPost transactionDto)
        {

            try
            {
                var transaction = _mapper.Map<Transaction>(transactionDto);
                transaction.CreatedAt = DateTime.Now;
                transaction.Status = "Created";
                //check happening in repo
                await _transactionRepository.AddAsync(transaction);
            }
            catch (DALException)
            {
                throw;
            }
        }

        public async Task UpdateAsync(int id, TransactionUpdate transactionDto)
        {
            var existingTransaction = await _transactionRepository.GetByIdAsync(id);
            if (existingTransaction == null)
            {
                // Обработка ошибки: транзакция не найдена
                return;
            }

            _mapper.Map(transactionDto, existingTransaction);
            await _transactionRepository.UpdateAsync(existingTransaction);
        }

        public async Task DeleteAsync(int id)
        {
            await _transactionRepository.DeleteAsync(id);
        }

        public object GetTransactionsCountByMonth()
        {
            return _transactionRepository.GetTransactionsCountByMonth();
        }

        public object GetTransactionsCountByYear()
        {
            return _transactionRepository.GetTransactionsCountByYear();
        }

        public object AvgTransactions()
        {
            return _transactionRepository.AvgTransactions();
        }

        public object StatsCommissionsMonth()
        {
            return _transactionRepository.StatsCommissionsMonth();
        }

        public object StatsCommissionsYear()
        {
            return _transactionRepository.StatsCommissionsYear();
        }

        public async Task<TransactionGet?> SetSuccessStatus(int transactionId)
        {
            try
            {
                var entity = await _transactionRepository.SetStatusToCompleted(transactionId);
                return _mapper.Map<TransactionGet>(entity);
            }
            catch
            {
                throw;
            }
        }

        public async Task<TransactionGet?> SetCancelledStatus(int transactionId)
        {
            try
            {
                var entity = await _transactionRepository.SetStatusToCancelled(transactionId);
                return _mapper.Map<TransactionGet>(entity);
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<TransactionGet>?> FilterTransactions(
        int? listingId, int? amountMoreThan, int? amountLessThan,
        int? buyerId, int? sellerId, string? status,
        DateTime? after, DateTime? before,
        int pageNumber = 1, int pageSize = 10)
        {
            var entities = await _transactionRepository.Filter(
                listingId, amountMoreThan, amountLessThan,
                buyerId, sellerId, status, after, before,
                pageNumber, pageSize);

            return entities.Select(_mapper.Map<TransactionGet>);
        }
    }
}
