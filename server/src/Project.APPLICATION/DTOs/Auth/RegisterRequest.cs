namespace Project.APPLICATION.DTOs.Auth;

public record RegisterRequest(
    string Name,
    string Email,
    string Password,
    string? InvitationToken = null
);
