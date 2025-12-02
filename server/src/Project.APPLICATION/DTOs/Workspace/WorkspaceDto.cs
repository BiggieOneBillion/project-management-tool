using Project.APPLICATION.DTOs.User;

namespace Project.APPLICATION.DTOs.Workspace;

public record WorkspaceDto(
    string Id,
    string Name,
    string Slug,
    string? Description,
    string? ImageUrl,
    string OwnerId,
    int MemberCount,
    int ProjectCount,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record WorkspaceDetailDto(
    string Id,
    string Name,
    string Slug,
    string? Description,
    string? ImageUrl,
    string Settings,
    string OwnerId,
    UserDto Owner,
    List<WorkspaceMemberDto> Members,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record WorkspaceMemberDto(
    string Id,
    string UserId,
    string WorkspaceId,
    string Role,
    DateTime JoinedAt,
    UserDto User
);
