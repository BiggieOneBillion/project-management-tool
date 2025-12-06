using Project.CORE.Enums;

namespace Project.APPLICATION.DTOs.Invitation;

public class InvitationDto
{
    public string Id { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Token { get; init; } = string.Empty;
    public InvitationType Type { get; init; }
    public string? WorkspaceId { get; init; }
    public string? ProjectId { get; init; }
    public string InvitedById { get; init; } = string.Empty;
    public InvitationStatus Status { get; init; }
    public DateTime ExpiresAt { get; init; }
    public DateTime CreatedAt { get; init; }
}
