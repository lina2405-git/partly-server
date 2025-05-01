using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickNPlay.Controllers;
using PickNPlay.Exceptions;
using PickNPlay.Helpers;
using PickNPlay.Helpers.Senders;
using PickNPlay.picknplay_bll.Models.User;
using PickNPlay.picknplay_bll.Services;

namespace YourNamespaceHere.Controllers
{
    /// <summary>
    /// for all actions in this controller admin rights are required
    /// </summary>
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly UserService service;
        private readonly IConfiguration configuration;
        private readonly MailSender sender;
        public UserController(UserService userService, IConfiguration configuration)
        {
            service = userService;
            this.configuration = configuration;

            sender = new(configuration);
        }

        /// <summary>
        /// returns brief info about user by its userId.
        /// admin rights are required.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Policy = "MustBeRegistered")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(UserGet))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserGet>> GetCurrentUser()
        {
            var user = await service.GetByIdAsync(JWTTranslator.GetUserId(HttpContext).Value);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Policy = "MustBeRegistered")]
        [HttpGet("id")]
        [ProducesResponseType(200, Type = typeof(UserGet))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserGet>> GetUserByIdAsync(int userId)
        {
            var user = await service.GetByIdAsync(userId);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound();
            }
        }
        /// <summary>
        /// returns all users
        /// admin rights are required
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "MustBeAdmin")]
        [Authorize(Policy = "MustBeModer")]
        [HttpGet("all")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserGet>))]
        public async Task<ActionResult<IEnumerable<UserGet>>> GetAllUsersAsync()
        {
            var users = await service.GetAllAsync();
            return Ok(users);
        }

        [Authorize(Policy = "MustBeAdmin")]
        [HttpGet("mostProfitable")]
        public async Task<ActionResult<IEnumerable<object>>> GetTopProfitableUsers(int term)
        {
            var users = await service.GetMostProfitableUsersAsync(term);
            if (users != null)
            {
                return Ok(users);
            }

            return NotFound();
        }


        [Authorize(Policy = "MustBeAdmin")]
        [HttpGet("newest")]
        public async Task<ActionResult<IEnumerable<UserReallyBriefInfoWithoutReviews>>> GetNewestUsers(int take)
        {
            var users = await service.GetLatestUsers(take);
            if (users.Count() != 0)
            {
                return Ok(users);
            }
            return NotFound();
        }

        [HttpGet("created/month")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<object>> StatsByMonth()
        {
            var stats = service.GetAmountOfCreatedByMonth();
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
            var stats = service.GetAmountOfCreatedByYear();
            if (stats == null)
            {
                return NotFound();
            }

            return Ok(stats);
        }

        [HttpGet("creators/month")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<object>> CreatedForMonth()
        {
            var stats = service.GetSuccessfulCreatorsByMonth();
            if (stats == null)
            {
                return NotFound();
            }

            return Ok(stats);
        }

        [HttpGet("creators/year")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<object>> CreatedForYear()
        {
            var stats = service.GetSuccessfulCreatorsByYear();
            if (stats == null)
            {
                return NotFound();
            }

            return Ok(stats);
        }


        /// <summary>
        /// edits user by its model and userId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Policy = "MustBeAdmin")]
        [HttpPut("{id}/edit")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutUserAsync(int id, [FromBody] UserUpdate model)
        {
            var existingUser = await service.GetByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            await service.UpdateAsync(id, model);
            return NoContent();
        }

        [Authorize]
        [HttpPut("change/password")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        public async Task<ActionResult> ChangePassword(string oldPassword, string newPassword)
        {
            try
            {
                bool isUpdated = await service.ChangePassword(JWTTranslator.GetUserId(HttpContext).Value, oldPassword, newPassword);
                if (isUpdated)
                {
                    return NoContent();
                }

                return Forbid();
            }
            catch (DALException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [Authorize]
        [HttpPut("change/nickname")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        public async Task<ActionResult> ChangeNickname(string nickname)
        {
            try
            {
                bool isUpdated = await service.ChangeNickname(JWTTranslator.GetUserId(HttpContext).Value, nickname);
                if (isUpdated)
                {
                    return NoContent();
                }

                return Forbid();
            }
            catch (DALException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [Authorize]
        [HttpPut("change/image")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        public async Task<ActionResult> ChangeProfileImage(IFormFile image)
        {
            try
            {
                ImageController imageController = new(configuration);
                var imageUrl = await imageController.UploadImageToCloudNoActionResult(image);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    bool isUpdated = await service.ChangePhoto(JWTTranslator.GetUserId(HttpContext).Value, imageUrl);
                    if (isUpdated)
                    {
                        return NoContent();
                    }

                    return Forbid();
                }

                return NotFound();
            }
            catch (DALException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [Authorize]
        [HttpPut("change/description")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        public async Task<ActionResult> ChangeProfileDescription(string content)
        {
            try
            {
                if (!string.IsNullOrEmpty(content))
                {
                    bool isUpdated = await service.ChangeDescription(content, JWTTranslator.GetUserId(HttpContext).Value);
                    if (isUpdated)
                    {
                        return NoContent();
                    }
                    return NotFound();
                }

                return BadRequest();
            }
            catch (DALException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("password/reset/send")]
        public async Task<ActionResult> ResetPasswordEmailSend(string email)
        {
            try
            {
                var resetTokenAndEmail = await service.GetPasswordResetTokenAndUsername(email);

                sender.SendResetPasswordEmail(email, resetTokenAndEmail.Item1, resetTokenAndEmail.Item2);

                return Ok();
            }
            catch (DALException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpPost("password/reset/confirm")]
        public async Task<ActionResult> ResetPasswordEmailConfirm([FromBody] ResetModel resetModel)
        {
            try
            {
                var isSuccessed = await service.ResetPassword(resetModel);
                if (isSuccessed)
                {
                    return Ok();
                }
                return BadRequest("Token expired");
            }
            catch (DALException e)
            {
                return NotFound(e.Message);
            }
        }
        /// <summary>
        /// removes user by its userId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Policy = "MustBeAdmin")]
        [HttpDelete("{id}/delete")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveUserAsync(int id)
        {
            var existingUser = await service.GetByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            await service.DeleteAsync(id);
            return NoContent();
        }

        [Authorize]
        [HttpPost("email/verification/send")]
        public async Task<IActionResult> SendVerificationEmail()
        {
            try
            {
                var userId = JWTTranslator.GetUserId(HttpContext).Value;
                var token = await service.GetEmailVerifToken(userId);
                var user = await service.GetByIdAsync(userId);

                MailSender sender = new(configuration);
                await sender.SendVerificationEmailAsync(user.Email, token, user.Username);

            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }

            return NoContent();

        }

        //а это будет эндпоинтом конкретно для верификации 

        [HttpPost("email/verification/confirm")]
        public async Task<IActionResult> VerifyEmail(string verificationToken)
        {
            try
            {
                var isVerified = await service.VerifyEmail(verificationToken);
                if (isVerified)
                {
                    return NoContent();
                }

                return Forbid("Wrong token");
            }
            catch (ArgumentNullException e)
            {
                return BadRequest(e.Message);
            }
            catch (InvalidOperationException)
            {
                return Conflict("Email is already confirmed");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
