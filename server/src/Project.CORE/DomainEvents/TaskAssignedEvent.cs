namespace Project.CORE.DomainEvents;

public class TaskAssignedEvent : DomainEvent
{
    public string TaskId { get; }
    public string TaskTitle { get; }
    public string AssigneeId { get; }
    public string AssigneeName { get; }
    public string ProjectId { get; }
    
    public TaskAssignedEvent(string taskId, string taskTitle, string assigneeId, string assigneeName, string projectId)
    {
        TaskId = taskId;
        TaskTitle = taskTitle;
        AssigneeId = assigneeId;
        AssigneeName = assigneeName;
        ProjectId = projectId;
    }
}
