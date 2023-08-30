namespace API.Models.Main

{
    public class BannedIp
    {
        [Key]
        public required string IpV4 { get; set; } = null!;
    }
}
