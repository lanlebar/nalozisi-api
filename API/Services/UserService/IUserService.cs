using API.DTOs.Torrent;
using API.DTOs.User;
using System.Security.Claims;

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

        // User based methods
        // Update user username
        Task<Boolean> UpdateUsername(Claim claim, string username);

        // Update user email
        Task<Boolean> UpdateEmail(Claim claim, string email);

        // Get user pfp stream with mime
        Task<(Stream, string)> GetPfpStreamWithMime(int userId);

        // Get user pfp in base 64
        Task<string> GetPfpBase64(int userId);

        // Update user pfp
        Task<Boolean> UpdatePfp(Claim claim, IFormFile profilePicture);

        // Remove user pfp
        Task<Boolean> RemovePfp(Claim claim);

        // Update user password
        Task<Boolean> UpdatePassword(Claim claim, string newPassword);

        // Get user by id
        Task<User> GetUserById(int userId);

        // Get user by username
        Task<User> GetUserByUsername(string username);

        // Get user by email
        Task<User> GetUserByEmail(string email);

        // Check if user can upload
        Task<Boolean> CanUpload(int userId);

        // Delete user
        Task<Boolean> DeleteUser(int userId);


        // Torrent based user methods
        // User uploaded torrents
        Task<List<ProfileTorrentDto>> GetUploadedTorrentsByUserId(int userId);
        // User liked torrents
        Task<List<ProfileTorrentDto>> GetLikedTorrentsByUserId(int userId);
    }
}
