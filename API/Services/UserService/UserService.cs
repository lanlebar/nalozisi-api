using API.DTOs.Torrent;
using API.DTOs.User;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Extensions;

namespace API.Services.UserService
{
    public class UserService : IUserService
    {
        // Fields
        private readonly DataContext _context;

        // Constructor
        public UserService(DataContext context)
        {
            _context = context;
        }

        // Methods
        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                return await _context.User.ToListAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Boolean> UserExists(string username, string email)
        {
            try
            {
                var user = await _context.User.FirstOrDefaultAsync(u => u.Username == username || u.Email == email);
                return user != null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Boolean> UserExists(string username)
        {
            try
            {
                var user = await _context.User.FirstOrDefaultAsync(u => u.Username == username);
                return user != null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Boolean> UserExists(int userId)
        {
            try
            {
                var user = await _context.User.FirstOrDefaultAsync(u => u.UserId == userId);
                return user != null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<ProfileTorrentDto>> GetUploadedTorrentsByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProfileTorrentDto>> GetLikedTorrentsByUserId(int userId)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.UserId == userId) ?? throw new Exception("Uporabnik s tem Id ne obstaja!");


            List<ProfileTorrentDto> fakeTorrents = new List<ProfileTorrentDto>();

            // Create and add fake torrent objects to the list
            for (int i = 1; i <= 10; i++)
            {
                fakeTorrents.Add(new ProfileTorrentDto
                {
                    Title = $"Fake Torrent {i}",
                    Source = GetRandomEnumValue<TorrentDisplaySource>(),
                    // You can generate a fake IFormFile for testing purposes
                    Image = null,
                    Category = $"Category {i}",
                    Format = $"Format {i}",
                    Year = i % 2 == 0 ? null : (2000 + i).ToString(), // Optional year
                    UploaderUsername = $"Uploader{i}",
                    Size = i * 100.0f,
                    Seeders = i * 10,
                    Leechers = i * 5
                });
            }

            return fakeTorrents;
        }
        private string GetRandomEnumValue<T>()
        {
            // return random enum value
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .OrderBy(e => Guid.NewGuid())
                .FirstOrDefault()
                .ToString();
        }

        public async Task<User> GetUserById(int userId)
        {
            try
            {
                var user = await _context.User.FirstOrDefaultAsync(u => u.UserId == userId) ?? throw new Exception("Uporabnik s tem Id ne obstaja!");
                return user;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Username == username) ?? throw new Exception("Uporabnik s tem uporabniškim imenom ne obstaja!");
            return user;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email) ?? throw new Exception("Uporabnik s tem e-poštnim naslovom ne obstaja!");
            return user;

        }

        public async Task<Boolean> CanUpload(int userId)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.UserId == userId) ?? throw new Exception("Uporabnik s tem Id ne obstaja!");
            // Load role relation
            _context.Entry(user).Reference(u => u.Role).Load();
            if (user.Role.RoleName == "Nalagalec" || user.Role.RoleName == "Admin")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Task<User> UpdateUser(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Boolean> DeleteUser(int userId)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.UserId == userId) ?? throw new Exception("Uporabnik s tem Id ne obstaja!");
            try
            {
                _context.User.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
