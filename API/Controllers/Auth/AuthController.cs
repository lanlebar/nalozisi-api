using API.DTOs.User;
using API.Services.AuthService;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers.Auth
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Fields
        private readonly IAuthService _authService;

        // Constructor
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            try
            {
                string token = await _authService.RegisterAsync(userRegisterDto);
                return StatusCode(201, new JwtTokenResponse { Token= token });
            }
            catch (ArgumentException e)
            {
                return BadRequest(new ErrorRespone { ErrorCode = 1, Message = e.Message });
            }
            catch (ConflictException e)
            {
                // Wild card error
                return Conflict(new ErrorRespone { ErrorCode = 2, Message = e.Message });
            }
            catch (Exception e)
            {
                // Wild card error
                return StatusCode(500, new ErrorRespone { ErrorCode = 2, Message = e.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] UserLoginDto userLoginDto)
        {
            try
            {
                var token = await _authService.LoginAsync(userLoginDto);
                return Ok(new JwtTokenResponse { Token = token });
            }
            catch (ArgumentException e)
            {
                return BadRequest(new ErrorRespone { ErrorCode = 1, Message = e.Message });
            }
            catch (Exception e)
            {
                // Wild card error
                return StatusCode(500, new ErrorRespone { ErrorCode = 2, Message = e.Message });
            }
        }


    }
}

