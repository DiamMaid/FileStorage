using FileStorage.Domain.Entities;

namespace FileStorage.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByUsernameAsync(string username);
        Task<Guid> AddAsync(User user);
    }
}
