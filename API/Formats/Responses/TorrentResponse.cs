namespace API.Formats.Return
{
    public class TorrentResponse
    {
        public required int TorrentId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string MagnetLink { get; set; }

    }
}
