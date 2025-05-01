using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using PickNPlay.picknplay_bll.Models.Review;
using PickNPlay.picknplay_dal.Data;
using PickNPlay.picknplay_dal.Entities;

namespace PickNPlay.picknplay_bll.Services
{
    public class ReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _reviewRepository = unitOfWork.ReviewRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReviewGet>> GetAllAsync()
        {
            var reviews = await _reviewRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ReviewGet>>(reviews);
        }

        public async Task<ReviewGet> GetByIdAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            return _mapper.Map<ReviewGet>(review);
        }

        public async Task AddAsync(ReviewPost reviewDto)
        {
            var review = _mapper.Map<Review>(reviewDto);
            await _reviewRepository.AddAsync(review);
        }

        public async Task UpdateAsync(int id, ReviewUpdate reviewDto)
        {
            var existingReview = await _reviewRepository.GetByIdAsync(id);
            if (existingReview == null)
            {
                // Обработка ошибки: отзыв не найден
                return;
            }

            _mapper.Map(reviewDto, existingReview);
            await _reviewRepository.UpdateAsync(existingReview);
        }

        public async Task DeleteAsync(int id)
        {
            await _reviewRepository.DeleteAsync(id);
        }
    }
}
