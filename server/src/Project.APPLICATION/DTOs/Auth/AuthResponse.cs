using Project.APPLICATION.DTOs.User;

namespace Project.APPLICATION.DTOs.Auth;

public record AuthResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt,
    UserDto User
);
