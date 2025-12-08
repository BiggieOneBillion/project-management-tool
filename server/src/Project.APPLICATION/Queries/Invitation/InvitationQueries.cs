using MediatR;
using Project.APPLICATION.DTOs.Invitation;
using Project.CORE.Enums;

namespace Project.APPLICATION.Queries.Invitation;

public record GetWorkspaceInvitationsQuery(
    string WorkspaceId, 
    InvitationStatus? Status = InvitationStatus.PENDING
) : IRequest<List<InvitationDto>>;

public record GetUserPendingInvitationsQuery(
    string Email
) : IRequest<List<InvitationDto>>;
