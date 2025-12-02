using Project.CORE.ValueObjects;

namespace Project.CORE.Entities;

public class Workspace
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string Settings { get; set; } = "{}";
    public string OwnerId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation Properties
    public User Owner { get; set; } = null!;
    public ICollection<WorkspaceMember> Members { get; set; } = new List<WorkspaceMember>();
    public ICollection<ProjectEntity> Projects { get; set; } = new List<ProjectEntity>();
}

public class WorkspaceMember
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; } = string.Empty;
    public string WorkspaceId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public WorkspaceRole Role { get; set; }
    public DateTime JoinedAt { get; set; }
    
    // Navigation Properties
    public User User { get; set; } = null!;
    public Workspace Workspace { get; set; } = null!;
}
