// using AutoMapper;
// using PickNPlay.picknplay_bll.Models.Category;
// using PickNPlay.picknplay_bll.Models.Provider;
// using PickNPlay.picknplay_dal.Data;
// using PickNPlay.picknplay_dal.Entities;
// using PickNPlay.picknplay_dal.Interfaces;
// using PickNPlay.picknplay_dal.Repositories;
//
// namespace PickNPlay.picknplay_bll.Services
// {
//     public class ProviderService
//     {
//         private readonly IProviderRepository _providerRepository;
//         private readonly IMapper mapper;
//
//         public ProviderService(IUnitOfWork unitOfWork, IMapper mapper)
//         {
//             _providerRepository = unitOfWork.ProviderRepository;
//             this.mapper = mapper;
//         }
//
//         public async Task<IEnumerable<ProviderGet>> GetAllAsync()
//         {
//             var categories = await _providerRepository.GetAllAsync();
//             return categories.Select(mapper.Map<ProviderGet>);
//         }
//
//         public async Task<ProviderGet> GetByIdAsync(int id)
//         {
//             var category = await _providerRepository.GetByIdAsync(id);
//             return mapper.Map<ProviderGet>(category);
//         }
//
//         public async Task AddAsync(ProviderPost providerDto)
//         {
//             var category = mapper.Map<AccountProvider>(providerDto);
//             await _providerRepository.AddAsync(category);
//         }
//
//         public async Task UpdateAsync(int id, CategoryUpdate providerDto)
//         {
//             var existingProvider = await _providerRepository.GetByIdAsync(id);
//             if (existingProvider == null)
//             {
//                 // Обработка ошибки: категория не найдена
//                 return;
//             }
//
//             mapper.Map(providerDto, existingProvider);
//             await _providerRepository.UpdateAsync(existingProvider);
//         }
//
//         public async Task DeleteAsync(int id)
//         {
//             await _providerRepository.DeleteAsync(id);
//         }
//
//         internal async Task EditUrl(int id, string newUrl)
//         {
//             try
//             {
//                 await _providerRepository.EditUrl(id, newUrl);
//             }
//             catch (Exception)
//             {
//
//                 throw;
//             }
//         }
//
//         public async Task<IEnumerable<ProviderGet>> Filter(int page, int size)
//         {
//             var entities = await _providerRepository.Filter(page, size);
//             return entities.Select(mapper.Map<ProviderGet>);
//         }
//
//         public async Task<IEnumerable<object>> GetPopularProvider(int term)
//         {
//             return await _providerRepository.GetPopularProvider(term);
//         }
//     }
// }
