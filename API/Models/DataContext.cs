namespace API.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> opions) : base(opions) { }

        // Tables
        // User related
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Ratio> Ratio { get; set; }
        public required virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        
        // Torrent related
        public virtual DbSet<Torrent> Torrent { get; set; }
        public virtual DbSet<TorrentCategory> TorrentCategory { get; set; }
        public virtual DbSet<TorrentTag> TorrentTag { get; set; }

        // Torrent-User related
        public virtual DbSet<Like> Like { get; set; }
        public virtual DbSet<Peer> Peer { get; set; }

        // General
        public virtual DbSet<BannedEmail> BannedEmail { get; set; }
        public virtual DbSet<BannedIp> BannedIp { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }


    }
}