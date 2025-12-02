namespace Project.CORE.DomainEvents;

public class WorkspaceMemberAddedEvent : DomainEvent
{
    public string WorkspaceId { get; }
    public string WorkspaceName { get; }
    public string UserId { get; }
    public string UserName { get; }
    public string Role { get; }
    
    public WorkspaceMemberAddedEvent(string workspaceId, string workspaceName, string userId, string userName, string role)
    {
        WorkspaceId = workspaceId;
        WorkspaceName = workspaceName;
        UserId = userId;
        UserName = userName;
        Role = role;
    }
}
