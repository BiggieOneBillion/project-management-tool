using Project.APPLICATION.DTOs.User;

namespace Project.APPLICATION.DTOs.Comment;

public class CommentDto
{
    public string Id { get; init; } = "";
    public string TaskId { get; init; } = "";
    public string UserId { get; init; } = "";
    public string Content { get; init; } = "";
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public UserDto? User { get; init; }
}
