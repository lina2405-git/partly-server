// using AutoMapper;
// using AutoMapper.Configuration.Annotations;
// using PickNPlay.picknplay_bll.Models.Category;
// using PickNPlay.picknplay_bll.Models.Game;
// using PickNPlay.picknplay_dal.Data;
// using PickNPlay.picknplay_dal.Entities;
// using PickNPlay.picknplay_dal.Repositories;
//
// namespace PickNPlay.picknplay_bll.Services
// {
//     public class GameService
//     {
//         // private readonly IGameRepository _gameRepository;
//         private readonly ICategoryRepository _categoryRepository;
//         private readonly IMapper _mapper;
//
//         public GameService(IUnitOfWork unitOfWork, IMapper mapper)
//         {
//             // _gameRepository = unitOfWork.GameRepository;
//             _categoryRepository = unitOfWork.CategoryRepository;
//             _mapper = mapper;
//         }
//
//         public async Task<IEnumerable<GameGet>> GetAllAsync()
//         {
//             var games = await _gameRepository.GetAllAsync();
//
//             var tasks = games.Select(e =>
//             {
//                 var model = _mapper.Map<GameGet>(e);
//                 model.ListingNumber = _gameRepository.GetNumberOfListingsForGameAsync(e.GameId).Result;
//                 return model;
//             });
//
//             return tasks;
//         }
//
//         public async Task<GameGet> GetByIdAsync(int id)
//         {
//             var game = await _gameRepository.GetByIdAsync(id);
//             return _mapper.Map<GameGet>(game);
//         }
//
//         public async Task AddAsync(GamePost gameDto)
//         {
//             var game = _mapper.Map<Game>(gameDto);
//             await _gameRepository.AddAsync(game);
//         }
//
//         public async Task UpdateAsync(int id, GameUpdate gameDto)
//         {
//             var existingGame = await _gameRepository.GetByIdAsync(id);
//             if (existingGame == null)
//             {
//                 // Обработка ошибки: игра не найдена
//                 return;
//             }
//
//             _mapper.Map(gameDto, existingGame);
//             await _gameRepository.UpdateAsync(existingGame);
//         }
//
//         public async Task DeleteAsync(int id)
//         {
//             await _gameRepository.DeleteAsync(id);
//         }
//
//
//         public async Task<IEnumerable<GameGet>> Filter(int page, int size)
//         {
//             var entities = await _gameRepository.Filter(page, size);
//             return entities.Select(_mapper.Map<GameGet>);
//         }
//         //public async Task<GameWithListingsAndCategoriesGet?> GetGameWithListingsAndCategories(int gameId)
//         //{
//         //    // first, we take entity
//         //    var entity = await _gameRepository.GetGameWithListingsAsync(gameId);
//         //    if (entity == null)
//         //    {
//         //        return null;
//         //    }
//         //    // then we map it
//         //    var model = _mapper.Map<GameWithListingsAndCategoriesGet>(entity);
//
//         //    // then we retrieve categories
//         //    var categories = await _categoryRepository.GetCategoriesByGameId(gameId);
//
//         //    //next, we attach categories to the model(if they are exist, of course)
//         //    if (categories.Count() > 0)
//         //    {
//         //        model.Categories = categories.Select(_mapper.Map<CategoryGet>); 
//         //    }
//
//         //    //finally, return 
//         //    return model;
//         //}
//
//
//         public async Task<IEnumerable<object>> StatisticByGames(int term)
//         {
//             return await _gameRepository.GetPopularGame(term);
//         }
//
//     }
//
// }
