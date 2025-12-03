using Project.APPLICATION.DTOs.User;

namespace Project.APPLICATION.DTOs.Project;

public class ProjectDto
{
    public string Id { get; init; } = "";
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public string Priority { get; init; } = "";
    public string Status { get; init; } = "";
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string TeamLeadId { get; init; } = "";
    public string WorkspaceId { get; init; } = "";
    public int Progress { get; init; }
    public int TaskCount { get; init; }
    public int MemberCount { get; init; }
    public UserDto? TeamLead { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}

public class ProjectDetailDto
{
    public string Id { get; init; } = "";
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public string Priority { get; init; } = "";
    public string Status { get; init; } = "";
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string TeamLeadId { get; init; } = "";
    public string WorkspaceId { get; init; } = "";
    public int Progress { get; init; }
    public UserDto? TeamLead { get; init; }
    public List<ProjectMemberDto>? Members { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}

public class ProjectMemberDto
{
    public string Id { get; init; } = "";
    public string UserId { get; init; } = "";
    public string ProjectId { get; init; } = "";
    public DateTime AddedAt { get; init; }
    public UserDto? User { get; init; }
}
