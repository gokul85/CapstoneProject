using Microsoft.AspNetCore.Mvc;
using PremiumService.Interfaces;
using PremiumService.Models.DTOs;
using System.Diagnostics.CodeAnalysis;

namespace PremiumService.Controllers
{
    [ExcludeFromCodeCoverage]
    [Route("/api/premium/")]
    [ApiController]
    public class PremiumUserController : ControllerBase
    {
        private readonly IPremiumUserService _premiumservice;
        private readonly ILogger<PremiumUserController> _logger;
        public PremiumUserController(IPremiumUserService premiumservice, ILogger<PremiumUserController> logger)
        {
            _premiumservice = premiumservice;
            _logger = logger;
        }

        /// <summary>
        /// Subscribe to Premium.
        /// </summary>
        /// <param name="SubscribePremiumDTO">Subscribe Payment data transfer object.</param>
        /// <returns>A message indicating the result of the payment and subscription.</returns>
        [HttpPost("Subscribe")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseModel>> Subscribe([FromBody] SubscribePremiumDTO subscribePremiumDTO)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("UserId", out var userIdHeader))
                {
                    int userId = int.Parse(userIdHeader.FirstOrDefault());
                    subscribePremiumDTO.UserId = userId;
                    ResponseModel result = await _premiumservice.SubscribePremium(subscribePremiumDTO);
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
                _logger.LogError(ex, "Premium Subscription For the User {UserID}", subscribePremiumDTO.UserId);
                return BadRequest(new ResponseModel());
            }
        }


        /// <summary>
        /// Check Contact View.
        /// </summary>
        /// <param name="profileid">Profile ID.</param>
        /// <returns>Contact Details or Remaining Count.</returns>
        [HttpGet("CheckViewContact")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseModel>> CheckViewContact(int profileid)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("UserId", out var userIdHeader))
                {
                    int userId = int.Parse(userIdHeader.FirstOrDefault());
                    ResponseModel result = await _premiumservice.CheckContactView(userId,profileid);
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
                _logger.LogError(ex, "Check View Contact For the User Profile {UserProfile}", profileid);
                return BadRequest(new ResponseModel() { ErrorMessage = ex.Message});
            }
        }

        /// <summary>
        /// Contact View.
        /// </summary>
        /// <param name="profileId">Profile ID.</param>
        /// <returns>Contact Details.</returns>
        [HttpPost("ViewContact")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseModel>> ViewContact([FromBody] NewContactViewDTO newContactViewDTO)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("UserId", out var userIdHeader))
                {
                    int userId = int.Parse(userIdHeader.FirstOrDefault());
                    ResponseModel result = await _premiumservice.ContactView(userId, newContactViewDTO.ProfileId);
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
                _logger.LogError(ex, "View Contact For the User Profile {UserProfile}", newContactViewDTO.ProfileId);
                return BadRequest(new ResponseModel());
            }
        }
    }
}
