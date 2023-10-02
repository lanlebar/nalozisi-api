using API.DTOs.User;
using API.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Formats.Asn1;
using System.Security.Claims;
namespace API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // Fields
        private readonly IUserService _userService;

        // Constructor
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Routes
       [HttpDelete("{userid}"), AllowAnonymous]
        public async Task<ActionResult<string>> DeleteUser(UserDeleteDto userDeleteDto)
        {
            // Get user id from token
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userIdFromToken = User.Claims.First(c => c.Type == "sub").Value;

            int n = 10;
            // Check if the user making the request is the same as the one being deleted
            if (currentUserId != userDeleteDto.UserId.ToString())
            {
                return Unauthorized();
            }

            try
            {
                var result = await _userService.DeleteUser(userDeleteDto.UserId);
                if (result)
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = "Cannot delete user" });
                }
            }
            catch (Exception e)
            {

                return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            }

        }
    }
}
