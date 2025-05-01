using PickNPlay.picknplay_dal.Interfaces;
using PickNPlay.picknplay_dal.Repositories;

namespace PickNPlay.picknplay_dal.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly picknplayContext context;
        public UnitOfWork(picknplayContext _context)
        {
            context = _context;
        }

        public ICategoryRepository CategoryRepository => new CategoryRepository(context);

        // public IGameRepository GameRepository =>  new GameRepository(context);

        public IListingRepository ListingRepository =>  new ListingRepository(context);

        public IMessageRepository MessageRepository => new MessageRepository(context);

        public IReviewRepository ReviewRepository => new ReviewRepository(context);

        public ITransactionRepository TransactionRepository => new TransactionRepository(context);

        public IUserRepository UserRepository => new UserRepository(context);

        // public IProviderRepository ProviderRepository => new ProviderRepository(context);

        public IFavouriteRepository FavouriteRepository => new FavouriteRepository(context);

        public IDepositRepository DepositRepository => new DepositRepository(context);
        public Task<int> Save()
        {
            return context.SaveChangesAsync(true);
        }
    }
}
