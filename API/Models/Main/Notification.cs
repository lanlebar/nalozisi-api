namespace API.Models.Main;

public partial class Notification
{
    [Key]
    public required int NotificationId { get; set; }
    public required int UserId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required DateTime TimeSent { get; set; }

    // Navigation properties
    // 1-many
    public virtual User User { get; set; } = null!;
}
