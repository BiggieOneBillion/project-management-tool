using MediatR;
using Project.APPLICATION.DTOs.Auth;

namespace Project.APPLICATION.Commands.Auth;

public record LoginCommand(
    string Email,
    string Password
) : IRequest<AuthResponse>;

public record RegisterCommand(
    string Name,
    string Email,
    string Password,
    string? InvitationToken = null
) : IRequest<AuthResponse>;

public record RefreshTokenCommand(
    string RefreshToken
) : IRequest<AuthResponse>;

public record LogoutCommand(
    string UserId
) : IRequest<Unit>;
