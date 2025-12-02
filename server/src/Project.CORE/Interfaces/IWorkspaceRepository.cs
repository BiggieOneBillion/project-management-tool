using Project.CORE.Entities;

namespace Project.CORE.Interfaces;

public interface IWorkspaceRepository : IRepository<Workspace>
{
    Task<IEnumerable<Workspace>> GetUserWorkspacesAsync(string userId);
    Task<Workspace?> GetBySlugAsync(string slug);
    Task<Workspace?> GetWithMembersAsync(string id);
    Task<Workspace?> GetWithProjectsAsync(string id);
}
