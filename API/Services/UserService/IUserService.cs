using API.DTOs.Torrent;
using API.DTOs.User;

namespace API.Services.UserService
{
    public interface IUserService
    {
        // Get all users
        Task<List<User>> GetAllUsers();

        // Verify if user exists
        // Returns true if user exists, false if not
        Task<Boolean> UserExists(string username, string email);
        Task<Boolean> UserExists(string username);
        Task<Boolean> UserExists(int userId);

        // Torrent based user methods
        // User uploaded torrents
        Task<List<ProfileTorrentDto>> GetUploadedTorrentsByUserId(int userId);
        // User liked torrents
        Task<List<ProfileTorrentDto>> GetLikedTorrentsByUserId(int userId);

        // User based methods
        // Get user by id
        Task<User> GetUserById(int userId);

        // Get user by username
        Task<User> GetUserByUsername(string username);

        // Get user by email
        Task<User> GetUserByEmail(string email);

        // Check if user can upload
        Task<Boolean> CanUpload(int userId);

        // Update user - returns new user
        Task<User> UpdateUser(int userId);

        // Delete user
        Task<Boolean> DeleteUser(int userId);
    }
}
