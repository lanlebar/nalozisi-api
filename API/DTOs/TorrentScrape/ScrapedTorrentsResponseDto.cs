using API.DTOs.TorrentScrape;
using Newtonsoft.Json;

namespace API.DTOs.TorrentScrape
{
    public class ScrapedTorrentsResponseDto
    {
        [JsonProperty("ThePirateBay")]
        public List<ScrapredTorrentDto>? ThePirateBay { get; set; }
        
        [JsonProperty("1337x")]
        public List<ScrapredTorrentDto>? _1337x { get; set; }
        
        [JsonProperty("Yts")]
        public List<ScrapredTorrentDto>? Yts { get; set; }
    }
}