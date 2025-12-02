using Project.CORE.Entities;

namespace Project.CORE.Interfaces;

public interface IProjectRepository : IRepository<ProjectEntity>
{
    Task<IEnumerable<ProjectEntity>> GetWorkspaceProjectsAsync(string workspaceId);
    Task<ProjectEntity?> GetWithTasksAsync(string id);
    Task<ProjectEntity?> GetWithMembersAsync(string id);
}
