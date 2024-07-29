using Microsoft.AspNetCore.Mvc;
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
        public UserProfileController(IProfileService profileService, ILogger<UserProfileController> logger)
        {
            _profileService = profileService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new user profile.
        /// </summary>
        /// <param name="RegisterUserProfileDTO">The user registration userprofile data transfer object.</param>
        /// <returns>A message indicating the result of the user profile.</returns>
        [HttpPost("CreateProfile")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseModel>> Register([FromBody] RegisterUserProfileDTO profiledto)
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
        public async Task<ActionResult<ResponseModel>> Register([FromBody] AddUserProfileDTO addprofiledto)
        {
            try
            {
                foreach (var header in HttpContext.Request.Headers)
                {
                    _logger.LogInformation($"Header: {header.Key}, Value: {header.Value}");
                }
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
                    return BadRequest(new ResponseModel { HasError=true,ErrorMessage = "UserId claim is missing." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add Profile For the User {UserID}", addprofiledto.UserId);
                return BadRequest(new ResponseModel());
            }
        }
    }
}
