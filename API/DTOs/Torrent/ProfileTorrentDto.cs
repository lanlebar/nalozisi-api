namespace API.DTOs.Torrent
{
    public class ProfileTorrentDto
    {
        public required string Title { get; set; }
        public required string Source { get; set; }
        public required IFormFile Image { get; set; }
        public required string Category { get; set; }
        public required string Format { get; set; }
        public string? Year { get; set; }
        public required string UploaderUsername { get; set; }
        public required float Size { get; set; }
        public required int Seeders { get; set; }
        public required int Leechers { get; set; }
    }
}
