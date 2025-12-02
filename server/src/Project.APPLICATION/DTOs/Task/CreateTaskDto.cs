namespace Project.APPLICATION.DTOs.Task;

public record CreateTaskDto(
    string ProjectId,
    string Title,
    string Description,
    string Type,
    string Priority,
    string Status,
    string? AssigneeId,
    DateTime DueDate
);
