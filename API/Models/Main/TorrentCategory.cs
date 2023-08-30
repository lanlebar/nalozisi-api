namespace API.Models.Main
{
    public class TorrentCategory
    {
        [Key]
        public required int TorrentCategoryId { get; set; }
        public required string CategoryName { get; set; }

        // Navigation properties
        // 1-many
        public virtual List<Torrent> Torrents { get; set; } = new();

    }
}
