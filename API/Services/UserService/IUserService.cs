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

        // Get user by id
        Task<User> GetUserById(int userId);

        // Get user by username
        Task<User> GetUserByUsername(string username);

        // Get user by email
        Task<User> GetUserByEmail(string email);

        // Check if user can upload
        Task<Boolean> CanUpload(int userId);

        // Update user - returns bool
        Task<User> UpdateUser(int userId);

        // Delete user
        Task<List<User>> DeleteUser(int userId);
    }
}
