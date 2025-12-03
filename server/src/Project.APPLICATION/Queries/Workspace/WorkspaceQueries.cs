using MediatR;
using Project.APPLICATION.DTOs.Workspace;

namespace Project.APPLICATION.Queries.Workspace;

public record GetWorkspaceByIdQuery(string Id, bool IncludeMembers = false, bool IncludeProjects = false) 
    : IRequest<WorkspaceDto?>;

public record GetAllWorkspacesQuery() : IRequest<List<WorkspaceDetailDto>>;

public record GetUserWorkspacesQuery(string UserId) : IRequest<List<WorkspaceDto>>;
