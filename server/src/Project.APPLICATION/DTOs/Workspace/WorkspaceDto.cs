using Project.APPLICATION.DTOs.Project;
using Project.APPLICATION.DTOs.User;

namespace Project.APPLICATION.DTOs.Workspace;

public class WorkspaceDto
{
    public string Id { get; init; } = "";
    public string Name { get; init; } = "";
    public string Slug { get; init; } = "";
    public string? Description { get; init; }
    public string? ImageUrl { get; init; }
    public string OwnerId { get; init; } = "";
    public int MemberCount { get; init; }
    public List<WorkspaceMemberDto>? Members { get; init; }
    public List<ProjectDto>? Projects { get; init; }
    public int ProjectCount { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}

public class WorkspaceDetailDto
{
    public string Id { get; init; } = "";
    public string Name { get; init; } = "";
    public string Slug { get; init; } = "";
    public string? Description { get; init; }
    public string? ImageUrl { get; init; }
    public string Settings { get; init; } = "";
    public string OwnerId { get; init; } = "";
    public UserDto? Owner { get; init; }
    public List<WorkspaceMemberDto>? Members { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}

public class WorkspaceMemberDto
{
    public string Id { get; init; } = "";
    public string UserId { get; init; } = "";
    public string WorkspaceId { get; init; } = "";
    public string Role { get; init; } = "";
    public DateTime JoinedAt { get; init; }
    public UserDto? User { get; init; }
}
