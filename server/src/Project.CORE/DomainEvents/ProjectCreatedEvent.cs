namespace Project.CORE.DomainEvents;

public class ProjectCreatedEvent : DomainEvent
{
    public string ProjectId { get; }
    public string ProjectName { get; }
    public string WorkspaceId { get; }
    public string TeamLeadId { get; }
    
    public ProjectCreatedEvent(string projectId, string projectName, string workspaceId, string teamLeadId)
    {
        ProjectId = projectId;
        ProjectName = projectName;
        WorkspaceId = workspaceId;
        TeamLeadId = teamLeadId;
    }
}
