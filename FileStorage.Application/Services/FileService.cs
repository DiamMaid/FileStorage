using FileStorage.Application.DTOs;
using FileStorage.Application.Interfaces;
using FileStorage.Domain.Entities;
using FileStorage.Domain.Interfaces;

namespace FileStorage.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly string _storagePath;

        public FileService(IFileRepository fileRepository, string storagePath)
        {
            _fileRepository = fileRepository;
            _storagePath = storagePath;
        }

        public async Task<FileDto> UploadAsync(string fileName, string contentType, long sizeBytes, Guid userId, Stream content)
        {
            var storedFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
            var fullPath = Path.Combine(_storagePath, storedFileName);

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await content.CopyToAsync(fileStream);
            }

            var entity = new FileEntity
            {
                Id = Guid.NewGuid(),
                FileName = fileName,
                StoredPath = fullPath,
                ContentType = contentType,
                SizeBytes = sizeBytes,
                UserId = userId,
                UploadedAt = DateTime.UtcNow
            };

            await _fileRepository.AddAsync(entity);

            return MapToDto(entity);
        }

        public async Task<(Stream Stream, string ContentType, string FileName)?> DownloadAsync(Guid fileId, Guid userId)
        {
            var file = await _fileRepository.GetByIdAsync(fileId);

            if (file == null || file.UserId != userId)
                return null;

            if (!File.Exists(file.StoredPath))
                return null;

            var stream = new FileStream(file.StoredPath, FileMode.Open, FileAccess.Read);
            return (stream, file.ContentType, file.FileName);
        }

        public async Task<IEnumerable<FileDto>> GetFilesAsync(Guid userId)
        {
            var files = await _fileRepository.GetByUserIdAsync(userId);
            return files.Select(MapToDto);
        }

        public async Task<bool> DeleteAsync(Guid fileId, Guid userId)
        {
            var file = await _fileRepository.GetByIdAsync(fileId);

            if (file == null || file.UserId != userId)
                return false;

            if (File.Exists(file.StoredPath))
                File.Delete(file.StoredPath);

            await _fileRepository.DeleteAsync(fileId);
            return true;
        }

        private static FileDto MapToDto(FileEntity entity)
        {
            return new FileDto
            {
                Id = entity.Id,
                FileName = entity.FileName,
                ContentType = entity.ContentType,
                SizeBytes = entity.SizeBytes,
                UploadedAt = entity.UploadedAt
            };
        }
    }
}
