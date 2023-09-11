using API.DTOs.Torrent;

namespace API.Services.TorrentService
{
    public class TorrentService : ITorrentService
    {
        // Fields
        private readonly DataContext _context;
        public TorrentService(DataContext context)
        {
            _context = context;
        }

        // Methods
        public async Task<List<Torrent>> CreateTorrentAsync(AddTorrentDto addTorrentDto)
        {
            return null;
            try
            {
                //var torrent = new Torrent
                //{
                //    Title = request.Title,
                //};
                //await _context.Torrent.AddAsync(torrent);
                //await _context.SaveChangesAsync();
                //return await _context.Torrent.ToListAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<Torrent>> GetTorrentByIdAsync(int torrentId)
        {
            return null;
        }

        public async Task<List<Torrent>> GetTorrentByQueryAsync(string searchQuery)
        {
            return null;
        }

        public async Task<List<Torrent>> GetTorrentByCategoryAsync(int torrentCategoryId)
        {
            return null;
        }

        public async Task<List<Torrent>> GetTorrentByTagAsync(int torrentTagId)
        {
            return null;
        }

        public async Task<List<Torrent>> GetAllTorrentsAsync()
        {
            return null;
        }

        public async Task<List<Torrent>> UpdateTorrentAsync(UpdateTorrentDto request)
        {
            return null;
        }

        public async Task<List<Torrent>> DeleteTorrentAsync(int torrentId)
        {
            return null;
        }
    }
}