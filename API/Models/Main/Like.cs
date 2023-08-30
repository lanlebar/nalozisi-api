namespace API.Models.Main
{
    [PrimaryKey(nameof(UserId), nameof(TorrentId))]
    public class Like
    {
        public required int UserId { get; set; }
        public required int TorrentId { get; set; }

        // Navigation properties
        public virtual Torrent Torrent { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
