using Project.CORE.ValueObjects;

namespace Project.APPLICATION.DTOs.Invitation;

public record InviteToWorkspaceRequest(
    string WorkspaceId,
    string Email,
    WorkspaceRole Role = WorkspaceRole.MEMBER
);

public record InviteToProjectRequest(
    string ProjectId,
    string Email
);
