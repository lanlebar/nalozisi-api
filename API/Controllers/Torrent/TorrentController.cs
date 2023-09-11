using API.DTOs.Error;
using API.DTOs.Torrent;
using API.DTOs.User;
using API.Services.AuthService;
using API.Services.TorrentService;

namespace API.Controllers.Torrent
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

        [HttpPost("upload")]
        public async Task<ActionResult<string>> UploadTorrent([FromBody] UploadTorrentDto uploadTorrentDto)
        {
            try
            {

                return Ok();
            }
            catch (ArgumentException e)
            {
                // Missing fields
                return BadRequest(new ErrorResponseDto { ErrorCode = 1, Message = e.Message });
            }
            catch (ConflictException e)
            {
                // User not found
                return Conflict(new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            }
            catch (Exception e)
            {
                // Wild card error
                return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<string>> CreateTorrent([FromBody] UserLoginDto userLoginDto)
        {
            try
            {
                string token = await _authService.LoginAsync(userLoginDto);
                return Ok(new JwtTokenResponseDto { Token = token });
            }
            catch (ArgumentException e)
            {
                // Missing fields
                return BadRequest(new ErrorResponseDto { ErrorCode = 1, Message = e.Message });
            }
            catch (ConflictException e)
            {
                // User not found
                return Conflict(new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            }
            catch (Exception e)
            {
                // Wild card error
                return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            }
        }
    }
}