using System.Net;

namespace API.Models.Main
{
    [PrimaryKey(nameof(UserId), nameof(TorrentId), nameof(PeerType))]
    public class Peer
    {
        public required int UserId { get; set; }
        public required int TorrentId { get; set; }
        public required PeerType PeerType { get; set; }
        public required IPAddress IpV4 { get; set; }

        // Navigation properties
        public virtual Torrent Torrent { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
