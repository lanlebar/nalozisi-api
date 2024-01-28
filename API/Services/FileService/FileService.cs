using API.DTOs.FileSystem;
using API.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Reflection.Metadata;

namespace API.Services.FileService
{
    public class FileService : IFileService
    {
        // Fields
        private readonly IConfiguration _configuration;

        // Constructor
        public FileService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Methods
        public async Task<string> ConvertFileStreamToBase64(Stream fileStream)
        {
            using (var memoryStream = new MemoryStream())
            {
                await fileStream.CopyToAsync(memoryStream);
                byte[] imageBytes = memoryStream.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }

        public string ConvertFileToBase64(string filePath, FileSystemFileType fileType)
        {
            byte[] fileBytes = File.ReadAllBytes(GetFullFilePath(filePath, fileType));
            return Convert.ToBase64String(fileBytes);
        }

        public string GetFullFilePath(string filePath, FileSystemFileType fileType)
        {
            // Get needed data from appsettings.json
            string? profilePicStorage = _configuration["FileSystem:ProfilePics"];
            if (profilePicStorage == null)
            {
                throw new Exception("Cannot access internal file storage data!");
            }

            return fileType switch
            {
                FileSystemFileType.ProfileImage => $"{profilePicStorage}/{filePath}",
                _ => profilePicStorage.ToString()
            };
        }

        public string GetMimeType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".webp" => "image/webp",
                ".bmp" => "image/bmp",
                ".gif" => "image/gif",
                ".tiff" or ".tif" => "image/tiff",
                _ => "application/octet-stream", // Default MIME
            };
        }
    }
}
