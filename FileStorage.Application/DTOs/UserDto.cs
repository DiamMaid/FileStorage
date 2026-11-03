namespace FileStorage.Application.DTOs
{
    /// <summary>
    /// DTO для представления информации о пользователе.
    /// </summary>
    public class UserDto
    {
        public Guid Id { get; init; }
        public string Username { get; init; }
        public string Role { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
