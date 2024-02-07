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
        private readonly ISearchService _searchService;
        private readonly IConfiguration _configuration;

        // Constructor
        public SearchController(ISearchService searhService, IConfiguration configuration)
        {
            _searchService = searhService;
            _configuration = configuration;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult> Search([FromQuery] SearchRequestDto searchRequsetDto)
        {
            try
            {
                
                // Query is the only necceaary parameter
                if (searchRequsetDto.Query == null)
                {
                    return BadRequest(new ErrorResponseDto { ErrorCode = 1, Message = "Izraz za iskanje je obvezen!" });
                }

                // If category and source is null, set to all (wasn't specified with request)
                searchRequsetDto.Category ??= "All";
                searchRequsetDto.Source ??= "All";

                // Check if provider exists
                if (!_searchService.GetAllSupportedProviders().Contains(searchRequsetDto.Source))
                {
                    return BadRequest(new ErrorResponseDto { ErrorCode = 1, Message = "Neveljaven vnos vira!" });
                }

                // Check if category for the provider exists
                if (!_searchService.GetProviderCategories(searchRequsetDto.Source).Contains(searchRequsetDto.Category))
                {
                    return BadRequest(new ErrorResponseDto { ErrorCode = 1, Message = "Neveljaven vnos kategorije!" });
                }

                //return Ok();

                //// Convert category to enum and check existence of it
                //Enums.TorrentCategory torrentCategory;
                //if (!Enum.TryParse(searchRequsetDto.Category, true, out torrentCategory))
                //{
                //    return BadRequest(new ErrorResponseDto { ErrorCode = 1, Message = "Neveljaven vnos kategorije!" });
                //}

                //// Convert source to enum and check existence of it
                //TorrentSource torrentSource;
                //if (!Enum.TryParse(searchRequsetDto.Source, true, out torrentSource))
                //{
                //    return BadRequest(new ErrorResponseDto { ErrorCode = 1, Message = "Neveljaven vnos vira!" });
                //}

                //// Prepare new internalSearchRequestDto
                //InternalSearchRequestDto internalSearchRequestDto = new InternalSearchRequestDto
                //{
                //    Query = searchRequsetDto.Query,
                //    Category = torrentCategory,
                //    Source = torrentSource
                //};
                
                var result = await _searchService.GetScrapedTorrentsAsync(searchRequsetDto);
                return Ok(result);
            }
            catch (NotFoundException)
            {
                return StatusCode(404);
            }
            catch (Exception e)
            {
                return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            }
        }

        [HttpGet("allProviderCategories"), Authorize]
        public ActionResult GetProviderCategories()
        {
            return Ok(_searchService.GetAllProviderCategories());
        }
    }
}
