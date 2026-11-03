using FileStorage.Application.DTOs;

namespace FileStorage.Application.Interfaces
{
    public interface IFileService
    {
        Task<FileDto> UploadAsync(string fileName, string contentType, long sizeBytes, Guid userId, Stream content);
        Task<(Stream Stream, string ContentType, string FileName)?> DownloadAsync(Guid fileId, Guid userId);
        Task<IEnumerable<FileDto>> GetFilesAsync(Guid userId);
        Task DeleteAsync(Guid fileId, Guid userId);
    }
}
