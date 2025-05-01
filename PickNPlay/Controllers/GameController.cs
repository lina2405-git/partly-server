// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using PickNPlay.picknplay_bll.Models.Game;
// using PickNPlay.picknplay_bll.Services;
//
// namespace PickNPlay.Controllers
// {
//     /// <summary>
//     /// games controller
//     /// </summary>
//     // [Authorize]
//     // [ApiController]
//     // [Route("games")]
//
//     public class GameController : ControllerBase
//     {
//         private readonly GameService _service;
//
//         public GameController(GameService service)
//         {
//             _service = service;
//         }
//         /// <summary>
//         /// returns game by it's id.
//         /// auth is not required.
//         /// </summary>
//         /// <param name="id"></param>
//         /// <returns></returns>
//         [AllowAnonymous]
//         [HttpGet("{id}")]
//         [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameGet))]
//         [ProducesResponseType(StatusCodes.Status404NotFound)]
//         public async Task<ActionResult<GameGet>> GetGameByIdAsync(int id)
//         {
//             var game = await _service.GetByIdAsync(id);
//             if (game != null)
//             {
//                 return Ok(game);
//             }
//             else
//             {
//                 return NotFound();
//             }
//         }
//         /// <summary> 
//         /// returns game with its listings and categories to which this game is related to.
//         /// auth is not required.
//         /// </summary>
//         /// <param name="id"></param>
//         /// <returns></returns>
//         //[AllowAnonymous]
//         //[HttpGet("{id}/withListingsAndCategories")]
//         //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameWithListingsAndCategoriesGet))]
//         //[ProducesResponseType(StatusCodes.Status404NotFound)]
//         //public async Task<ActionResult<GameWithListingsAndCategoriesGet>> GetGameWithListingsAndCategories(int id)
//         //{
//         //    var model = await _service.GetGameWithListingsAndCategories(id);
//         //    if (model != null)
//         //    {
//         //        return Ok(model);
//         //    }
//         //    return NotFound();
//         //}
//
//         /// <summary>
//         /// returns list with all games.
//         /// not optimized.
//         /// auth is not required.
//         /// </summary>
//         /// <returns></returns>
//         [AllowAnonymous]
//         [HttpGet("all")]
//         [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GameGet>))]
//         public async Task<ActionResult<IEnumerable<GameGet>>> GetAllGamesAsync()
//         {
//             var games = await _service.GetAllAsync();
//             return Ok(games);
//         }
//
//         [AllowAnonymous]
//         [HttpGet("filter")]
//         [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GameGet>))]
//         [ProducesResponseType(StatusCodes.Status404NotFound)]
//         public async Task<ActionResult<IEnumerable<GameGet>>> Filter(int page = 1, int size = 10)
//         {
//             var models = await _service.Filter(page, size);
//
//             if (models != null)
//             {
//                 return Ok(models);
//             }
//
//             return NotFound();
//         }
//
//         [HttpGet("popularity")]
//         public async Task<ActionResult<IEnumerable<object>>> StatisticByGames(int days)
//         {
//             var stats = await _service.StatisticByGames(days);
//             return Ok(stats);
//         }
//
//         /// <summary>
//         /// creates a new game by given model.
//         /// model: GamePost.
//         /// admin token required.
//         /// </summary>
//         /// <param name="model"></param>
//         /// <returns></returns>
//         [Authorize(Policy = "MustBeAdmin")]
//         [HttpPost("add")]
//         [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GameGet))]
//         [ProducesResponseType(StatusCodes.Status400BadRequest)]
//         public async Task<ActionResult<GameGet>> PostGameAsync([FromBody] GamePost model)
//         {
//             await _service.AddAsync(model);
//             return Ok(model);
//         }
//
//         /// <summary>
//         /// allows to edit the game by it's id and its changed model.
//         /// model: GameUpdate.
//         /// admin token required.
//         /// </summary>
//         /// <param name="id"></param>
//         /// <param name="model"></param>
//         /// <returns></returns>
//         [Authorize(Policy = "MustBeAdmin")]
//         [HttpPut("{id}/edit")]
//         [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameGet))]
//         [ProducesResponseType(StatusCodes.Status400BadRequest)]
//         [ProducesResponseType(StatusCodes.Status404NotFound)]
//         public async Task<ActionResult<GameGet>> PutGameAsync(int id, [FromBody] GameUpdate model)
//         {
//             await _service.UpdateAsync(id, model);
//
//             return Ok();
//         }
//
//         /// <summary>
//         /// allows to delete the game by it's id.
//         /// admin token required.
//         /// </summary>
//         /// <param name="id"></param>
//         /// <returns></returns>
//         [Authorize(Policy = "MustBeAdmin")]
//         [HttpDelete("{id}/delete")]
//         [ProducesResponseType(StatusCodes.Status204NoContent)]
//         [ProducesResponseType(StatusCodes.Status404NotFound)]
//         public async Task<IActionResult> RemoveGameAsync(int id)
//         {
//             await _service.DeleteAsync(id);
//             return Ok();
//         }
//     }
// }
