namespace Project.APPLICATION.DTOs.Auth;

public record LoginRequest(
    string Email,
    string Password
);
