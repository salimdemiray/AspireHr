using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Login.Application.DTOs;
using Auth.Login.Application.Interfaces;
using Auth.Login.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Login.Application.Services;

public class AuthService : IAuthService
{
    private readonly IApplicationUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IApplicationUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
        if (user == null || !user.IsActive)
            return null;

        // Basit başlangıç için BCrypt ile doğrulama
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return null;

        user.LastLoginAt = DateTime.UtcNow;
        var token = GenerateJwtToken(user);
        var expiresAt = DateTime.UtcNow.AddMinutes(GetExpiryMinutes());

        // Şu an için basit yetki listesi döndürülüyor
        var permissions = new List<string> { "*" };

        return new LoginResponse(
            token,
            expiresAt,
            new UserInfoDto(user.Id, user.Username, user.Email, user.FullName, permissions)
        );
    }

    public async Task<UserInfoDto?> GetCurrentUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null || !user.IsActive)
            return null;

        var permissions = new List<string> { "*" };
        return new UserInfoDto(user.Id, user.Username, user.Email, user.FullName, permissions);
    }

    private string GenerateJwtToken(Domain.Entities.ApplicationUser user)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "AspireHr_SuperSecretKey_ChangeMe_123456789");
        var issuer = _configuration["Jwt:Issuer"] ?? "AspireHr.Auth";
        var audience = _configuration["Jwt:Audience"] ?? "AspireHr.Client";

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.Username),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("fullName", user.FullName ?? string.Empty)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(GetExpiryMinutes()),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private int GetExpiryMinutes()
    {
        if (int.TryParse(_configuration["Jwt:ExpiryMinutes"], out var minutes))
            return minutes;
        return 60;
    }
}
