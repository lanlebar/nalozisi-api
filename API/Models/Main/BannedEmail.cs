namespace API.Models.Main;

public class BannedEmail
{
    [Key]
    public required string Email { get; set; } = null!;
}
