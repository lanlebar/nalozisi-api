namespace API.DTOs.Torrent
{
    public class UploadTorrentDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required IFormFile TorrentFile { get; set; }
        public IFormFile? Image { get; set; }
        public required string TorrentCategory { get; set; }
        public required int UserId { get; set; }
        public required string MagnetLink { get; set; }   
    }
}
