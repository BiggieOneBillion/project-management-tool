using Project.CORE.Entities;
using Project.CORE.ValueObjects;

namespace Project.CORE.Aggregates;

/// <summary>
/// Workspace Aggregate Root - Encapsulates workspace business logic
/// </summary>
public class WorkspaceAggregate
{
    private readonly Workspace _workspace;
    private readonly List<WorkspaceMember> _members;
    private readonly List<ProjectEntity> _projects;
    
    public WorkspaceAggregate(Workspace workspace)
    {
        _workspace = workspace ?? throw new ArgumentNullException(nameof(workspace));
        _members = new List<WorkspaceMember>();
        _projects = new List<ProjectEntity>();
    }
    
    public Workspace Workspace => _workspace;
    public IReadOnlyList<WorkspaceMember> Members => _members.AsReadOnly();
    public IReadOnlyList<ProjectEntity> Projects => _projects.AsReadOnly();
    
    /// <summary>
    /// Add a member to the workspace
    /// </summary>
    public void AddMember(string userId, WorkspaceRole role, string message = "")
    {
        // Business rule: Cannot add duplicate members
        if (_members.Any(m => m.UserId == userId))
        {
            throw new InvalidOperationException($"User {userId} is already a member of this workspace");
        }
        
        var member = new WorkspaceMember
        {
            Id = Guid.NewGuid().ToString(),
            UserId = userId,
            WorkspaceId = _workspace.Id,
            Role = role,
            Message = message,
            JoinedAt = DateTime.UtcNow
        };
        
        _members.Add(member);
    }
    
    /// <summary>
    /// Remove a member from the workspace
    /// </summary>
    public void RemoveMember(string userId)
    {
        // Business rule: Cannot remove the owner
        if (userId == _workspace.OwnerId)
        {
            throw new InvalidOperationException("Cannot remove the workspace owner");
        }
        
        var member = _members.FirstOrDefault(m => m.UserId == userId);
        if (member != null)
        {
            _members.Remove(member);
        }
    }
    
    /// <summary>
    /// Change member role
    /// </summary>
    public void ChangeMemberRole(string userId, WorkspaceRole newRole)
    {
        var member = _members.FirstOrDefault(m => m.UserId == userId);
        if (member == null)
        {
            throw new InvalidOperationException($"User {userId} is not a member of this workspace");
        }
        
        member.Role = newRole;
    }
    
    /// <summary>
    /// Add a project to the workspace
    /// </summary>
    public void AddProject(ProjectEntity project)
    {
        if (project.WorkspaceId != _workspace.Id)
        {
            throw new InvalidOperationException("Project does not belong to this workspace");
        }
        
        _projects.Add(project);
    }
    
    /// <summary>
    /// Check if user is a member
    /// </summary>
    public bool IsMember(string userId)
    {
        return _workspace.OwnerId == userId || _members.Any(m => m.UserId == userId);
    }
    
    /// <summary>
    /// Check if user is an admin
    /// </summary>
    public bool IsAdmin(string userId)
    {
        if (_workspace.OwnerId == userId) return true;
        
        var member = _members.FirstOrDefault(m => m.UserId == userId);
        return member?.Role == WorkspaceRole.ADMIN;
    }
}
