using MediatR;
using Project.APPLICATION.DTOs.Project;

namespace Project.APPLICATION.Commands.Project;

public record CreateProjectCommand(
    string Name,
    string Description,
    string Priority,
    string Status,
    DateTime StartDate,
    DateTime EndDate,
    string TeamLeadId,
    string WorkspaceId
) : IRequest<ProjectDto>;

public record UpdateProjectCommand(
    string Id,
    string Name,
    string Description,
    string Priority,
    string Status,
    DateTime? StartDate,
    DateTime? EndDate,
    int? Progress
) : IRequest<ProjectDto>;

public record DeleteProjectCommand(string Id) : IRequest<Unit>;
