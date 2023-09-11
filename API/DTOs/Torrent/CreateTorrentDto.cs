namespace API.DTOs.Torrent
{
    public class CreateTorrentDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required List<IFormFile> Files { get; set; }
        public IFormFile? Image { get; set; }
        public required string TorrentCategory { get; set; }
        public required int UserId { get; set; }
        public string? MagnetLink { get; set; }
    }
}
