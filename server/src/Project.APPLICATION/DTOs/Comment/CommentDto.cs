using Project.APPLICATION.DTOs.User;

namespace Project.APPLICATION.DTOs.Comment;

public record CommentDto(
    string Id,
    string TaskId,
    string UserId,
    string Content,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    UserDto User
);
