namespace API.Models.Main;

public class Role
{
    [Key]
    public required int RoleId { get; set; }
    public required string RoleName { get; set; }

    // Navigation properties

    // 1-many
    public virtual List<User> Users { get; set; } = new();
}
