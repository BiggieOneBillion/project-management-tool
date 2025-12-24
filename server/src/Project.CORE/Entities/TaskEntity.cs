using Project.CORE.ValueObjects;

namespace Project.CORE.Entities;

public class TaskEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ProjectId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ValueObjects.TaskStatus Status { get; set; }
    public TaskType Type { get; set; }
    public Priority Priority { get; set; }
    public string? AssigneeId { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation Properties
    public ProjectEntity Project { get; set; } = null!;
    public User? Assignee { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Note> Notes { get; set; } = new List<Note>();
}

public class Comment
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string TaskId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? ParentId { get; set; }
    public int Level { get; set; } = 0;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation Properties
    public TaskEntity Task { get; set; } = null!;
    public User User { get; set; } = null!;
    public Comment? Parent { get; set; }
    public ICollection<Comment> Replies { get; set; } = new List<Comment>();
}
