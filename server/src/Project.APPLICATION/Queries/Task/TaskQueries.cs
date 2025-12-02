using MediatR;
using Project.APPLICATION.DTOs.Task;

namespace Project.APPLICATION.Queries.Task;

public record GetTaskByIdQuery(string Id, bool IncludeComments = false) : IRequest<TaskDto?>;

public record GetAllTasksQuery() : IRequest<List<TaskDto>>;

public record GetProjectTasksQuery(string ProjectId) : IRequest<List<TaskDto>>;

public record GetUserTasksQuery(string UserId) : IRequest<List<TaskDto>>;
