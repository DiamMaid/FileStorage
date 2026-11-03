using FileStorage.Domain.Enums;

namespace FileStorage.Domain.Entities
{
    /// <summary>
    /// Представляет пользователя системы с учётными данными и ролью.
    /// </summary>
    public class User
    {
        public Guid Id { get; init; }
        public string Username { get; init; }
        public string PasswordHash { get; init; }
        public UserRole Role { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
