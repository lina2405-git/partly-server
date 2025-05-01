using AutoMapper;
using PickNPlay.Controllers;
using PickNPlay.Exceptions;
using PickNPlay.picknplay_bll.Models;
using PickNPlay.picknplay_bll.Models.Listing;
using PickNPlay.picknplay_bll.Models.Message;
using PickNPlay.picknplay_dal.Data;
using PickNPlay.picknplay_dal.Entities;

namespace PickNPlay.picknplay_bll.Services
{
    public class ListingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ListingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;

            _mapper = mapper;
        }

        public async Task<IEnumerable<ListingGet>> GetAllAsync()
        {
            var listings = await _unitOfWork.ListingRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ListingGet>>(listings);
        }

        public async Task<ListingGet> GetByIdWithoutReviewsAsync(int listingId)
        {
            var listing = await _unitOfWork.ListingRepository.GetByIdAsync(listingId);
            return _mapper.Map<ListingGet>(listing);
        }

        public async Task<int> GetListingNumberByFilters(int? idCategory = null, int? idGame = null, int? idPlatform = null)
        {
            int quantity = await _unitOfWork.ListingRepository.GetListingNumberByFilters(idCategory, idGame, idPlatform);
            return quantity;
        }

        public async Task<ListingGet?> GetByIdCheckingFavouriteAsync(int listingId, int? userId, int reviewsPage = 1, int reviewsSize = 10)
        {
            var listing = await _unitOfWork.ListingRepository.GetByIdAsync(listingId);

            if (listing == null)
            {
                return null;
            }

            var reviews = await _unitOfWork.ReviewRepository.Filter(listing.UserId, reviewsPage, reviewsSize);


            listing.User.Reviews = (ICollection<Review>)reviews;

            int TotalRating = listing.User.Reviews.Select(n => n.Rating).Sum();

            double AvgRating = 0;
            
            if (listing.User.Reviews.Count() != 0)
                AvgRating = (double)TotalRating / listing.User.Reviews.Count();
            
            if (userId.HasValue)
            {
                listing.isFavourite = await _unitOfWork.FavouriteRepository.IsFavouriteForUser(listingId, userId.Value);
            }

            var model = _mapper.Map<ListingGet>(listing);
            model.User.AvgRating = AvgRating;

            if (userId.HasValue)
            {
                var messages = await _unitOfWork.MessageRepository.GetMessagesAndMarkAsRead(listingId, userId.Value, listing.UserId, userId.Value);
                model.Messages = messages.Select(e =>
                {
                    var temp = _mapper.Map<MessageGet>(e);

                    if (e.SenderId == userId)
                    {
                        temp.isSenderCurrentUser = true;
                    }

                    return temp;
                });
            }

            model.User.TransactionQuantity = _unitOfWork.UserRepository.TransactionCount(listing.UserId);

            return model;
        }

        public async Task AddAsync(ListingPost listingDto, int userId)
        {
            var listing = _mapper.Map<Listing>(listingDto);

            listing.UserId = userId;
            listing.Status = "Active";
            listing.FinalPrice = listingDto.SellerPrice;
            
            
            if (listingDto.ListingImages != null)
            {
                listing.ListingImages = new List<ListingImage>();
                
                foreach (var image in listingDto.ListingImages)
                {
                    var mappedImage = _mapper.Map<ListingImage>(image);
                    mappedImage.ListingId = listing.ListingId;
                    listing.ListingImages.Add(mappedImage);
                }
            }

            await _unitOfWork.ListingRepository.AddAsync(listing);
        }


        public async Task UpdateAsync(int userId, ListingUpdate listingDto)
        {

            var mapped = _mapper.Map<Listing>(listingDto);

            mapped.UserId = userId;
            mapped.Status = "Active";
            mapped.FinalPrice = listingDto.SellerPrice;

            await _unitOfWork.ListingRepository.UpdateAsync(mapped);
        }

        public async Task DeactivateAsync(int id)
        {
            await _unitOfWork.ListingRepository.DeleteAsync(id);
        }
        public async Task ActivateAsync(int id)
        {
            await _unitOfWork.ListingRepository.ActivateListing(id);
        }
        public async Task<IEnumerable<ListingGet>> Filter(
            int? userId,
            int? gameId,
            int? categoryId,
            decimal? minPrice,
            decimal? maxPrice,
            int? providerId,
            string? keyword,
            int? сurrentUserId,
            int page = 1,
            int size = 30)
        {
            var entities = await _unitOfWork.ListingRepository.Filter(userId, сurrentUserId, gameId, categoryId, minPrice, maxPrice, providerId, keyword, page, size);

            if (сurrentUserId.HasValue && сurrentUserId != 0)
            {
                //gets all favs for exact user
                var favouriteListingIds = await _unitOfWork.FavouriteRepository.GetListingIdsInFavsForExactUser(сurrentUserId.Value);

                //checks whether the listing is in the fav list
                var intersectedFavIds = entities.Select(e => e.ListingId).Intersect(favouriteListingIds);


                var modelsToReturn = entities.Select(e =>
                {
                    var temp = _mapper.Map<ListingGet>(e);
                    if (intersectedFavIds.Contains(temp.ListingId))
                    {
                        temp.isFavourite = true;
                    }
                    return temp;
                });

                return modelsToReturn;
            }

            return entities.Select(_mapper.Map<ListingGet>);
        }

        public async Task<PaginationInfo> GetPaginationInfo(int page, int size)
        {
            return await _unitOfWork.ListingRepository.GetPaginationInfo(page, size);
        }

        public object GetStatsByMonthAsync()
        {
            return  _unitOfWork.ListingRepository.GetAmountOfCreatedByMonth();
        }

        public object GetStatsByYearAsync()
        {
            return _unitOfWork.ListingRepository.GetAmountOfCreatedByYear();
        }

        public async Task<int> Count()
        {
            return await _unitOfWork.ListingRepository.Count();
        }


    }

}
