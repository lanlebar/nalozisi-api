using API.DTOs.Error;
using API.DTOs.Torrent;
using API.DTOs.User;
using API.Services.AuthService;
using API.Services.TorrentService;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<ActionResult<string>> UploadTorrent([FromBody] UploadTorrentDto uploadTorrentDto)
        {
            try
            {
                var resp = await _torrentService.UploadTorrentAsync(uploadTorrentDto);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(new ErrorResponseDto { ErrorCode = 1, Message = e.Message });
            }
            catch (ConflictExceptionDto e)
            {
                return Conflict(new ErrorResponseDto { ErrorCode = 1, Message = e.Message });
            }
            catch (Exception e)
            {
                // Wild card error
                return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            }
        }

        [HttpPost("create")]
        public ActionResult CreateTorrent()
        {
            // TODO
            return BadRequest();
        }

        [HttpGet("scrape")]
        public async Task<ActionResult> ScrapeTorrents(string query, string category, int limit)
        {
            try
            {
                string result = await _torrentService.GetScrapedTorrentsAsync(query, category, limit);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}