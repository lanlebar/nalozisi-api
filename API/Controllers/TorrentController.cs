using API.DTOs.Error;
using API.DTOs.Torrent;
using API.DTOs.User;
using API.Services.AuthService;
using API.Services.TorrentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    [Route("api/torrent")]
    [ApiController]
    public class TorrentController : ControllerBase
    {
        // Fields
        private readonly IAuthService _authService;
        private readonly ITorrentService _torrentService;

        // Constructor
        public TorrentController(IAuthService authService, ITorrentService torrentService)
        {
            _authService = authService;
            _torrentService = torrentService;
        }

        [HttpPost("upload"), Authorize]
        public ActionResult UploadTorrent([FromBody] UploadTorrentDto uploadTorrentDto)
        {
            // TODO
            return BadRequest(new ErrorResponseDto { ErrorCode = 1, Message = "Not implemented" });

            //try
            //{
            //    var resp = await _torrentService.UploadTorrentAsync(uploadTorrentDto);
            //    return Ok();
            //}
            //catch (ArgumentException e)
            //{
            //    return BadRequest(new ErrorResponseDto { ErrorCode = 1, Message = e.Message });
            //}
            //catch (ConflictExceptionDto e)
            //{
            //    return Conflict(new ErrorResponseDto { ErrorCode = 1, Message = e.Message });
            //}
            //catch (Exception e)
            //{
            //    // Wild card error
            //    return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            //}
        }

        [HttpPost("create"), Authorize]
        public ActionResult CreateTorrent()
        {
            // TODO
            return BadRequest(new ErrorResponseDto { ErrorCode = 1, Message = "Not implemented"});
        }

        [HttpGet("scrape")]
        public async Task<ActionResult> ScrapeTorrents(
            [Required] string query,
            [Required] string category = "All",
            [Required] int limit = 10
        )
        {
            try
            {
                // Convert parameters to enum
                Enums.TorrentCategory torrentCategory;
                if (!Enum.TryParse(category, true, out torrentCategory))
                {
                    return BadRequest(new ErrorResponseDto { ErrorCode = 1, Message = "Invalid category"});
                }

                string result = await _torrentService.GetScrapedTorrentsAsync(query, torrentCategory, limit);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            }
        }
    }
}