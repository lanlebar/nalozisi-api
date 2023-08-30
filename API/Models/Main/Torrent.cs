namespace API.Models.Main;

public class Torrent
{
    [Key]
    public required int TorrentId { get; set; }
    public  required string Title { get; set; }
    public string? DescriptionFileGuid { get; set; }
    public string? ImageFileGuid { get; set; }
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
    public virtual List<TorrentTag> TorrentTags { get; set; } = new();
    public virtual List<Like> LikedUsers { get; set; } = new();
    public virtual List<Peer> Peers { get; set; } = new();
}
