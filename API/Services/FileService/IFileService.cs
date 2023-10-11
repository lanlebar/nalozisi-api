using API.DTOs.FileSystem;

namespace API.Services.FileService
{
    public interface IFileService
    {
        // Save file
        Task SaveFile(IFormFile file, FileSystemFileType fileSystemFileType, User? user = null, Torrent? torrent = null);

        // Get file
        Task<FileDto> GetFile(string filePath);

        // Delete file
        Task<Boolean> DeleteFile(string filePath);
    }
}
