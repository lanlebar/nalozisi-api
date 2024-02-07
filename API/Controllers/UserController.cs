using API.DTOs.Torrent;
using API.DTOs.User;
using API.Services.FileService;
using API.Services.UserService;
using API.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Formats.Asn1;
using System.Security.Claims;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
namespace API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // Fields
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IFileService _fileService;

        // Constructor
        public UserController(IUserService userService, IAuthService authService, IFileService fileService)
        {
            _userService = userService;
            _authService = authService;
            _fileService = fileService;
        }

        // Routes
        [HttpGet("{userId}"), Authorize]
        public async Task<ActionResult> GetUser(int userId)
        {
            try
            {
                User user = await _userService.GetUserById(userId);
                return Ok(new GetUser
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                    Joined = user.JoinedDate.Date.ToShortDateString(),
                    Role = Convert.ToString(user.Role.RoleName)
                });
            }
            catch (NotFoundException e)
            {
                return NotFound(e);
            }
            catch (Exception e)
            {
                return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            }
        }

        [HttpGet("pfp/{userId}")]
        public async Task<IActionResult> GetUserPfp(int userId)
        {
            try
            {
                var (stream, mimeType) = await _userService.GetPfpStreamWithMime(userId);
                if (stream == null || mimeType == null)
                {
                    return NotFound();
                }
                return File(stream, mimeType);
            }
            catch (Exception e)
            {
                return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            }
        }

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

        [HttpPost("updateUsername"), Authorize]
        public async Task<ActionResult> UpdateUsername([FromForm] UpdateUsernameDto updateUsernameDto)
        {
            try
            {
                // Check if user exists
                var userId = User.FindFirstValue("uid");
                if (userId == null)
                {
                    return Unauthorized();
                }

                // Check if current username is the same
                var oldUsername = User.FindFirstValue("username");
                if (oldUsername == updateUsernameDto.Username)
                {
                    return Conflict(new ErrorResponseDto { ErrorCode = 1, Message = "Novo uporabniško ime ne sme biti enako prejšnjemu!" });
                }
                
                var claim = new Claim("uid", userId);
                var updateUsernameMethodInvoke = await _userService.UpdateUsername(claim, updateUsernameDto.Username);
                if (!updateUsernameMethodInvoke)
                {
                    return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = "Server error" });
                }
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

        [HttpPost("updateEmail"), Authorize]
        public async Task<ActionResult> UpdateEmail([FromForm] UpdateEmailDto updateEmailDto)
        {
            try
            {
                // Check if user exists
                var userId = User.FindFirstValue("uid");
                if (userId == null)
                {
                    return Unauthorized();
                }

                // Check if current email is the same
                var oldEmail = User.FindFirstValue("email");
                if (oldEmail == updateEmailDto.Email)
                {
                    return BadRequest(new ErrorResponseDto { ErrorCode = 1, Message = "Nov e-poštni naslov ne sme biti enak prejšnjemu!" });
                }

                var claim = new Claim("uid", userId);
                var updateEmailMethodInvoke = await _userService.UpdateEmail(claim, updateEmailDto.Email);
                if (!updateEmailMethodInvoke)
                {
                    return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = "Server error" });
                }
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

        [HttpPost("updatePfp"), Authorize]
        public async Task<ActionResult> UpdatePfp([FromForm] UpdatePfpDto updatePfpDto)
        {
            try
            {
                // Check if user exists
                var userId = User.FindFirstValue("uid");
                if (userId == null)
                {
                    return Unauthorized();
                }

                var claim = new Claim("uid", userId);
                // Check if profile picture is attached - determine between update and remove profile picture
                if (updatePfpDto.ProfilePicFile == null)
                {
                    // Remove profile picture
                    var removePfpMethodInvoke = await _userService.RemovePfp(claim);
                    if (!removePfpMethodInvoke)
                    {
                        return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = "Server error" });
                    }
                    return Ok();
                }
                else
                {
                    // Update profile picture
                    var updatePfpMethodInvoke = await _userService.UpdatePfp(claim, updatePfpDto.ProfilePicFile);
                    if (!updatePfpMethodInvoke)
                    {
                        return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = "Server error" });
                    }
                    return Ok();
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            }
        }

        [HttpPost("updatePassword"), Authorize]
        public async Task<ActionResult> UpdatePassword([FromForm] UpdatePasswordDto updatePasswordDto)
        {
            try
            {
                // Check if user exists
                var userId = User.FindFirstValue("uid");
                if (userId == null)
                {
                    return Unauthorized();
                }

                // Old password cannot be the same as new password
                if (updatePasswordDto.NewPassword == updatePasswordDto.OldPassword)
                {
                    return StatusCode(403, new ErrorResponseDto { ErrorCode = 1, Message = "Novo geslo ne sme biti enako prejšnjemu" });
                }

                // Verify the old password is correct
                var username = User.FindFirstValue("username") ?? throw new ArgumentNullException();
                if (! await _authService.VerifyLogin(username, updatePasswordDto.OldPassword))
                {
                    return StatusCode(403, new ErrorResponseDto { ErrorCode = 1, Message = "Staro geslo ni pravilno!" });

                }

                var claim = new Claim("uid", userId);
                var updatePasswordMethodInvoke = await _userService.UpdatePassword(claim, updatePasswordDto.NewPassword);
                if (!updatePasswordMethodInvoke)
                {
                    return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = "Server error" });
                }
                return Ok();
            }
            catch (Exception e)
            {

                if (e is ArgumentNullException)
                {
                    return BadRequest(new ErrorResponseDto { ErrorCode = 1, Message = "Uporabniško ime je prazno!" });
                }
                return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            }
        }

        [HttpDelete("deleteUser"), Authorize]
        public async Task<IActionResult> DeleteUser([FromForm] DeleteUserDto deleteUserDto)
        {
            try
            {
                // Check if user exists
                var userId = User.FindFirstValue("uid");
                if (userId == null)
                {
                    return Unauthorized();
                }

                // Verify password
                var username = User.FindFirstValue("username") ?? throw new ArgumentNullException();
                if (!await _authService.VerifyLogin(username, deleteUserDto.Password))
                {
                    return StatusCode(403, new ErrorResponseDto { ErrorCode = 1, Message = "Geslo ni pravilno" });
                }

                var claim = new Claim("uid", userId);
                var deleteUserMethodInvoke = await _userService.DeleteUser(Convert.ToInt32(userId));
                if (!deleteUserMethodInvoke)
                {
                    return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = "Server error" });
                }
                return Ok();
            }
            catch (Exception e)
            {
                if (e is ArgumentNullException)
                {
                    return BadRequest(new ErrorResponseDto { ErrorCode = 1, Message = "Uporabniško ime je prazno!" });
                }
                return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            }
        }
    }
}
