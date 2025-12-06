using Project.CORE.Entities;

namespace Project.CORE.Interfaces;

public interface IProjectRepository : IRepository<Entities.ProjectEntity>
{
    Task<Entities.ProjectEntity?> GetWithTasksAsync(string id);
    Task<Entities.ProjectEntity?> GetWithMembersAsync(string id);
    Task<IEnumerable<Entities.ProjectEntity>> GetWorkspaceProjectsAsync(string workspaceId);
    Task<IEnumerable<Entities.ProjectEntity>> GetWorkspaceProjectsWithTasksAsync(string workspaceId);
    Task<IEnumerable<Entities.ProjectEntity>> GetUserProjectsAsync(string userId);
}
