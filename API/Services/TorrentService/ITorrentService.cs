using API.DTOs.Torrent;

namespace API.Services.TorrentService
{
    public interface ITorrentService
    {
        // Create torrent
        Task<List<Torrent>> CreateTorrentAsync(AddTorrentDto request);

        // Get torrent
        // By torrent id
        Task<List<Torrent>> GetTorrentByIdAsync(int torrentId);
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
