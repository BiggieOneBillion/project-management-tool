using Project.CORE.Entities;

namespace Project.CORE.Interfaces;

public interface ITaskRepository : IRepository<TaskEntity>
{
    Task<IEnumerable<TaskEntity>> GetProjectTasksAsync(string projectId);
    Task<IEnumerable<TaskEntity>> GetUserTasksAsync(string userId);
    Task<TaskEntity?> GetWithCommentsAsync(string id);
    // Task<IEnumerable<TaskEntity>> GetTasksForUserInWorkspaceAsync(string UserId);
}
