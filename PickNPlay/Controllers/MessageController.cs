using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickNPlay.Exceptions;
using PickNPlay.Helpers;
using PickNPlay.picknplay_bll.Models.Message;
using PickNPlay.picknplay_bll.Services;

namespace PickNPlay.Controllers
{
    /// <summary>
    /// Controller for handling message operations.
    /// </summary>
    [ApiController]
    [Route("messages")]
    public class MessageController : ControllerBase
    {
        private readonly MessageService _messageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageController"/> class.
        /// </summary>
        /// <param name="messageService">The message service.</param>
        public MessageController(MessageService messageService)
        {
            _messageService = messageService;
        }

        /// <summary>
        /// returns chats by userId
        /// </summary>
        /// <param name="forSeller">if you want to get chats for listings that this user sells</param>
        /// <returns></returns>
        /// <exception cref="DALException"></exception>
        [Authorize(Policy = "MustBeRegistered")]
        [HttpGet("chats")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ListingWithLastMessage>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ListingWithLastMessage>>> GetChatsByUserIdAsync(bool forSeller)
        {
            try
            {
                var userId = JWTTranslator.GetUserId(HttpContext);

                if (!userId.HasValue)
                {
                    return BadRequest("User ID not found in token");
                }

                if (forSeller)
                {
                    var userRole = JWTTranslator.GetUserRole(HttpContext);
                    if (userRole < 2)
                    {
                        return StatusCode(403, "You are not seller");
                    }

                    var models = await _messageService.GetChatsForSeller(userId.Value);
                    if (models.Any())
                    {
                        return Ok(models);
                    }
                    return NotFound(Enumerable.Empty<MessageGet>());
                }
                else
                {
                    var models = await _messageService.GetChatsForBuyer(userId.Value);
                    if (models.Any())
                    {
                        return Ok(models);
                    }
                    return NotFound(Enumerable.Empty<MessageGet>());
                }
            }
            catch (DALException e)
            {
                return NotFound(e.Message);
            }
            catch
            {
                throw;
            }
        }

        [Authorize(Policy = "MustBeRegistered")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MessageGet>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<MessageGet>>> GetMessagesByListingIdAsync(int listingId, int senderId, int receiverId)
        {
            var userId = JWTTranslator.GetUserId(HttpContext);

            if (!userId.HasValue)
            {
                return BadRequest("User ID not found in token");
            }

            var messages = await _messageService.GetMessagesByListingIdAndUsers(listingId, senderId, receiverId, userId.Value);

            if (messages.Any())
            {
                return Ok(messages);
            }
            else
            {
                return NotFound("Messages for this chat were not found");
            }
        }



        /// <summary>
        /// Gets all messages.
        /// </summary>
        /// <returns>All messages.</returns>
        /// <response code="200">Returns the list of messages.</response>
        [Authorize(Policy = "MustBeAdmin")]
        [Authorize(Policy = "MustBeModer")]
        [HttpGet("all")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MessageGet>))]
        public async Task<ActionResult<IEnumerable<MessageGet>>> GetAllMessagesAsync()
        {
            var messages = await _messageService.GetAllAsync();
            return Ok(messages);
        }

        /// <summary>
        /// Adds a new message.
        /// </summary>
        /// <param name="model">The message to add.</param>
        /// <returns>The added message.</returns>
        /// <response code="201">If the message is created successfully.</response>
        /// <response code="400">If the message creation fails.</response>
        [Authorize(Policy = "MustBeRegistered")]
        [HttpPost("add")]
        [ProducesResponseType(201, Type = typeof(MessageGet))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<MessageGet>> PostMessageAsync([FromBody] MessagePost model)
        {
            try
            {
                var userId = JWTTranslator.GetUserId(HttpContext);

                if (!userId.HasValue)
                {
                    return BadRequest("Sender was not found");
                }

                var models = await _messageService.AddAsync(model, userId.Value);

                if (models.Any())
                {
                    return Ok(models);
                }

                return NotFound();
            }
            catch (ArgumentException)
            {
                return BadRequest("You are trying to send message to yourself. (eblan?)");
            }
        }

        /// <summary>
        /// Updates an existing message.
        /// </summary>
        /// <param name="id">The ID of the message to update.</param>
        /// <param name="model">The updated message details.</param>
        /// <returns>An updated message.</returns>
        /// <response code="200">If the message is updated successfully.</response>
        /// <response code="400">If the update fails.</response>
        /// <response code="404">If the message is not found.</response>
        [Authorize(Policy = "MustBeRegistered")]
        [Authorize(Policy = "MustBeModer")]
        [Authorize(Policy = "MustBeAdmin")]
        [Authorize(Policy = "MustBeDealer")]
        [HttpPut("{id}/edit")]
        [ProducesResponseType(200, Type = typeof(MessageGet))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<MessageGet>> PutMessageAsync(int id, [FromBody] MessageUpdate model)
        {
            await _messageService.UpdateAsync(id, model);
            return Ok();
        }

        /// <summary>
        /// Deletes a message.
        /// </summary>
        /// <param name="id">The ID of the message to delete.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the message is deleted successfully.</response>
        /// <response code="404">If the message is not found.</response>
        [Authorize(Policy = "MustBeRegistered")]
        [Authorize(Policy = "MustBeModer")]
        [Authorize(Policy = "MustBeAdmin")]
        [Authorize(Policy = "MustBeDealer")]
        [HttpDelete("{id}/delete")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteMessageAsync(int id)
        {
            await _messageService.DeleteAsync(id);
            return Ok();
        }
    }
}
