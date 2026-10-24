namespace Auth.Login.Application.DTOs;

public record LoginRequest(string Username, string Password);

public record LoginResponse(string Token, DateTime ExpiresAt, UserInfoDto User);

public record UserInfoDto(string Id, string Username, string Email, string? FullName, List<string> Permissions);
