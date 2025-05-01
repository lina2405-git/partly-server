using PickNPlay.picknplay_dal.Interfaces;
using PickNPlay.picknplay_dal.Repositories;

namespace PickNPlay.picknplay_dal.Data
{
    public interface IUnitOfWork
    {
        public ICategoryRepository CategoryRepository { get; }
        // public IGameRepository GameRepository { get; }
        public IListingRepository ListingRepository { get; }
        public IMessageRepository MessageRepository { get; }
        public IReviewRepository ReviewRepository { get; }
        public ITransactionRepository TransactionRepository { get; }
        public IUserRepository UserRepository { get; }
        // public IProviderRepository ProviderRepository { get; }
        public IFavouriteRepository FavouriteRepository { get; }
        public IDepositRepository DepositRepository { get; }
        public Task<int> Save();
    }
}