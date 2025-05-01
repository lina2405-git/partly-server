using AutoMapper;
using PickNPlay.picknplay_bll.Models.Category;
using PickNPlay.picknplay_dal.Data;
using PickNPlay.picknplay_dal.Entities;
using PickNPlay.picknplay_dal.Repositories;

namespace PickNPlay.picknplay_bll.Services
{

    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _categoryRepository = unitOfWork.CategoryRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<CategoryGet>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(mapper.Map<CategoryGet>);
        }

        public async Task<CategoryGet> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return mapper.Map<CategoryGet>(category);
        }

        public async Task<IEnumerable<object>> GetPopularCategory(int term)
        {
            var categories = await _categoryRepository.GetPopularCategory(term);
            return categories;
        }

        public async Task AddAsync(CategoryPost categoryDto)
        {
            var category = mapper.Map<Category>(categoryDto);
            await _categoryRepository.AddAsync(category);
        }

        public async Task UpdateAsync(int id, CategoryUpdate categoryDto)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null)
            {
                // Обработка ошибки: категория не найдена
                return;
            }

            mapper.Map(categoryDto, existingCategory);
            await _categoryRepository.UpdateAsync(existingCategory);
        }

        public async Task DeleteAsync(int id)
        {
            await _categoryRepository.DeleteAsync(id);
        }
    }
}

