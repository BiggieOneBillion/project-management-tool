using MediatR;
using Project.APPLICATION.DTOs.Project;
using Project.APPLICATION.DTOs.Workspace;

namespace Project.APPLICATION.Queries.Project;

public record GetProjectByIdQuery(string Id, bool IncludeTasks = false, bool IncludeMembers = false) 
    : IRequest<ProjectDto?>;

public record GetAllProjectsQuery() : IRequest<List<ProjectDto>>;

public record GetWorkspaceProjectsQuery(string WorkspaceId, bool IncludeTasks = false) 
    : IRequest<List<ProjectDto>>;

public record GetProjectMembersQuery(string ProjectId) 
    : IRequest<List<ProjectMemberDto>>;

public record GetAvailableWorkspaceMembersQuery(string ProjectId, string WorkspaceId) 
    : IRequest<List<WorkspaceMemberDto>>;
