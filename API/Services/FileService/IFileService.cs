using API.DTOs.FileSystem;

namespace API.Services.FileService
{
    public interface IFileService
    {
        Task<string> ConvertFileStreamToBase64(Stream fileStream);

        string ConvertFileToBase64(string filePath, FileSystemFileType fileType);

        string GetMimeType(string filePath);
    }
}
