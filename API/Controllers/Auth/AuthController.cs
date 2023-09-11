using API.DTOs.Error;
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

        [HttpPost("register"), AllowAnonymous]
        public async Task<ActionResult<string>> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            try
            {
                string token = await _authService.RegisterAsync(userRegisterDto);
                return StatusCode(201, new JwtTokenResponseDto { Token= token });
            }
            catch (ArgumentException e)
            {
                // Missing fields
                return BadRequest(new ErrorResponseDto { ErrorCode = 1, Message = e.Message });
            }
            catch (ConflictException e)
            {
                // User not found
                return Conflict(new ErrorResponseDto { ErrorCode = 1, Message = e.Message });
            }
            catch (Exception e)
            {
                // Wild card error
                return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            }
        }

        [HttpPost("login"), AllowAnonymous]
        public async Task<ActionResult<string>> Login([FromBody] UserLoginDto userLoginDto)
        {
            try
            {
                var token = await _authService.LoginAsync(userLoginDto);
                return Ok(new JwtTokenResponseDto { Token = token });
            }
            catch (ArgumentException e)
            {
                return StatusCode(401, new ErrorResponseDto { ErrorCode = 1, Message = e.Message });

            }
            catch (Exception e)
            {
                // Wild card error
                return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            }
        }

        [HttpGet("verify"), Authorize]
        public ActionResult Verify()
        {
            // If we get here, the token is valid
            return Ok();
        }

    }
}

