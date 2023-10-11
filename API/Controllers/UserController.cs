using API.DTOs.Torrent;
using API.DTOs.User;
using API.Services.FileService;
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
        [HttpGet("likedTorrents"), Authorize]
        public async Task<ActionResult<List<ProfileTorrentDto>>> GetLikedTorrents(int userId)
        {
            try
            {
                var torrents = await _userService.GetLikedTorrentsByUserId(userId);
                return Ok(torrents);
            }
            catch (Exception e)
            {
                return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            }
        }

        [HttpGet("uploadedTorrents"), Authorize]
        public async Task<ActionResult<List<ProfileTorrentDto>>> getUploadedTorrents(int userId)
        {
            try
            {
                var torrents = await _userService.GetUploadedTorrentsByUserId(userId);
                return Ok(torrents);
            }
            catch (Exception e)
            {
                return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            }
        }

        [HttpPost("update"), Authorize]
        public async Task<ActionResult> UpdateUser ([FromForm] UpdateUserDto updateUserDto)
        {
            // Check which fields are being updated
            if (updateUserDto.Username == null && updateUserDto.Email == null && updateUserDto.Password == null && updateUserDto.ProfilePicFile == null)
            {
                return BadRequest(new ErrorResponseDto { ErrorCode = 1, Message = "No fields to update" });
            }
            try
            {
                // Check if user exists
                var userId = User.FindFirst("uid")?.Value;
                if (userId == null)
                {
                    return Unauthorized();
                }
                await _userService.UpdateUser(updateUserDto);

                return Ok();
            }
            catch (ConflictExceptionDto e)
            {
                return Conflict(new ErrorResponseDto { ErrorCode = 1, Message = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            }
        }


        [HttpDelete("{userid}"), Authorize]
        public async Task<ActionResult<string>> DeleteUser(DeleteUserDto userDeleteDto)
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
