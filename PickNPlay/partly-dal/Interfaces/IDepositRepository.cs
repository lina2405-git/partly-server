using PickNPlay.picknplay_dal.Entities;
using PickNPlay.picknplay_dal.Interfaces;

namespace PickNPlay.picknplay_dal.Data
{
    public interface IDepositRepository : IRepository<Deposit>
    {
        Task<bool> TransferMoney(string sessionId);
    }
}