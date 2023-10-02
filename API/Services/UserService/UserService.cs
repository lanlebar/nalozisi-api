using API.DTOs.User;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

        public async Task<User> GetUserById(int id)
        {
            try
            {
                var user = await _context.User.FirstOrDefaultAsync(u => u.UserId == id) ?? throw new Exception("Uporabnik s tem Id ne obstaja!");
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
