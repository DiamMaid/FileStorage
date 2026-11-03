namespace FileStorage.Application.DTOs
{
    /// <summary>
    /// Запрос на регистрацию нового пользователя.
    /// </summary>
    public class RegisterRequest                  
    {
        public string Username { get; init; }      
        public string Password { get; init; }       
    }
}
