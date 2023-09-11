namespace API.DTOs.Torrent
{
    public class AddTorrentDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required IFormFile Image { get; set; }
        public required int UserId { get; set; }
        public required string MagnetLink { get; set; }   
    }
}
