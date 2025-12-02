using Project.APPLICATION.DTOs.User;

namespace Project.APPLICATION.DTOs.Project;

public record ProjectDto(
    string Id,
    string Name,
    string Description,
    string Priority,
    string Status,
    DateTime StartDate,
    DateTime EndDate,
    string TeamLeadId,
    string WorkspaceId,
    int Progress,
    int TaskCount,
    int MemberCount,
    UserDto? TeamLead,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record ProjectDetailDto(
    string Id,
    string Name,
    string Description,
    string Priority,
    string Status,
    DateTime StartDate,
    DateTime EndDate,
    string TeamLeadId,
    string WorkspaceId,
    int Progress,
    UserDto TeamLead,
    List<ProjectMemberDto> Members,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record ProjectMemberDto(
    string Id,
    string UserId,
    string ProjectId,
    DateTime AddedAt,
    UserDto User
);
