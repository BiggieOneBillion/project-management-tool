using Project.CORE.Enums;

namespace Project.CORE.Entities;

public class Invitation
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public InvitationType Type { get; set; }
    public string? WorkspaceId { get; set; }
    public string? ProjectId { get; set; }
    public string InvitedById { get; set; } = string.Empty;
    public InvitationStatus Status { get; set; } = InvitationStatus.PENDING;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Navigation Properties
    public Workspace? Workspace { get; set; }
    public ProjectEntity? Project { get; set; }
    public User InvitedBy { get; set; } = null!;
}
