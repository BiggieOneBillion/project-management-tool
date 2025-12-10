using Project.APPLICATION.DTOs.User;

namespace Project.APPLICATION.DTOs.Comment;

public class CommentDto
{
    public string Id { get; init; } = "";
    public string TaskId { get; init; } = "";
    public string UserId { get; init; } = "";
    public string Content { get; init; } = "";
    public string? ParentId { get; init; }
    public int Level { get; init; } = 0;
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public UserDto? User { get; init; }
    public List<CommentDto> Replies { get; init; } = new();
    public bool CanEdit { get; init; }
    public bool CanDelete { get; init; }
}
