// using Microsoft.AspNetCore.Mvc;
// using PickNPlay.picknplay_bll.Models.Provider;
// using PickNPlay.picknplay_bll.Services;
// using Stripe;
//
// namespace PickNPlay.Controllers
// {
//     // [ApiController]
//     // [Route("provider")]
//     public class ProviderController : ControllerBase
//     {
//         private readonly ProviderService service;
//
//         public ProviderController(ProviderService service)
//         {
//             this.service = service;
//         }
//
//         [HttpGet("all")]
//         public async Task<ActionResult<IEnumerable<ProviderGet>>> GetAllProvidersAsync()
//         {
//             var models = await service.GetAllAsync();
//             return Ok(models);
//         }
//
//         [HttpGet()]
//         public async Task<ActionResult<ProviderGet>> GetProviderByIdAsync([FromQuery] int id)
//         {
//             var model = await service.GetByIdAsync(id);
//             if (model != null)
//             {
//                 return Ok(model);
//             }
//             return NotFound();
//         }
//
//         [HttpPost]
//         public async Task<ActionResult> AddProviderAsync([FromBody] ProviderPost model)
//         {
//             try
//             {
//                 await service.AddAsync(model);
//                 return Ok("Created");
//             }
//             catch (Exception)
//             {
//                 throw;
//             }
//         }
//
//         [HttpPut("url")]
//         public async Task<ActionResult> SimpleUrlEdit([FromQuery] int id, [FromQuery] string newUrl)
//         {
//             try
//             {
//                 await service.EditUrl(id, newUrl);
//                 return Ok();
//             }
//             catch (Exception e)
//             {
//
//                 return BadRequest(e.Message);
//             }
//         }
//
//         [HttpGet("filter")]
//         public async Task<ActionResult<IEnumerable<ProviderGet>>> Filter(int page = 1, int size = 10)
//         {
//             var models = await service.Filter(page, size);
//             
//             if (models.Count() > 0)
//             {
//                 return Ok(models);
//             }
//             
//             return NotFound();
//         }
//
//         [HttpGet("popularity")]
//         public async Task<ActionResult<IEnumerable<object>>> StatisticByProviders(int days)
//         {
//             var stats = await service.GetPopularProvider(days);
//             return Ok(stats);
//         }
//
//     }
// }
