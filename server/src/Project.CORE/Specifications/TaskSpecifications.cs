using System.Linq.Expressions;
using Project.CORE.Entities;
using Project.CORE.ValueObjects;

namespace Project.CORE.Specifications;

/// <summary>
/// Base specification class
/// </summary>
public abstract class Specification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>> Criteria { get; protected set; } = null!;
    public List<Expression<Func<T, object>>> Includes { get; } = new();
    public Expression<Func<T, object>>? OrderBy { get; protected set; }
    public Expression<Func<T, object>>? OrderByDescending { get; protected set; }
    
    public bool IsSatisfiedBy(T entity)
    {
        return Criteria.Compile()(entity);
    }
    
    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }
}

/// <summary>
/// Specification for tasks by status
/// </summary>
public class TasksByStatusSpecification : Specification<TaskEntity>
{
    public TasksByStatusSpecification(ValueObjects.TaskStatus status)
    {
        Criteria = task => task.Status == status;
    }
}

/// <summary>
/// Specification for tasks by project
/// </summary>
public class TasksByProjectSpecification : Specification<TaskEntity>
{
    public TasksByProjectSpecification(string projectId)
    {
        Criteria = task => task.ProjectId == projectId;
        AddInclude(task => task.Assignee!);
        AddInclude(task => task.Project);
    }
}

/// <summary>
/// Specification for tasks by assignee
/// </summary>
public class TasksByAssigneeSpecification : Specification<TaskEntity>
{
    public TasksByAssigneeSpecification(string assigneeId)
    {
        Criteria = task => task.AssigneeId == assigneeId;
        AddInclude(task => task.Project);
    }
}

/// <summary>
/// Specification for overdue tasks
/// </summary>
public class OverdueTasksSpecification : Specification<TaskEntity>
{
    public OverdueTasksSpecification()
    {
        Criteria = task => task.DueDate < DateTime.UtcNow && 
                          task.Status != ValueObjects.TaskStatus.DONE;
        OrderBy = task => task.DueDate;
    }
}

/// <summary>
/// Specification for high priority tasks
/// </summary>
public class HighPriorityTasksSpecification : Specification<TaskEntity>
{
    public HighPriorityTasksSpecification()
    {
        Criteria = task => task.Priority == Priority.HIGH;
        OrderBy = task => task.DueDate;
    }
}

/// <summary>
/// Specification for tasks due soon (within days)
/// </summary>
public class TasksDueSoonSpecification : Specification<TaskEntity>
{
    public TasksDueSoonSpecification(int daysAhead = 7)
    {
        var futureDate = DateTime.UtcNow.AddDays(daysAhead);
        Criteria = task => task.DueDate <= futureDate && 
                          task.DueDate >= DateTime.UtcNow &&
                          task.Status != ValueObjects.TaskStatus.DONE;
        OrderBy = task => task.DueDate;
    }
}

/// <summary>
/// Specification for blocked tasks
/// </summary>
public class BlockedTasksSpecification : Specification<TaskEntity>
{
    public BlockedTasksSpecification()
    {
        Criteria = task => task.Status == ValueObjects.TaskStatus.BLOCKED;
        AddInclude(task => task.Assignee!);
        AddInclude(task => task.Project);
    }
}
