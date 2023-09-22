namespace API.DTOs.TorrentScrape
{
    public class ScrapredTorrentDto
    {
        public required string provider { get; set; }
        public required string title { get; set; }
        public required string time { get; set; }
        public required string size { get; set; }
        public required string url { get; set; }
        public required int seeds { get; set; }
        public required int peers { get; set; }
        public required string imdb { get; set; }
    }
}
