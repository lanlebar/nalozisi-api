using API.DTOs.User;
using API.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // Fields
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Routes
        [HttpGet("userid"), AllowAnonymous]
        public async Task<ActionResult<string>> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            return Ok();
            try
            {
                return Ok();
            }
            catch (ArgumentException e)
            {
                // Missing fields
                return BadRequest(new ErrorResponseDto { ErrorCode = 1, Message = e.Message });
            }
            catch (ConflictExceptionDto e)
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
    }
}
