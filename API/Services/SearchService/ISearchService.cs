using API.DTOs.Search;
using API.DTOs.TorrentScrape;

namespace API.Services.SearchService
{
    public interface ISearchService
    {
        // Get scraped torrents
        Task<ScrapedTorrentsResponseDto> GetScrapedTorrentsAsync(SearchRequestDto request);
    }
}
