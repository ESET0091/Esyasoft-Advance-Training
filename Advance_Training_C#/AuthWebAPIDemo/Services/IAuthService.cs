using AuthWebAPIDemo.Entities;
using AuthWebAPIDemo.Model;

namespace AuthWebAPIDemo.Services
{
    public interface IAuthService
    {
        Task<TokenResponseDto?> LoginAsync(UserDto request);
        Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);

        Task<User?> RegisterAsync(UserDto request);
    }
}