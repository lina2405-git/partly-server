using AngleSharp.Dom;
using Microsoft.EntityFrameworkCore;
using PickNPlay.Exceptions;
using PickNPlay.picknplay_dal.Data;
using PickNPlay.picknplay_dal.Entities;

namespace PickNPlay.picknplay_dal.Repositories
{
    public class DepositRepository : IDepositRepository
    {
        private readonly picknplayContext _context;

        public DepositRepository(picknplayContext context)
        {
            this._context = context;
        }

        public async Task AddAsync(Deposit entity)
        {
            entity.Status = "Created";

            await _context.Deposits.AddAsync(entity);
            await _context.SaveChangesAsync(true);
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Deposit>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Deposit?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> TransferMoney(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return false;
            }
            
            var deposit = await _context.Deposits.Include(e=>e.User).FirstOrDefaultAsync(d => d.SessionId == sessionId);

            if (deposit == null)
            {
                return false;
            }

            deposit.User.Balance += deposit.DepositAmount;
            deposit.Status = "Completed";

            _context.Deposits.Update(deposit);
            await _context.SaveChangesAsync(true);
            return true;
        }

        public Task UpdateAsync(Deposit entity)
        {
            throw new NotImplementedException();
        }
    }
}
