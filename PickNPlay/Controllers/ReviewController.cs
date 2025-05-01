using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickNPlay.Helpers;
using PickNPlay.picknplay_bll.Models.Review;
using PickNPlay.picknplay_bll.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    /// <summary>
    /// Controller for handling review operations.
    /// </summary>
    [ApiController]
    [Route("reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewController"/> class.
        /// </summary>
        /// <param name="service">The review service.</param>
        public ReviewController(ReviewService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Gets a review by its ID.
        /// </summary>
        /// <param name="id">The ID of the review.</param>
        /// <returns>A review with the specified ID.</returns>
        /// <response code="200">Returns the review.</response>
        /// <response code="404">If the review is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReviewGet))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReviewGet>> GetReviewByIdAsync(int id)
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
        /// Gets all reviews.
        /// </summary>
        /// <returns>All reviews.</returns>
        /// <response code="200">Returns the list of reviews.</response>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReviewGet>))]
        public async Task<ActionResult<IEnumerable<ReviewGet>>> GetAllReviewsAsync()
        {
            var models = await service.GetAllAsync();
            return Ok(models);
        }

        /// <summary>
        /// Adds a new review.
        /// </summary>
        /// <param name="model">The review to add.</param>
        /// <returns>The added review.</returns>
        /// <response code="201">If the review is created successfully.</response>
        /// <response code="400">If the review creation fails.</response>
        [Authorize(Policy = "MustBeRegistered")]
        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ReviewGet))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PostReviewAsync([FromBody] ReviewPost model)
        {
            await service.AddAsync(model);
            return Ok();
        }

        /// <summary>
        /// Updates an existing review.
        /// </summary>
        /// <param name="id">The ID of the review to update.</param>
        /// <param name="model">The updated review details.</param>
        /// <returns>An updated review.</returns>
        /// <response code="200">If the review is updated successfully.</response>
        /// <response code="400">If the update fails.</response>
        /// <response code="404">If the review is not found.</response>
        [Authorize(Policy = "MustBeRegistered")]
        [HttpPut("{id}/edit")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReviewGet))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReviewGet>> PutReviewAsync(int id, [FromBody] ReviewUpdate model)
        {
            await service.UpdateAsync(id, model);
            return Ok();
        }

        /// <summary>
        /// Deletes a review.
        /// </summary>
        /// <param name="id">The ID of the review to delete.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the review is deleted successfully.</response>
        /// <response code="404">If the review is not found.</response>
        [Authorize(Policy = "MustBeRegistered")]
        [HttpDelete("{id}/delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RemoveReviewAsync(int id)
        {
            await service.DeleteAsync(id);
            return Ok();
        }
    }
}
