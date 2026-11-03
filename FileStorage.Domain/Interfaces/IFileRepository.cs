using FileStorage.Domain.Entities;

namespace FileStorage.Domain.Interfaces
{
    public interface IFileRepository
    {
        Task<FileEntity?> GetByIdAsync(Guid id);
        Task<IEnumerable<FileEntity>> GetByUserIdAsync(Guid userId);
        Task<Guid> AddAsync(FileEntity file);
        Task DeleteAsync(Guid id);
    }
}
