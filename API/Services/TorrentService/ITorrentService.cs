using API.DTOs.Search;
using API.DTOs.Torrent;
using API.DTOs.TorrentScrape;

namespace API.Services.TorrentService
{
    public interface ITorrentService
    {
        // Get scraped torrents
        Task<ScrapedTorrentsResponseDto> GetScrapedTorrentsAsync(SearchRequestDto request);

        // Create torrent
        // By torrent file upload
        Task<Torrent> UploadTorrentAsync(UploadTorrentDto request);
        // Creating the actual torrent file - TODO
        Task<Torrent> CreateTorrentAsync(UploadTorrentDto request);

        // Get torrent
        // By torrent id
        Task<Torrent> GetTorrentByIdAsync(int torrentId);
        // By search query
        Task<List<Torrent>> GetTorrentByQueryAsync(string searchQuery);
        // By torrent category
        Task<List<Torrent>> GetTorrentByCategoryAsync(int torrentCategoryId);
        // By torrent tag
        Task<List<Torrent>> GetTorrentByTagAsync(int torrentTagId);
        // Get all torrents
        Task<List<Torrent>> GetAllTorrentsAsync();

        // Update torrent
        Task<List<Torrent>> UpdateTorrentAsync(UpdateTorrentDto request);

        // Delete torrent
        Task<List<Torrent>> DeleteTorrentAsync(int torrentId);
    }
}
