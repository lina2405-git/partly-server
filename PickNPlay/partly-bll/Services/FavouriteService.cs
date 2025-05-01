using AutoMapper;
using PickNPlay.Exceptions;
using PickNPlay.picknplay_bll.Models.Favourite;
using PickNPlay.picknplay_dal.Data;
using PickNPlay.picknplay_dal.Entities;

namespace PickNPlay.Controllers
{
    public class FavouriteService
    {
        private readonly IFavouriteRepository _favouriteRepository;
        private readonly IMapper mapper;

        public FavouriteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _favouriteRepository = unitOfWork.FavouriteRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<FavouriteGet>> GetAllAsync()
        {
            var categories = await _favouriteRepository.GetAllAsync();
            return categories.Select(mapper.Map<FavouriteGet>);
        }

        public async Task<FavouriteGet> GetByIdAsync(int id)
        {
            
            var model = await _favouriteRepository.GetByIdAsync(id);
            return mapper.Map<FavouriteGet>(model);

        }
        public async Task<IEnumerable<FavouriteGet>> GetByUserId(int userId)
        {
            var models = await _favouriteRepository.GetFavsForExactUser(userId);

            if (!models.Any())
            {
                return Enumerable.Empty<FavouriteGet>();
            }
            
            return models.Select(e =>
            {
                var temp = mapper.Map<FavouriteGet>(e);
                temp.Listing.isFavourite = true;
                return temp;
            });

        }

        public async Task<IEnumerable<FavouriteGet>> AddAsync(FavouritePost favouriteDto)
        {
            try
            {
                var favourite = mapper.Map<Favourite>(favouriteDto);
                var entities = await _favouriteRepository.AddAndReturnUpdatedList(favourite);

                return entities.Select(mapper.Map<FavouriteGet>);
            } 
            catch (DALException)
            {
                throw;
            }
        }

        public async Task<IEnumerable<FavouriteGet>> DeleteAsync(int id, int userId)
        {
            try
            {
                var entities = await _favouriteRepository.DeleteAndReturnUpdatedList(id, userId);
                return entities.Select(mapper.Map<FavouriteGet>);
            }
            catch (DALException)
            {
                throw;
            }
        }

    }
}
