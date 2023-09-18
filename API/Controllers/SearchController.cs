using API.Services.AuthService;
using API.Services.TorrentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace API.Controllers
{
    [Route("api/search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        // Fields
        private readonly IAuthService _authService;
        private readonly ITorrentService _torrentService;

        // Constructor
        public SearchController(IAuthService authService, ITorrentService torrentService)
        {
            _authService = authService;
            _torrentService = torrentService;
        }

        [HttpGet]
        public async Task<ActionResult> Search(
            [Required] string query
        )
        {
            try
            {
                //// Convert parameters to enum
                //Enums.TorrentCategory torrentCategory;
                //if (!Enum.TryParse(category, true, out torrentCategory))
                //{
                //    return BadRequest(new ErrorResponseDto { ErrorCode = 1, Message = "Invalid category" });
                //}

                string result = await _torrentService.GetScrapedTorrentsAsync(query, API.Enums.TorrentCategory.All, 10);
                return Ok(new {result});
            }
            catch (Exception e)
            {
                return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            }
        }


        //[HttpGet]
        //public async ActionResult<string> Search(
        //    [Required] string query,
        //    [Required] string category = "All",
        //    [Required] int limit = 10
        //)
        //{
        //    return Ok("d");
        //    // Get search results from all avaliable sources
        //    string scrapedResults = _torrentService.GetScrapedTorrentsAsync(query, Enums.TorrentCategory.All, 10).Result;
        //    return Ok("");
        //}
    }
}
