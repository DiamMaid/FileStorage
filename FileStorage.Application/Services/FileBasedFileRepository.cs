using System.Text.Json;
using FileStorage.Domain.Entities;
using FileStorage.Domain.Interfaces;

namespace FileStorage.Application.Services
{
    public class FileBasedFileRepository : IFileRepository
    {
        private readonly string _filePath;
        private readonly object _gate = new();
        private readonly Dictionary<Guid, FileEntity> _files;

        public FileBasedFileRepository(string storagePath)
        {
            _filePath = Path.Combine(storagePath, "files.json");
            _files = File.Exists(_filePath)
                ? JsonSerializer.Deserialize<Dictionary<Guid, FileEntity>>(File.ReadAllText(_filePath)) ?? new Dictionary<Guid, FileEntity>()
                : new Dictionary<Guid, FileEntity>();
        }

        public Task<FileEntity?> GetByIdAsync(Guid id)
        {
            lock (_gate)
            {
                _files.TryGetValue(id, out var file);
                return Task.FromResult(file);
            }
        }

        public Task<IEnumerable<FileEntity>> GetByUserIdAsync(Guid userId)
        {
            lock (_gate)
            {
                var files = _files.Values.Where(f => f.UserId == userId).ToList();
                return Task.FromResult<IEnumerable<FileEntity>>(files);
            }
        }

        public Task<Guid> AddAsync(FileEntity file)
        {
            lock (_gate)
            {
                _files[file.Id] = file;
                Save();
                return Task.FromResult(file.Id);
            }
        }

        public Task DeleteAsync(Guid id)
        {
            lock (_gate)
            {
                _files.Remove(id);
                Save();
                return Task.CompletedTask;
            }
        }

        private void Save()
        {
            File.WriteAllText(_filePath, JsonSerializer.Serialize(_files, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}