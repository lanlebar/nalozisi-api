using API.DTOs.Torrent;
using API.Services.FileService;
using System.Security.Claims;

namespace API.Services.UserService
{
    public class UserService : IUserService
    {
        // Fields
        private readonly DataContext _context;
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;

        // Constructor
        public UserService(DataContext context, IFileService fileService, IConfiguration configuration)
        {
            _context = context;
            _fileService = fileService;
            _configuration = configuration;
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

        // User based methods
        public async Task<Boolean> UpdateUsername(Claim claim,  string username)
        {
            // Input formatting - nothing can end with a trailing space
            username = username.Trim().ToLower();

            // Check if username is already taken
            if (await _context.User.AnyAsync(u => u.Username == username)) {
                throw new ConflictExceptionDto("Uporabnik s tem uporabniškim imenom že obstaja!");
            }

            try
            {
                // Update username
                var user = await GetUserById(int.Parse(claim.Value));
                user.Username = username;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Boolean> UpdateEmail(Claim claim, string email)
        {
            // Input formatting - nothing can end with a trailing space
            email = email.Trim().ToLower();

            // Check if email is already taken
            if (await _context.User.AnyAsync(u => u.Email == email))
            {
                throw new ConflictExceptionDto("Uporabnik s tem e-poštnim naslovom že obstaja!");
            }

            try
            {
                // Update email
                var user = await GetUserById(int.Parse(claim.Value));
                user.Email = email;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<(Stream, string)> GetPfpStreamWithMime(int userId)
        {
            // Get needed data from appsettings.json
            string? storageFilePath = _configuration["FileSystem:ProfilePics"];

            if (storageFilePath == null)
            {
                throw new Exception("Cannot access internal file storage data!");
            }

            // Find the profile picture file name (same as user id)
            var user = await GetUserById(userId);
            string pfpFilePath = user.ProfilePicFilePath;
            if (pfpFilePath == null)
            {
                // User doesn't have a profile picture, return null
                return (null, null);
            }
            string fullPfpFilePath = $"{storageFilePath}/{pfpFilePath}";

            var fileStream = new FileStream(fullPfpFilePath, FileMode.Open, FileAccess.Read);
            string mimeType = _fileService.GetMimeType(fullPfpFilePath);
            return (fileStream, mimeType);
        }

        public async Task<string> GetPfpBase64(int userId)
        {
            // Get needed data from appsettings.json
            string? storageFilePath = _configuration["FileSystem:ProfilePics"];

            if (storageFilePath == null)
            {
                throw new Exception("Cannot access internal file storage data!");
            }

            // Find the profile picture file name (same as user id)
            var user = await GetUserById(userId);
            string pfpFilePath = user.ProfilePicFilePath;
            if (pfpFilePath == null)
            {
                // User doesn't have a profile picture, return null
                return null;
            }
            string fullPfpFilePath = $"{storageFilePath}/{pfpFilePath}";

            return _fileService.ConvertFileToBase64(fullPfpFilePath, FileSystemFileType.ProfileImage);
        }

        public async Task<Boolean> UpdatePfp(Claim claim, IFormFile profilePicture)
        {
            // Get needed data from appsettings.json
            var supportedFormats = _configuration.GetSection("FileSystem:SupportedImageFormats").Get<string[]>();                
            string? storageFilePath = _configuration["FileSystem:ProfilePics"];
            int? maxProfilePicSize = Convert.ToInt32(_configuration["FileSystem:ProfilePicsSizeLimit"]);

            if (supportedFormats == null || storageFilePath == null || maxProfilePicSize == null)
            {
                throw new Exception("Cannot access internal file storage data!");
            }

            // Make sure folder exists
            Directory.CreateDirectory(storageFilePath);

            // Check if profile picture file type is supported
            string fileExtension = Path.GetExtension((profilePicture.FileName).ToLowerInvariant());
            if (!supportedFormats.Any(f => f.Equals(fileExtension, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException($"Tip naložene datoteke ({fileExtension}) ni podprt!");
            }

            // Check profile picture size limit
            if (profilePicture.Length > maxProfilePicSize)
            {
                throw new ArgumentException("Naložena datoteka je prevelika. Največa velikost datoteke je 5MB!");
            }

            // Rename image to match user id
            var user = await GetUserById(int.Parse(claim.Value));
            int userId = user.UserId;
            string profilePicFilePath = $"{userId}{fileExtension}";
            string fullProfilePicFilePath = $"{storageFilePath}/{profilePicFilePath}";

            // Delete current profile picture, if exists
            var existingFiles = Directory.GetFiles(storageFilePath, $"{userId}.*");
            foreach (var file in existingFiles)
            {
                if (Path.GetFileNameWithoutExtension(file).Equals(Convert.ToString(userId), StringComparison.OrdinalIgnoreCase))
                {
                    File.Delete(file);
                }
            }

            // Save image to path from appsettings.json
            using (var stream = new FileStream(fullProfilePicFilePath, FileMode.Create))
            {
                await profilePicture.CopyToAsync(stream);
            }

            // Update database image
            try
            {
                user.ProfilePicFilePath = profilePicFilePath;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<Boolean> RemovePfp(Claim claim)
        {
            // Get needed data from appsettings.json
            string? storageFilePath = _configuration["FileSystem:ProfilePics"];

            if (storageFilePath == null)
            {
                throw new Exception("Cannot access internal file storage data!");
            }

            // Delete profile picture - name like userId.*
            var user = await GetUserById(int.Parse(claim.Value));
            int userId = user.UserId;
            // Delete current profile picture, if exists
            var existingFiles = Directory.GetFiles(storageFilePath, $"{userId}.*");
            foreach (var file in existingFiles)
            {
                if (Path.GetFileNameWithoutExtension(file).Equals(Convert.ToString(userId), StringComparison.OrdinalIgnoreCase))
                {
                    File.Delete(file);
                }
            }

            // Updata database image
            try
            {
                user.ProfilePicFilePath = null;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Boolean> UpdatePassword(Claim claim, string password)
        {
            // Input validation
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Geslo ne sme biti prazno!");

            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            // Update password and password salt
            try
            {
                var user = await GetUserById(int.Parse(claim.Value));
                user.PasswordHash = hashedPassword;
                user.PasswordSalt = salt;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<User> GetUserById(int userId)
        {
            User user = await _context.User
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == userId) ?? throw new NotFoundException();
            return user;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            User user = await _context.User.FirstOrDefaultAsync(u => u.Username == username) ?? throw new Exception("Uporabnik s tem uporabniškim imenom ne obstaja!");
            return user;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            User user = await _context.User.FirstOrDefaultAsync(u => u.Email == email) ?? throw new Exception("Uporabnik s tem e-poštnim naslovom ne obstaja!");
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

        // Torrent based user methods
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
                    Title = $"Fake Torrent niofdgoi negoiherghegepogn dofpghpoehgesbbnds opghsepgh ophg {i}",
                    Source = GetRandomEnumValue<TorrentDisplaySource>(),
                    // You can generate a fake IFormFile for testing purposes
                    Image = null,
                    Category = $"Category {i}",
                    Format = $"Format {i}",
                    Year = i % 2 == 0 ? null : (2000 + i).ToString(), // Optional year
                    UploaderUsername = $"Uploadeehojt iehjetšahj tdophnetšh epsthjt phdpobj dgbsobn shbt horr{i}",
                    Size = i * 100.0f,
                    Seeders = i * 10,
                    Leechers = i * 5
                });
            }

            return fakeTorrents;
        }

        // Temp method
        private string GetRandomEnumValue<T>()
        {
            // return random enum value
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .OrderBy(e => Guid.NewGuid())
                .FirstOrDefault()
                .ToString();
        }

    }
}
