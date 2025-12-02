namespace Project.APPLICATION.DTOs.User;

public record UserDto(
    string Id,
    string Name,
    string Email,
    string? ImageUrl,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
