﻿using API.DTOs.User;
using API.Services.UserService;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace API.Services.AuthService
{
    public class AuthService : IAuthService
    {
        // Fields
        public readonly IConfiguration _configuration;
        private readonly DataContext _context;
        private readonly IUserService _userService;

        // Constructor
        public AuthService(DataContext context, IUserService userService, IConfiguration configuration)
        {
            _context = context;
            _userService = userService;
            _configuration = configuration;
        }

        // Methods
        public async Task<string> GenerateJwtToken(UserLoginDto userLoginDto)
        {
            if (!await _userService.UserExists(userLoginDto.Username))
            {
                throw new ArgumentException("Uporabnik s tem uporabniškim imenom ne obstaja!");
            }

            var user = await _context.User.FirstOrDefaultAsync(u => u.Username == userLoginDto.Username) 
                ?? throw new ArgumentException("Uporabnik s tem uporabniškim imenom ne obstaja!");
            
            List<Claim> claims = new List<Claim>
            {
                new Claim("uid", user.UserId.ToString()),
                new Claim("username", user.Username.ToString()),
                new Claim("email", user.Email.ToString()),
                new Claim("role", user.RoleId.ToString()),
                new Claim("joined", user.JoinedDate.ToString())
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Key").Value!));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            JwtSecurityToken token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddYears(10),
                signingCredentials: creds,
                issuer: _configuration.GetSection("AppSettings:Issuer").Value,
                audience: _configuration.GetSection("AppSettings:Audience").Value
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GenerateJwtToken(int userId)
        {
            if (!await _userService.UserExists(userId))
            {
                throw new ArgumentException("Uporabnik s tem uporabniškim imenom ne obstaja!");
            }

            var user = await _context.User.FirstOrDefaultAsync(u => u.UserId == userId) ?? 
                throw new ArgumentException("Uporabnik s tem uporabniškim imenom ne obstaja!");

            List<Claim> claims = new List<Claim>
            {
                new Claim("uid", user.UserId.ToString()),
                new Claim("username", user.Username.ToString()),
                new Claim("email", user.Email.ToString()),
                new Claim("role", user.RoleId.ToString()),
                new Claim("joined", user.JoinedDate.ToString())
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Key").Value!));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            JwtSecurityToken token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddYears(10),
                signingCredentials: creds,
                issuer: _configuration.GetSection("AppSettings:Issuer").Value,
                audience: _configuration.GetSection("AppSettings:Audience").Value
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> RegisterAsync(UserRegisterDto request)
        {
            // Input validation
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password) 
                || string.IsNullOrWhiteSpace(request.Email))
            {
                throw new ArgumentException("Uporabniške ime, e-poštni naslov in geslo so obvezni!");
            }

            // Input formatting - nothing can end with a trailing space
            request.Email = request.Email.Trim().ToLower();
            request.Username = request.Username.Trim().ToLower();
            request.Password = request.Password.Trim();


            // Check if user exists
            if (await _userService.UserExists(request.Username, request.Email))
            {
                throw new ConflictExceptionDto("Uporabnik s tem uporabniškim imenom ali e-poštnim naslovom že obstaja!");
            }
            string salt = GenerateSalt();
            string hashedPassword = HashPassword(request.Password, salt);
            // Role table: 1 = Admin, 2 = Uporabnik
            Role role = _context.Role.FirstOrDefault(r => r.RoleId == 2) ?? throw new ArgumentException("Role not found");
            var newUser = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = hashedPassword,
                PasswordSalt = salt,
                ProfilePicFilePath = null,
                JoinedDate = DateTime.UtcNow,
                RoleId = 2,
                Role = role
            };
            // Add new user instance to the database
            _context.User.Add(newUser);
            await _context.SaveChangesAsync();
            // Save changes to the database
            await _context.SaveChangesAsync();
            return await GenerateJwtToken(new UserLoginDto { Username = request.Username, Password = request.Password});
        }

        public async Task<string> LoginAsync(UserLoginDto request)
        {
            // Input validation
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                throw new ArgumentException("Uporabniško ime in geslo sta obvezna!");
            }

            // Input formatting - nothing cannot end with a trailing space
            request.Username = request.Username.Trim().ToLower();
            request.Password = request.Password.Trim();

            // Check if user exists
            if (!await _userService.UserExists(request.Username))
            {
                throw new ArgumentException("Napačno uporabniško ime ali geslo!");
            }

            // Get user by username
            User user = await _userService.GetUserByUsername(request.Username);

            // Check if password is correct
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new ArgumentException("Napačno uporabniško ime ali geslo!");
            }

            // Generate JWT token
            return await GenerateJwtToken(request);
        }

        public async Task<Boolean> VerifyLogin(string username, string password)
        {
            // Get user by username
            User user = await _userService.GetUserByUsername(username);

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return false;
            }
            return true;
        }

        // Helper methods
        private string GenerateSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt();
        }

        private string HashPassword(string password, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }
    }
}
