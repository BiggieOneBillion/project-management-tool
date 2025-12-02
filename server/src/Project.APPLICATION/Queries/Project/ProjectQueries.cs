using MediatR;
using Project.APPLICATION.DTOs.Project;

namespace Project.APPLICATION.Queries.Project;

public record GetProjectByIdQuery(string Id, bool IncludeTasks = false, bool IncludeMembers = false) 
    : IRequest<ProjectDto?>;

public record GetAllProjectsQuery() : IRequest<List<ProjectDto>>;

public record GetWorkspaceProjectsQuery(string WorkspaceId) : IRequest<List<ProjectDto>>;
