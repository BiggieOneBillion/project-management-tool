namespace Project.APPLICATION.DTOs.User;

public record CreateUserDto(
    string Name,
    string Email,
    string? ImageUrl
);
