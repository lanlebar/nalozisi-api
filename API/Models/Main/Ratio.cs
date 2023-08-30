namespace API.Models.Main;

public class Ratio
{
    [Key]
    public required int UserId { get; set; }
    public required int SeedingBytes { get; set; }
    public required int LeechingBytes { get; set; }

    // Navigation properties
    // 1-1
    public virtual User User { get; set; } = null!;
}
