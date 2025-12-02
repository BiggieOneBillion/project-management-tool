using MediatR;
using Project.APPLICATION.DTOs.User;

namespace Project.APPLICATION.Commands.User;

public record CreateUserCommand(
    string Name,
    string Email,
    string? ImageUrl
) : IRequest<UserDto>;

public record UpdateUserCommand(
    string Id,
    string Name,
    string? ImageUrl
) : IRequest<UserDto>;

public record DeleteUserCommand(string Id) : IRequest<Unit>;
