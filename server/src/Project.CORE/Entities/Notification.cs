namespace Project.CORE.Entities;

public class Notification
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // "MENTION", "COMMENT", "TASK_ASSIGNED", etc.
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? RelatedEntityId { get; set; } // NoteId, CommentId, TaskId, etc.
    public string? RelatedEntityType { get; set; } // "Note", "Comment", "Task", etc.
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    
    // Navigation Properties
    public User User { get; set; } = null!;
}
