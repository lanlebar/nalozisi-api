namespace API.Models.Main
{
    [PrimaryKey(nameof(TorrentId), nameof(TagId))]
    public class TorrentTag
    {
        public required int TorrentId { get; set; }
        public required int TagId { get; set; }
        public required string TagValue { get; set; }
        public virtual Tag Tag { get; set; } = null!;
        public virtual Torrent Torrent { get; set; } = null!;

    }
}
