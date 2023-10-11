using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTOs.User
{
    public class UpdateUserDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        [NotMapped]
        public IFormFile? ProfilePicFile { get; set; }
    }
}
