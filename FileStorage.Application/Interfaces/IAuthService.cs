using FileStorage.Application.DTOs;

namespace FileStorage.Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto> RegisterAsync(RegisterRequest request);
        Task<UserDto> LoginAsync(LoginRequest request);
    }
}
