using Project.APPLICATION.DTOs.User;
using Project.APPLICATION.DTOs.Comment;

namespace Project.APPLICATION.DTOs.Task;

public class TaskDto
{
    public string Id { get; init; } = "";
    public string ProjectId { get; init; } = "";
    public string Title { get; init; } = "";
    public string Description { get; init; } = "";
    public string Status { get; init; } = "";
    public string Type { get; init; } = "";
    public string Priority { get; init; } = "";
    public string? AssigneeId { get; init; }
    public DateTime DueDate { get; init; }
    public int CommentCount { get; init; }
    public UserDto? Assignee { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}

public class TaskDetailDto
{
    public string Id { get; init; } = "";
    public string ProjectId { get; init; } = "";
    public string Title { get; init; } = "";
    public string Description { get; init; } = "";
    public string Status { get; init; } = "";
    public string Type { get; init; } = "";
    public string Priority { get; init; } = "";
    public string? AssigneeId { get; init; }
    public DateTime DueDate { get; init; }
    public UserDto? Assignee { get; init; }
    public List<CommentDto>? Comments { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
