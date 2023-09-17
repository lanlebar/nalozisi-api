namespace API.Models.Main;

public class Torrent
{
    [Key]
    public required Guid TorrentGuid { get; set; }
    public  required string Title { get; set; }
    public string? TorrentFilePath { get; set; }
    public string? DescriptionFilePath { get; set; }
    public string? ImageFilePath { get; set; }
    public required double SizeBytes { get; set; }
    public required DateTime UploadedDate { get; set; }
    public required string MagnetLink { get; set; }
    public required int UserId { get; set; }
    public int? TorrentCategoryId { get; set; }
    public int? DownloadAmount { get; set; }
    public int? LikeAmount { get; set; }

    // Navigation properties

    // 1-many
    public TorrentCategory TorrentCategory { get; set; } = null!;
    public User User { get; set; } = null!;

    // many-many
    public virtual List<TorrentTag> TorrentTags { get; set; } = null!;
    public virtual List<Like> LikedUsers { get; set; } = new();
    public virtual List<Peer> Peers { get; set; } = new();
}
