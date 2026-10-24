using Auth.Login.Application.DTOs;

namespace Auth.Login.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<UserInfoDto?> GetCurrentUserAsync(string userId, CancellationToken cancellationToken = default);
}
