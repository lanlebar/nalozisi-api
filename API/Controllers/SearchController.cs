using API.DTOs.Search;
using API.DTOs.TorrentScrape;
using API.Services.AuthService;
using API.Services.SearchService;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        // Fields
        private readonly IAuthService _authService;
        private readonly ISearchService _searchService;
        private readonly IConfiguration _configuration;

        // Constructor
        public SearchController(IAuthService authService, ISearchService searhService, IConfiguration configuration)
        {
            _authService = authService;
            _searchService = searhService;
            _configuration = configuration;
        }

        [HttpGet, AllowAnonymous]
        public async Task<ActionResult> Search([FromQuery] SearchRequestDto searchRequsetDto)
        {
            try
            {
                // Query is the only necceaary parameter
                if (searchRequsetDto.Query == null)
                {
                    return BadRequest(new ErrorResponseDto { ErrorCode = 1, Message = "Query is required" });
                }
                searchRequsetDto.Source ??= "All";
                searchRequsetDto.Category ??= "All";
                if (_configuration["InternalApiSettings:BaseScrapeSearchLimit"] == null)
                {
                    // Safer solution
                    searchRequsetDto.Limit = "20";
                }
                searchRequsetDto.Limit ??= _configuration["InternalApiSettings:BaseScrapeSearchLimit"];


                // Convert category to enum and check existance of it
                if (!Enum.TryParse(searchRequsetDto.Category, true, out Enums.TorrentCategory torrentCategory))
                {
                    return BadRequest(new ErrorResponseDto { ErrorCode = 1, Message = "Invalid category" });
                }
                // Convert source to enum and check existance of it
                TorrentSource torrentSource;
                if (!Enum.TryParse(searchRequsetDto.Source, true, out torrentSource))
                {
                    return BadRequest(new ErrorResponseDto { ErrorCode = 1, Message = "Invalid source" });
                }


                ScrapedTorrentsResponseDto result = await _searchService.GetScrapedTorrentsAsync(searchRequsetDto);
                return Ok(result);
            }
            catch (NotFoundExceptionDto e)
            {
                return StatusCode(404);
            }
            catch (Exception e)
            {
                return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            }
        }
    }
}
