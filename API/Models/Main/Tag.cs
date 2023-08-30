namespace API.Models.Main
{
    public class Tag
    {
        [Key]
        public required int TagId { get; set; }
        public required string TagName { get; set; }

        // Navigation properties

        // many-many
        public virtual List<TorrentTag> TorrentTags { get; set; } = new();

    }
}
