using FileStorage.Domain.Entities;
using FileStorage.Domain.Interfaces;

namespace FileStorage.Application.Services
{
    public class InMemoryFileRepository : IFileRepository
    {
        private readonly Dictionary<Guid, FileEntity> _files = new();

        public Task<FileEntity?> GetByIdAsync(Guid id)
        {
            _files.TryGetValue(id, out var file);
            return Task.FromResult(file);
        }

        public Task<IEnumerable<FileEntity>> GetByUserIdAsync(Guid userId)
        {
            var files = _files.Values.Where(f => f.UserId == userId);
            return Task.FromResult(files);
        }

        public Task<Guid> AddAsync(FileEntity file)
        {
            _files[file.Id] = file;
            return Task.FromResult(file.Id);
        }

        public Task DeleteAsync(Guid id)
        {
            _files.Remove(id);
            return Task.CompletedTask;
        }
    }
}