using API.DTOs.Recommend;
using API.Services.RecommendService;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/recommend")]
    [ApiController]
    public class RecommnedController : ControllerBase
    {

        // Fields
        private readonly IRecommnedService _recommnedService;

        // Constructor
        public RecommnedController(IRecommnedService recommnedService)
        {
            _recommnedService = recommnedService;
        }

        [HttpGet("nowPlaying"), Authorize]
        public async Task<ActionResult<List<TmdbMovieResponse>>> NowPlaying(string language, int page, string region)
        {
            try
            {
                if (language != "sl-SI" && language != "en-US")
                {
                    return BadRequest();
                }

                var result = await _recommnedService.NowPlaying(language, page, region);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            }
        }

        [HttpGet("popular"), Authorize]
        public async Task<ActionResult<List<TmdbMovieResponse>>> Popular(string language, int page, string region)
        {
            try
            {
                if (language != "sl-SI" && language != "en-US")
                {
                    return BadRequest();
                }

                var result = await _recommnedService.Popular(language, page, region);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            }
        }

        [HttpGet("topRated"), Authorize]
        public async Task<ActionResult<List<TmdbMovieResponse>>> TopRated(string language, int page, string region)
        {
            try
            {
                if (language != "sl-SI" && language != "en-US")
                {
                    return BadRequest();
                }

                var result = await _recommnedService.TopRated(language, page, region);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            }
        }

        [HttpGet("upcoming"), Authorize]
        public async Task<ActionResult<List<TmdbMovieResponse>>> Upcoming(string language, int page, string region)
        {
            try
            {
                if (language != "sl-SI" && language != "en-US")
                {
                    return BadRequest();
                }

                var result = await _recommnedService.Upcoming(language, page, region);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            }
        }

        [HttpGet("trendingMovie"), Authorize]
        public async Task<ActionResult<List<TmdbTrendingResponse>>> TrendingMovie(string timeWindow, string language)
        {
            try
            {
                timeWindow = timeWindow.ToLower();
                if (timeWindow != "day" && timeWindow != "week")
                {
                    return BadRequest();
                }

                if (language != "sl-SI" && language != "en-US")
                {
                    return BadRequest();
                }

                var result = await _recommnedService.TrendingMovie(timeWindow, language);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            }
        }

        [HttpGet("trendingTv"), Authorize]
        public async Task<ActionResult<List<TmdbTrendingResponse>>> TrendingTv(string timeWindow, string language)
        {
            try
            {
                timeWindow = timeWindow.ToLower();
                if (timeWindow != "day" && timeWindow != "week")
                {
                    return BadRequest();
                }

                if (language != "sl-SI" && language != "en-US")
                {
                    return BadRequest();
                }

                var result = await _recommnedService.TrendingTv(timeWindow, language);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new ErrorResponseDto { ErrorCode = 2, Message = e.Message });
            }
        }
    }
}