using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PickNPlay.Exceptions;
using PickNPlay.Helpers.Senders;
using PickNPlay.picknplay_bll.Models.User;
using PickNPlay.picknplay_bll.Services;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace PickNPlay.Controllers
{
    [ApiController]
    [Route("authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserService _service;
        private readonly IConfiguration _configuration;


        public AuthenticationController(
            UserService service,
            IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;

        }

        public class AuthenticationModel
        {
            public string? UserName { get; set; }
            public string? Password { get; set; }
        }

        public class RegistrationModel 
        {
            [Required]
            public string UserName { get; set; }

            [Required]
            public string Password { get; set; }
            [Required, Compare("Password")]
            public string ConfirmPassword { get; set; }

            [Required, EmailAddress]
            public string Email { get; set; }

        }
        public class TokenResponse
        {
            public string Token { get; set; }
            public DateTime Until { get; set; }
        }

        /// <summary>
        /// allows user to authenticate 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<TokenResponse>> AuthenticateAsync(AuthenticationModel body)
        {
            try
            {
                var user = await ValidateUserCredentialsAsync(body.UserName, body.Password);

                if (user == null)
                {
                    return Unauthorized();
                }

                var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:Secret"]));
                var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claimsForToken = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("created", user.CreatedAt.ToString()),
                    new Claim("userRole",user.UserRoleId.ToString())
                };

                var jwtToken = new JwtSecurityToken(
                    issuer: _configuration["Authentication:Issuer"],
                    audience: _configuration["Authentication:Audience"],
                    claims: claimsForToken,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: signingCredentials);

                var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtToken);

                return Ok(new TokenResponse
                {
                    Token = tokenToReturn,
                    Until = jwtToken.ValidTo
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// more preferred than "POST users/add"
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(202,Type = typeof(TokenResponse))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(418)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]

        public async Task<ActionResult<TokenResponse>> RegisterNewUser([FromBody] UserPost model)
        {
            try
            {
                var created =  _service.AddAsync(model);

                var token =  AuthenticateAsync(new ()
                {
                    Password = created.Result.PasswordHash,
                    UserName = created.Result.Username,
                });

                await Task.WhenAll(created, token);

                var verifToken = await _service.GetEmailVerifToken(created.Result.UserId);

                MailSender sender = new(_configuration);
                await sender.SendVerificationEmailAsync(created.Result.Email, verifToken, created.Result.Username);

                return token.Result;

            }
            catch (ServiceException)
            {
                return Conflict(new { error = "User with these email and/or username is already registered." });
            }
            catch (DALException)
            {
                return BadRequest(model);
            }
        }


        private async Task<UserGet> ValidateUserCredentialsAsync(string? userName, string? password)
        {
            var user = await _service.GetByUsernameAndPasswordAsync(userName, password);
            if (user == null)
            {
                return null;
            }
            return user;
        }
    }
}
