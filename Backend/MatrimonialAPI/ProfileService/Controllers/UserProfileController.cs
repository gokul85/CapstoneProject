using Microsoft.AspNetCore.Mvc;
using ProfileService.AsyncDataServices;
using ProfileService.Interfaces;
using ProfileService.Models.DTOs;
using System.Security.Claims;

namespace ProfileService.Controllers
{
    [Route("/api/profile/")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly ILogger<UserProfileController> _logger;
        private readonly RabbitMQPublisher _rabbitMQPublisher;
        public UserProfileController(IProfileService profileService, ILogger<UserProfileController> logger, RabbitMQPublisher rabbitMQPublisher)
        {
            _profileService = profileService;
            _logger = logger;
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        /// <summary>
        /// Creates a new user profile.
        /// </summary>
        /// <param name="RegisterUserProfileDTO">The user registration userprofile data transfer object.</param>
        /// <returns>A message indicating the result of the user profile.</returns>
        [HttpPost("CreateProfile")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseModel>> CreateProfile([FromBody] RegisterUserProfileDTO profiledto)
        {
            try
            {
                ResponseModel result = await _profileService.CreateUserProfile(profiledto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Profile Registration For the User {UserID}", profiledto.UserId);
                return BadRequest(new ResponseModel());
            }
        }


        /// <summary>
        /// Add user profile details.
        /// </summary>
        /// <param name="AddUserProfileDTO">The user add userprofile data transfer object.</param>
        /// <returns>A message indicating the result of the user profile.</returns>
        [HttpPost("AddProfile")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseModel>> AddProfile([FromForm] AddUserProfileDTO addprofiledto)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("UserId", out var userIdHeader))
                {
                    int userId = int.Parse(userIdHeader.FirstOrDefault());
                    addprofiledto.UserId = userId;
                    ResponseModel result = await _profileService.AddUserProfile(addprofiledto);
                    return Ok(result);
                }
                else
                {
                    _logger.LogWarning("UserId claim is missing in the request headers.");
                    return BadRequest(new ResponseModel { HasError = true, ErrorMessage = "UserId claim is missing." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add Profile For the User {UserID}", addprofiledto.UserId);
                return BadRequest(new ResponseModel() { HasError=true,ErrorMessage=ex.Message});
            }
        }

        [HttpGet("GetUserContactDetails")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseModel>> GetUserContactDetails(int id)
        {
            try
            {
                ResponseModel result = await _profileService.GetUserContactDetails(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "View Contact Details Error for User Profile ", id);
                return BadRequest(new ResponseModel() { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}
