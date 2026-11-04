using FileStorage.Application.DTOs;
using FileStorage.Application.Interfaces;
using FileStorage.Domain.Entities;
using FileStorage.Domain.Enums;
using FileStorage.Domain.Interfaces;

namespace FileStorage.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> LoginAsync(LoginRequest request)
        {
            ValidateCredentials(request.Username, request.Password);

            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null)
                throw new InvalidOperationException("Неверный логин или пароль");

            if (!PasswordHasher.Verify(request.Password, user.PasswordHash))
                throw new InvalidOperationException("Неверный логин или пароль");

            return MapToDto(user);
        }

        public async Task<UserDto> RegisterAsync(RegisterRequest request)
        {
            ValidateCredentials(request.Username, request.Password);

            var existing = await _userRepository.GetByUsernameAsync(request.Username);

            if (existing != null)
                throw new InvalidOperationException("Пользователь уже существует");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                PasswordHash = PasswordHasher.Hash(request.Password),
                Role = UserRole.User,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);

            return MapToDto(user);
        }

        private static void ValidateCredentials(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || username.Length < 3)
                throw new InvalidOperationException("Имя пользователя должно содержать не менее 3 символов");

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                throw new InvalidOperationException("Пароль должен содержать не менее 6 символов");
        }

        private static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role.ToString(),
                CreatedAt = user.CreatedAt
            };
        }
    }
}
