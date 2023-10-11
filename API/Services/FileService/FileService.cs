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
        public async Task SaveFile(IFormFile file, FileSystemFileType fileSystemFileType, User? user = null, Torrent? torrent = null)
        {
            List<string> supportedImageFormats = _configuration.GetSection("FileSystem:SupportedImageFormats").Get<List<string>>();
            if (supportedImageFormats == null)
            {
                throw new Exception("SupportedImageFormats not found in appsettings.json");
            }


            // Check if file is supported
            string fileExtension = file.FileName.Split('.').Last();
            if (!supportedImageFormats.Contains(fileExtension))
            {
                throw new Exception("File format is not supported");
            }

            string basePath = GetBasePath(fileSystemFileType);
            string fileName;
            switch (fileSystemFileType)
            {
                case FileSystemFileType.ProfileImage:
                    // Save file to basePath + userId
                    if (user == null)
                    {
                        throw new Exception("User is required for saving a profile picture");
                    }
                    fileName = user.UserId.ToString();
                    break;
                case FileSystemFileType.TorrentImage:
                    // Save file to basePath + torrentId
                    if (torrent == null)
                    {
                        throw new Exception("Torrent is required for saving a torrent picture");
                    }
                    fileName = torrent.TorrentGuid.ToString();
                    break;
                case FileSystemFileType.Wysiwyg:
                    // Save file to basePath + torrentId
                    if (torrent == null)
                    {
                        throw new Exception("Torrent is required for saving a torrent picture");
                    }
                    fileName = torrent.TorrentGuid.ToString();
                    break;
                default:
                    throw new Exception("Invalid fileSystemFileType");
            }

            // Create directory if it doesn't exist
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            // Save file
            string filePath = $"{Path.Combine(basePath, fileName)}.{fileExtension}";
            using Stream stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
        }

        public Task<FileDto> GetFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteFile(string filePath)
        {
            throw new NotImplementedException();
        }

        // Helper methods
        private string GetBasePath(FileSystemFileType fileSystemFileType)
        {
            string? path;
            switch (fileSystemFileType)
            {
                case FileSystemFileType.ProfileImage:
                    path = _configuration["FileSystem:ProfilePics"];
                    break;

                case FileSystemFileType.TorrentImage:
                    path = _configuration["FileSystem:TorrentPics"];
                    break;

                case FileSystemFileType.Wysiwyg:
                    path = _configuration["FileSystem:WysiwygFiles"];
                    break;

                default:
                    throw new Exception("Invalid fileSystemFileType");
            }
            return path ?? throw new Exception("Base path not found in appsettings.json");
        }
    }
}
