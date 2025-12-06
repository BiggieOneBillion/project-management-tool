using Project.CORE.Entities;

namespace Project.CORE.Interfaces;

public interface IInvitationRepository : IRepository<Invitation>
{
    Task<Invitation?> GetByTokenAsync(string token);
    Task<Invitation?> GetPendingInvitationAsync(string email, string? workspaceId, string? projectId);
    Task<IEnumerable<Invitation>> GetUserInvitationsAsync(string email);
    Task<IEnumerable<Invitation>> GetWorkspaceInvitationsAsync(string workspaceId);
    Task<IEnumerable<Invitation>> GetProjectInvitationsAsync(string projectId);
}
