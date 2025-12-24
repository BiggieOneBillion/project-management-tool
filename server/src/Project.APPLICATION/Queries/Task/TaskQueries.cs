using MediatR;
using Project.APPLICATION.DTOs.Task;
using Project.APPLICATION.Results;

namespace Project.APPLICATION.Queries.Task;

public record GetTaskByIdQuery(string Id, bool IncludeComments = false) : IRequest<TaskDto?>;

public record GetAllTasksQuery() : IRequest<List<TaskDto>>;

public record GetProjectTasksQuery(string ProjectId) : IRequest<List<TaskDto>>;

public record GetUserTasksQuery(string UserId) : IRequest<List<TaskDto>>;
public record GetTasksForUserInWorkspaceQuery(string WorkspaceId, string UserId) : IRequest<Result<List<TaskDto>>>;
