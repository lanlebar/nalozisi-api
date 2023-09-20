using API.DTOs.Search;
using API.Services.AuthService;
using API.Services.TorrentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("api/search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        // Fields
        private readonly IAuthService _authService;
        private readonly ITorrentService _torrentService;

        // Constructor
        public SearchController(IAuthService authService, ITorrentService torrentService)
        {
            _authService = authService;
            _torrentService = torrentService;
        }

        [HttpGet, AllowAnonymous]
        public async Task<ActionResult> Search([FromQuery] SearchRequestDto searchRequsetDto)
        {
            try
            {
                // Query is the only necceaary parameter
                if (searchRequsetDto.Query == null)
                {
                    return BadRequest(new ErrorResponseDto { ErrorCode = 1, Message = "Query is required" });
                }

                // Convert category to enum and check existance of it
                Enums.TorrentCategory torrentCategory;
                if (!Enum.TryParse(searchRequsetDto.Category, true, out torrentCategory))
                {
                    return BadRequest(new ErrorResponseDto { ErrorCode = 1, Message = "Invalid category" });
                }
                // Convert source to enum and check existance of it
                Enums.TorrentSource torrentSource;
                if (!Enum.TryParse(searchRequsetDto.Source, true, out torrentSource))
                {
                    return BadRequest(new ErrorResponseDto { ErrorCode = 1, Message = "Invalid source" });
                }


                string result = await _torrentService.GetScrapedTorrentsAsync(searchRequsetDto);
                return Ok(new { result });
            }
            catch (Exception e)
            {
                return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            }
        }
    }
}
