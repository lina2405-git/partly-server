using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickNPlay.Helpers;
using PickNPlay.picknplay_bll.Models;
using PickNPlay.picknplay_bll.Models.Listing;
using PickNPlay.picknplay_bll.Services;

namespace PickNPlay.Controllers
{
    /// <summary>
    /// Controller for handling listing operations.
    /// </summary>
    [ApiController]
    [Route("listings")]
    public class ListingController : ControllerBase
    {
        private readonly ListingService service;
        /// <summary>
        /// Initializes a new instance of the <see cref="ListingController"/> class.
        /// </summary>
        /// <param name="service">The listing service.</param>
        public ListingController(ListingService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Gets a listing by its ID.
        /// </summary>
        /// <param name="id">The ID of the listing.</param>
        /// <returns>A listing with the specified ID.</returns>
        /// <response code="200">Returns the listing.</response>
        /// <response code="404">If the listing is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ListingGet))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ListingGet>> GetListingByIdAsync(int id, int reviewsPageNumber = 1, int reviewsPageSize = 10)
        {
            var model = await service.GetByIdCheckingFavouriteAsync(id, JWTTranslator.GetUserId(HttpContext), reviewsPageNumber, reviewsPageSize);
            if (model != null)
            {
                return Ok(model);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("amount")]
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<int>> GetListingNumberByFilters(int? idCategory = null, int? idGame = null, int? idPlatform = null)
        {
            var model = await service.GetListingNumberByFilters(idCategory, idGame, idPlatform);
            return Ok(model);

        }

        [HttpGet("created/month")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<object>> StatsByMonth()
        {
            var stats = service.GetStatsByMonthAsync();
            if (stats == null)
            {
                return NotFound();
            }

            return Ok(stats);
        }

        [HttpGet("created/year")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<object>> StatsByYear()
        {
            var stats = service.GetStatsByYearAsync();
            if (stats == null)
            {
                return NotFound();
            }

            return Ok(stats);
        }


        /// <summary>
        /// Gets all listings.
        /// </summary>
        /// <returns>All listings.</returns>
        /// <response code="200">Returns the list of listings.</response>
        [HttpGet("all")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ListingGet>))]
        public async Task<ActionResult<IEnumerable<ListingGet>>> GetAllListingsAsync()
        {
            var models = await service.GetAllAsync();
            return Ok(models);
        }

        /// <summary>
        /// Adds a new listing.
        /// </summary>
        /// <param name="model">The listing to add.</param>
        /// <returns>The added listing.</returns>
        /// <response code="201">If the listing is created successfully.</response>
        /// <response code="400">If the listing creation fails.</response>
        [Authorize]
        [HttpPost("add")]
        [ProducesResponseType(201, Type = typeof(ListingGet))]
        [ProducesResponseType(400)]
        public async Task<ActionResult> PostListingAsync([FromBody] ListingPost model)
        {
            try
            {

                if (JWTTranslator.GetUserRole(HttpContext).Value < 2)
                {
                    return BadRequest("You don't have rights (haha, woman)");
                }

                await service.AddAsync(model, JWTTranslator.GetUserId(HttpContext).Value);
                return Ok(model);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Updates an existing listing.
        /// </summary>
        /// <param name="model">The updated listing details.</param>
        /// <returns>An updated listing.</returns>
        /// <response code="204">If the listing is updated successfully.</response>
        /// <response code="400">If the update fails.</response>
        /// <response code="404">If the listing is not found.</response>
        [Authorize]
        [HttpPut("edit")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ListingGet>> PutListingAsync([FromBody] ListingUpdate model)
        {
            await service.UpdateAsync(JWTTranslator.GetUserId(HttpContext).Value, model);
            return StatusCode(204);
        }

        /// <summary>
        /// Deactivates a listing.
        /// </summary>
        /// <param name="id">The ID of the listing to delete.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the listing is deleted successfully.</response>
        /// <response code="404">If the listing is not found.</response>
        [Authorize]
        [HttpPut("{id}/deactivate")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeactivateListingAsync(int id)
        {
            await service.DeactivateAsync(id);
            return StatusCode(204);
        }

        /// <summary>
        /// Deactivates a listing.
        /// </summary>
        /// <param name="id">The ID of the listing to delete.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the listing is deleted successfully.</response>
        /// <response code="404">If the listing is not found.</response>
        [Authorize]
        [HttpPut("{id}/activate")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> ActivateListingAsync(int id)
        {
            await service.DeactivateAsync(id);
            return StatusCode(204);
        }

        /// <summary>
        /// Filters listings based on provided parameters.
        /// </summary>
        /// <remarks>
        /// `page` and `size` default to 1 and 30, respectively.
        /// </remarks>
        /// <param name="userId">The user ID to filter by.</param>
        /// <param name="gameId">The game ID to filter by.</param>
        /// <param name="categoryId">The category ID to filter by.</param>
        /// <param name="minPrice">The minimum price to filter by.</param>
        /// <param name="maxPrice">The maximum price to filter by.</param>
        /// <param name="page">The page number.</param>
        /// <param name="providerId">The provider ID.</param>
        /// <param name="keyword">The keyword to search in titles and descriptions</param>
        /// <param name="size">The page size.</param>
        /// <returns>Filtered listings.</returns>
        /// <response code="200">If listings are found.</response>
        /// <response code="404">If no listings are found.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpGet("filter")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ListingGet>>> FilterListings(
            int? userId,
            int? gameId,
            int? categoryId,
            decimal? minPrice,
            decimal? maxPrice,
            int? providerId,
            string? keyword,
            int page = 1,
            int size = 30)
        {
            try
            {
                int? currentUserId = JWTTranslator.GetUserId(HttpContext);

                var models = await service.Filter(userId, gameId, categoryId, minPrice,
                    maxPrice, providerId, keyword, currentUserId, page, size);

                if (models.Count() > 0)
                {
                    return Ok(models);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return StatusCode(418, e.Message);
            }
        }

        [HttpGet("pagination")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PaginationInfo>> GetPaginationInfo(int page = 1, int size = 30)
        {
            PaginationInfo info = await service.GetPaginationInfo(page, size);  
            return Ok(info);
        }
    }
}
