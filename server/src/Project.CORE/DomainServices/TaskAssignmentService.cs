using Project.CORE.Entities;
using Project.CORE.Exceptions;

namespace Project.CORE.DomainServices;

/// <summary>
/// Domain service for task assignment business logic
/// </summary>
public class TaskAssignmentService
{
    /// <summary>
    /// Assign a task to a user with validation
    /// </summary>
    public void AssignTask(TaskEntity task, User assignee, ProjectEntity project)
    {
        if (task == null)
            throw new ArgumentNullException(nameof(task));
        
        if (assignee == null)
            throw new ArgumentNullException(nameof(assignee));
        
        if (project == null)
            throw new ArgumentNullException(nameof(project));
        
        // Business rule: Task must belong to the project
        if (task.ProjectId != project.Id)
        {
            throw new BusinessRuleViolationException(
                "TaskAssignment",
                "Task does not belong to the specified project");
        }
        
        // Business rule: Assignee must be a project member or team lead
        var isTeamLead = project.TeamLeadId == assignee.Id;
        var isMember = project.Members?.Any(m => m.UserId == assignee.Id) ?? false;
        
        if (!isTeamLead && !isMember)
        {
            throw new BusinessRuleViolationException(
                "TaskAssignment",
                $"User {assignee.Name} is not a member of project {project.Name}");
        }
        
        // Assign the task
        task.AssigneeId = assignee.Id;
        task.UpdatedAt = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Unassign a task
    /// </summary>
    public void UnassignTask(TaskEntity task)
    {
        if (task == null)
            throw new ArgumentNullException(nameof(task));
        
        task.AssigneeId = null;
        task.UpdatedAt = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Reassign a task to a different user
    /// </summary>
    public void ReassignTask(TaskEntity task, User newAssignee, ProjectEntity project)
    {
        if (task == null)
            throw new ArgumentNullException(nameof(task));
        
        // Unassign first
        UnassignTask(task);
        
        // Then assign to new user
        AssignTask(task, newAssignee, project);
    }
    
    /// <summary>
    /// Check if a user can be assigned to a task
    /// </summary>
    public bool CanAssignTask(User user, ProjectEntity project)
    {
        if (user == null || project == null)
            return false;
        
        var isTeamLead = project.TeamLeadId == user.Id;
        var isMember = project.Members?.Any(m => m.UserId == user.Id) ?? false;
        
        return isTeamLead || isMember;
    }
    
    /// <summary>
    /// Get all tasks assigned to a user in a project
    /// </summary>
    public IEnumerable<TaskEntity> GetUserTasksInProject(User user, ProjectEntity project)
    {
        if (user == null || project == null)
            return Enumerable.Empty<TaskEntity>();
        
        return project.Tasks?.Where(t => t.AssigneeId == user.Id) ?? Enumerable.Empty<TaskEntity>();
    }
}
