using Project.CORE.ValueObjects;

namespace Project.CORE.Entities;

public class ProjectEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Priority Priority { get; set; }
    public ProjectStatus Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? TeamLeadId { get; set; }
    public string WorkspaceId { get; set; } = string.Empty;
    public int Progress { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation Properties
    public User? TeamLead { get; set; }
    public Workspace Workspace { get; set; } = null!;
    public ICollection<ProjectMember> Members { get; set; } = new List<ProjectMember>();
    public ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
}

public class ProjectMember
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; } = string.Empty;
    public string ProjectId { get; set; } = string.Empty;
    public DateTime AddedAt { get; set; }
    
    // Navigation Properties
    public User User { get; set; } = null!;
    public ProjectEntity Project { get; set; } = null!;
}
