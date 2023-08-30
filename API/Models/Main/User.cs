using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Main;

public class User
{
    public int UserId { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string PasswordSalt { get; set; }
    public required DateTime JoinedDate { get; set; }
    public required int RoleId { get; set; }

    // Navigation properties
    
    // 1-1
    public virtual Ratio Ratio { get; set; } = null!;
    public virtual Role Role { get; set; } = null!;
    
    // 1-many
    public virtual List<Notification> Notifications { get; set; } = new();
    public virtual List<Torrent> UploadedTorrents { get; set; } = new();

    // many-many
    public virtual List<Like> LikedTorrents { get; set; } = new ();
    public virtual List<Peer> Peers { get; set; } = new ();

}
