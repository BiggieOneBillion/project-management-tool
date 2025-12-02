using Project.CORE.Entities;
using Project.CORE.ValueObjects;

namespace Project.CORE.Aggregates;

/// <summary>
/// Project Aggregate Root - Encapsulates project business logic
/// </summary>
public class ProjectAggregate
{
    private readonly ProjectEntity _project;
    private readonly List<ProjectMember> _members;
    private readonly List<TaskEntity> _tasks;
    
    public ProjectAggregate(ProjectEntity project)
    {
        _project = project ?? throw new ArgumentNullException(nameof(project));
        _members = new List<ProjectMember>();
        _tasks = new List<TaskEntity>();
    }
    
    public ProjectEntity Project => _project;
    public IReadOnlyList<ProjectMember> Members => _members.AsReadOnly();
    public IReadOnlyList<TaskEntity> Tasks => _tasks.AsReadOnly();
    
    /// <summary>
    /// Add a member to the project
    /// </summary>
    public void AddMember(string userId)
    {
        // Business rule: Cannot add duplicate members
        if (_members.Any(m => m.UserId == userId))
        {
            throw new InvalidOperationException($"User {userId} is already a member of this project");
        }
        
        var member = new ProjectMember
        {
            Id = Guid.NewGuid().ToString(),
            UserId = userId,
            ProjectId = _project.Id,
            AddedAt = DateTime.UtcNow
        };
        
        _members.Add(member);
    }
    
    /// <summary>
    /// Remove a member from the project
    /// </summary>
    public void RemoveMember(string userId)
    {
        // Business rule: Cannot remove the team lead
        if (userId == _project.TeamLeadId)
        {
            throw new InvalidOperationException("Cannot remove the project team lead");
        }
        
        var member = _members.FirstOrDefault(m => m.UserId == userId);
        if (member != null)
        {
            _members.Remove(member);
        }
    }
    
    /// <summary>
    /// Add a task to the project
    /// </summary>
    public void AddTask(TaskEntity task)
    {
        if (task.ProjectId != _project.Id)
        {
            throw new InvalidOperationException("Task does not belong to this project");
        }
        
        _tasks.Add(task);
        UpdateProgress();
    }
    
    /// <summary>
    /// Update task status
    /// </summary>
    public void UpdateTaskStatus(string taskId, ValueObjects.TaskStatus newStatus)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == taskId);
        if (task == null)
        {
            throw new InvalidOperationException($"Task {taskId} not found in project");
        }
        
        task.Status = newStatus;
        UpdateProgress();
    }
    
    /// <summary>
    /// Calculate and update project progress based on completed tasks
    /// </summary>
    public void UpdateProgress()
    {
        if (_tasks.Count == 0)
        {
            _project.Progress = 0;
            return;
        }
        
        var completedTasks = _tasks.Count(t => t.Status == ValueObjects.TaskStatus.DONE);
        _project.Progress = (int)((completedTasks / (double)_tasks.Count) * 100);
    }
    
    /// <summary>
    /// Change project status
    /// </summary>
    public void ChangeStatus(ProjectStatus newStatus)
    {
        // Business rule: Cannot go from COMPLETED to PLANNING
        if (_project.Status == ProjectStatus.COMPLETED && newStatus == ProjectStatus.PLANNING)
        {
            throw new InvalidOperationException("Cannot change completed project back to planning");
        }
        
        _project.Status = newStatus;
    }
    
    /// <summary>
    /// Check if user is a member
    /// </summary>
    public bool IsMember(string userId)
    {
        return _project.TeamLeadId == userId || _members.Any(m => m.UserId == userId);
    }
    
    /// <summary>
    /// Check if user is the team lead
    /// </summary>
    public bool IsTeamLead(string userId)
    {
        return _project.TeamLeadId == userId;
    }
    
    /// <summary>
    /// Get task statistics
    /// </summary>
    public (int total, int todo, int inProgress, int done, int blocked) GetTaskStatistics()
    {
        return (
            total: _tasks.Count,
            todo: _tasks.Count(t => t.Status == ValueObjects.TaskStatus.TODO),
            inProgress: _tasks.Count(t => t.Status == ValueObjects.TaskStatus.IN_PROGRESS),
            done: _tasks.Count(t => t.Status == ValueObjects.TaskStatus.DONE),
            blocked: _tasks.Count(t => t.Status == ValueObjects.TaskStatus.BLOCKED)
        );
    }
}
