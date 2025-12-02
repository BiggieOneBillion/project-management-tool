namespace Project.APPLICATION.DTOs.Task;

public record UpdateTaskDto(
    string Title,
    string Description,
    string Status,
    string Type,
    string Priority,
    string? AssigneeId,
    DateTime? DueDate
);
