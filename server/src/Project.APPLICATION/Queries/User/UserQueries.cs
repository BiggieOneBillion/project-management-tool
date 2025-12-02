using MediatR;
using Project.APPLICATION.DTOs.User;

namespace Project.APPLICATION.Queries.User;

public record GetUserByIdQuery(string Id) : IRequest<UserDto?>;

public record GetAllUsersQuery() : IRequest<List<UserDto>>;

public record GetUserByEmailQuery(string Email) : IRequest<UserDto?>;
