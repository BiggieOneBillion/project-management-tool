using MediatR;
using Project.APPLICATION.DTOs.Task;

namespace Project.APPLICATION.Commands.Task;

public record CreateTaskCommand(
    string ProjectId,
    string Title,
    string Description,
    string Type,
    string Priority,
    string Status,
    string? AssigneeId,
    DateTime DueDate
) : IRequest<TaskDto>;

public record UpdateTaskCommand(
    string Id,
    string Title,
    string Description,
    string Status,
    string Type,
    string Priority,
    string? AssigneeId,
    DateTime? DueDate
) : IRequest<TaskDto>;

public record DeleteTaskCommand(string Id) : IRequest<Unit>;

public record BulkDeleteTasksCommand(List<string> TaskIds) : IRequest<int>;
