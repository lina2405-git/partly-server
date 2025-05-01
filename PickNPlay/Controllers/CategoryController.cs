using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickNPlay.picknplay_bll.Models.Category;
using PickNPlay.picknplay_bll.Services;
using Serilog;

namespace PickNPlay.Controllers
{

    [ApiController]
    [Route("categories")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService service;

        public CategoryController(CategoryService service)
        {
            this.service = service;
        }

        /// <summary>
        /// gets category by its categoryId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(CategoryGet))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CategoryGet>> GetCategoryByIdAsync(int id)
        {
            var model = await service.GetByIdAsync(id);
            if (model != null)
            {
                return Ok(model);
            }
            else
            {
                return NotFound();
            }
        }
        /// <summary>
        /// returns all categories(very expensive).
        /// auth is not required
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("all")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryGet>))]
        public async Task<ActionResult<IEnumerable<CategoryGet>>> GetAllCategoriesAsync()
        {
            var models = await service.GetAllAsync();
            return Ok(models);
        }

        [Authorize(Policy = "MustBeAdmin")]
        [HttpGet("popular")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPopularCategoryAsync(int term)
        {
            var model = await service.GetPopularCategory(term);
            if (model != null)
            {
                return Ok(model);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// returns categories by gameId
        /// auth is not required
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        //[AllowAnonymous]
        //[HttpGet]
        //[ProducesResponseType(200, Type = typeof(IEnumerable<CategoryGet>))]
        //[ProducesResponseType(404)]
        //public async Task<ActionResult<IEnumerable<CategoryGet>>> GetCategoriesByGameId([FromQuery] int gameId)
        //{
        //    var models = await service.GetCategoriesByGameId(gameId);
        //    if (models == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(models);
        //}

        /// <summary>
        /// creates a new category.
        /// model:CategoryPost
        /// admin rights are required.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Policy = "MustBeAdmin")]
        [HttpPost("add")]
        [ProducesResponseType(201, Type = typeof(CategoryGet))]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> PostCategoryAsync([FromBody] CategoryPost model)
        {
            try
            {
                await service.AddAsync(model);
                return Ok(model);

            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// edits category by its model and id.
        /// model:CategoryGet.
        /// admin rights are required
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Policy = "MustBeAdmin")]
        [HttpPut("{id}/edit")]
        [ProducesResponseType(200, Type = typeof(CategoryGet))]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CategoryGet>> PutCategoryAsync([FromBody] CategoryUpdate model, [FromQuery] int id)
        {
            await service.UpdateAsync(id, model);
            return Ok();
        }

        /// <summary>
        /// removes category by its id.
        /// admin rights are required
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Policy = "MustBeAdmin")]
        [HttpDelete("{id}/delete")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> RemoveCategoryAsync([FromQuery] int id)
        {
            await service.DeleteAsync(id);
            return Ok();
        }
    }
}
