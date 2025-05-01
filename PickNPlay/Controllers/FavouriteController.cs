using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickNPlay.Exceptions;
using PickNPlay.Helpers;
using PickNPlay.picknplay_bll.Models.Favourite;

namespace PickNPlay.Controllers
{
    [ApiController]
    [Route("favourite")]
    public class FavouriteController : ControllerBase
    {
        private readonly FavouriteService service;

        public FavouriteController(FavouriteService service)
        {
            this.service = service;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FavouriteGet>>> GetFavouritesByUserId()
        {
            var models = await service.GetByUserId(JWTTranslator.GetUserId(HttpContext) ?? throw new Exception("Something wrong with token. It is rather null or empty"));

            if (models == null)
            {
                return NotFound();
            }

            return Ok(models);

        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<FavouriteGet>>> PostFavourite(int listingId)
        {
            try
            {
                var models = await service.AddAsync(new() { ListingId = listingId, UserId = JWTTranslator.GetUserId(HttpContext) ?? throw new Exception("Something wrong with token. It is rather null or empty") });
                return Ok(models);
            }
            catch (DALException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult<IEnumerable<FavouriteGet>>> DeleteFavourite(int listingId)
        {
            try
            {
                var models = await service.DeleteAsync(listingId, JWTTranslator.GetUserId(HttpContext) ?? throw new Exception("Something wrong with token. It is rather null or empty"));
                return Ok(models);
            }
            catch (DALException e)
            {
                return BadRequest(e);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
