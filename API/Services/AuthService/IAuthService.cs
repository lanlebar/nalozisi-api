using API.DTOs.Auth;
using API.DTOs.User;

namespace API.Services.AuthService
{
    public interface IAuthService
    {
        // Generate JWT
        Task<string> GenerateJwtToken(UserLoginDto user);

        // Register user
        // Validate user doesn't exist yet, validate email is not banned, generate salt, hash password, create user, return user
        Task<string> RegisterAsync(UserRegisterDto user);

        // Login user
        // Validate user exists, validate user is not banned, validate password, generate token, return token
        Task<string> LoginAsync(UserLoginDto user);

        // Logout user
        Task<User> Logout(AuthTokenDto token);

        // Get all banned emails
        Task<List<BannedEmail>> GetBannedEmails();

        // Verify if email is banned
        Task<bool> IsEmailBanned(string email);

        // Get all banned ips
        Task<List<BannedIp>> GetBannedIps();

        // Verify if ip is banned
        Task<bool> IsIpBanned(string ip);
    }
}
