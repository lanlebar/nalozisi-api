using API.DTOs.User;
using API.Services.AuthService;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers.Torrent
{
    [Route("api/torrent")]
    [ApiController]
    public class TorrentController : ControllerBase
    {
        // Fields
        private readonly IAuthService _authService;

        // Constructor
        public TorrentController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("create"), AllowAnonymous]
        public async Task<ActionResult<string>> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            try
            {
                string token = await _authService.RegisterAsync(userRegisterDto);
                return StatusCode(201, new JwtTokenResponse { Token = token });
            }
            catch (ArgumentException e)
            {
                // Missing fields
                return BadRequest(new ErrorRespone { ErrorCode = 1, Message = e.Message });
            }
            catch (ConflictException e)
            {
                // User not found
                return Conflict(new ErrorRespone { ErrorCode = 2, Message = e.Message });
            }
            catch (Exception e)
            {
                // Wild card error
                return StatusCode(500, new ErrorRespone { ErrorCode = 2, Message = e.Message });
            }
        }
    }
}