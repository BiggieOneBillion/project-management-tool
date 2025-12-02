using Project.APPLICATION.DTOs.User;
using Project.APPLICATION.DTOs.Comment;

namespace Project.APPLICATION.DTOs.Task;

public record TaskDto(
    string Id,
    string ProjectId,
    string Title,
    string Description,
    string Status,
    string Type,
    string Priority,
    string? AssigneeId,
    DateTime DueDate,
    int CommentCount,
    UserDto? Assignee,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record TaskDetailDto(
    string Id,
    string ProjectId,
    string Title,
    string Description,
    string Status,
    string Type,
    string Priority,
    string? AssigneeId,
    DateTime DueDate,
    UserDto? Assignee,
    List<CommentDto> Comments,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
