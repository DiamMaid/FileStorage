using System.Text.Json;
using FileStorage.Domain.Entities;
using FileStorage.Domain.Interfaces;

namespace FileStorage.Application.Services
{
    public class FileBasedUserRepository : IUserRepository
    {
        private readonly string _filePath;
        private readonly object _gate = new();
        private readonly Dictionary<Guid, User> _users;

        public FileBasedUserRepository(string storagePath)
        {
            _filePath = Path.Combine(storagePath, "users.json");
            _users = File.Exists(_filePath)
                ? JsonSerializer.Deserialize<Dictionary<Guid, User>>(File.ReadAllText(_filePath)) ?? new Dictionary<Guid, User>()
                : new Dictionary<Guid, User>();
        }

        public Task<User?> GetByIdAsync(Guid id)
        {
            lock (_gate)
            {
                _users.TryGetValue(id, out var user);
                return Task.FromResult(user);
            }
        }

        public Task<User?> GetByUsernameAsync(string username)
        {
            lock (_gate)
            {
                var user = _users.Values.FirstOrDefault(u => u.Username == username);
                return Task.FromResult(user);
            }
        }

        public Task<Guid> AddAsync(User user)
        {
            lock (_gate)
            {
                _users[user.Id] = user;
                Save();
                return Task.FromResult(user.Id);
            }
        }

        private void Save()
        {
            File.WriteAllText(_filePath, JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}