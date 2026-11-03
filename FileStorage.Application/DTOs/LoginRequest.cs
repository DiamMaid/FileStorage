namespace FileStorage.Application.DTOs
{
    /// <summary>
    /// Запрос на вход в систему.
    /// </summary>
    public class LoginRequest
    {
        public string Username { get; init; }
        public string Password { get; init; }
    }
}
