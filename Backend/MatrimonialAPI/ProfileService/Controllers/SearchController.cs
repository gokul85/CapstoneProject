using Microsoft.AspNetCore.Mvc;
using ProfileService.Interfaces;
using ProfileService.Models.DTOs;
using System.Diagnostics.CodeAnalysis;

namespace ProfileService.Controllers
{
    [ExcludeFromCodeCoverage]
    [Route("/api/search/")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchservice;
        private readonly ILogger<UserProfileController> _logger;
        public SearchController(ISearchService searchservice, ILogger<UserProfileController> logger)
        {
            _searchservice = searchservice;
            _logger = logger;
        }

        /// <summary>
        /// Search.
        /// </summary>
        /// <param name=""></param>
        /// <returns>searched user profile</returns>
        [HttpPost("SearchProfiles")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseModel>> CreateProfile([FromBody] SearchCriteriaDTO searchCriteriaDTO)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("UserId", out var userIdHeader))
                {
                    int userId = int.Parse(userIdHeader.FirstOrDefault());
                    ResponseModel result = await _searchservice.SearchProfiles(searchCriteriaDTO,userId);
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
                _logger.LogError(ex, "Can't able to Search");
                return BadRequest(new ResponseModel());
            }
        }


        [HttpGet("ViewProfile")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseModel>> CreateProfile(int profileid)
        {
            try
            {
                    ResponseModel result = await _searchservice.ViewProfile(profileid);
                    return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Can't able to Search");
                return BadRequest(new ResponseModel());
            }
        }
    }
}
