using MediatR;
using Project.APPLICATION.DTOs.Invitation;
using Project.CORE.ValueObjects;

namespace Project.APPLICATION.Commands.Invitation;

public record InviteToWorkspaceCommand(
    string WorkspaceId,
    string Email,
    WorkspaceRole Role,
    string InvitedById
) : IRequest<InvitationDto>;

public record InviteToProjectCommand(
    string ProjectId,
    string Email,
    string InvitedById
) : IRequest<InvitationDto>;

public record AcceptInvitationCommand(
    string Token,
    string? UserId = null
) : IRequest<InvitationDto>;

public record RevokeInvitationCommand(
    string InvitationId,
    string UserId
) : IRequest<Unit>;
