using Microsoft.EntityFrameworkCore;
using Project.CORE.Entities;
using Project.CORE.Interfaces;
using Project.INFRASTRUCTURE.Data;

namespace Project.INFRASTRUCTURE.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;
    
    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
    
    public virtual async Task<T?> GetByIdAsync(string id)
    {
        return await _dbSet.FindAsync(id);
    }
    
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }
    
    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    
    public virtual async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }
    
    public virtual async Task DeleteAsync(string id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}

public class WorkspaceRepository : Repository<Workspace>, IWorkspaceRepository
{
    public WorkspaceRepository(ApplicationDbContext context) : base(context) { }
    
    public async Task<IEnumerable<Workspace>> GetUserWorkspacesAsync(string userId)
    {
        return await _context.Workspaces
            .Include(w => w.Owner)
            .Include(w => w.Members)
                .ThenInclude(m => m.User)
            .Where(w => w.OwnerId == userId || w.Members.Any(m => m.UserId == userId))
            .ToListAsync();
    }
    
    public async Task<Workspace?> GetBySlugAsync(string slug)
    {
        return await _context.Workspaces
            .Include(w => w.Owner)
            .FirstOrDefaultAsync(w => w.Slug == slug);
    }
    
    public async Task<Workspace?> GetWithMembersAsync(string id)
    {
        return await _context.Workspaces
            .Include(w => w.Owner)
            .Include(w => w.Members)
                .ThenInclude(m => m.User)
            .Include(w => w.Projects) // Always include for ProjectCount
            .FirstOrDefaultAsync(w => w.Id == id);
    }
    
    public async Task<Workspace?> GetWithProjectsAsync(string id)
    {
        return await _context.Workspaces
            .Include(w => w.Owner)
            .Include(w => w.Projects)
            .Include(w => w.Members) // Always include for MemberCount
            .FirstOrDefaultAsync(w => w.Id == id);
    }
    
    public async Task<Workspace?> GetWithCountsAsync(string id)
    {
        return await _context.Workspaces
            .Include(w => w.Members)
            .Include(w => w.Projects)
            .FirstOrDefaultAsync(w => w.Id == id);
    }
}

public class ProjectRepository : Repository<ProjectEntity>, IProjectRepository
{
    public ProjectRepository(ApplicationDbContext context) : base(context) { }
    
    public async Task<IEnumerable<ProjectEntity>> GetWorkspaceProjectsAsync(string workspaceId)
    {
        return await _context.Projects
            .Include(p => p.TeamLead)
            .Include(p => p.Members)
            .Where(p => p.WorkspaceId == workspaceId)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<ProjectEntity>> GetWorkspaceProjectsWithTasksAsync(string workspaceId)
    {
        return await _context.Projects
            .Include(p => p.TeamLead)
            .Include(p => p.Members)
            .Include(p => p.Tasks)
                .ThenInclude(t => t.Assignee)
            .Where(p => p.WorkspaceId == workspaceId)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<ProjectEntity>> GetUserProjectsAsync(string userId)
    {
        return await _context.Projects
            .Include(p => p.TeamLead)
            .Include(p => p.Members)
            .Where(p => p.TeamLeadId == userId || p.Members.Any(m => m.UserId == userId))
            .ToListAsync();
    }
    
    public async Task<ProjectEntity?> GetWithTasksAsync(string id)
    {
        return await _context.Projects
            .Include(p => p.TeamLead)
            .Include(p => p.Tasks)
                .ThenInclude(t => t.Assignee)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    
    public async Task<ProjectEntity?> GetWithMembersAsync(string id)
    {
        return await _context.Projects
            .Include(p => p.TeamLead)
            .Include(p => p.Members)
                .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    
    public async Task<IEnumerable<ProjectMember>> GetProjectMembersAsync(string projectId)
    {
        return await _context.ProjectMembers
            .Where(pm => pm.ProjectId == projectId)
            .Include(pm => pm.User)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<WorkspaceMember>> GetAvailableWorkspaceMembersAsync(string projectId, string workspaceId)
    {
        // Get all project member user IDs
        var projectMemberUserIds = await _context.ProjectMembers
            .Where(pm => pm.ProjectId == projectId)
            .Select(pm => pm.UserId)
            .ToListAsync();
        
        // Get workspace members who are NOT in the project
        return await _context.WorkspaceMembers
            .Where(wm => wm.WorkspaceId == workspaceId && !projectMemberUserIds.Contains(wm.UserId))
            .Include(wm => wm.User)
            .ToListAsync();
    }
}

public class TaskRepository : Repository<TaskEntity>, ITaskRepository
{
    public TaskRepository(ApplicationDbContext context) : base(context) { }
    
    public async Task<IEnumerable<TaskEntity>> GetProjectTasksAsync(string projectId)
    {
        return await _context.Tasks
            .Include(t => t.Assignee)
            .Where(t => t.ProjectId == projectId)
            .ToListAsync();
    }

    public Task<IEnumerable<TaskEntity>> GetTasksForUserInWorkspaceAsync(string WorkspaceId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<TaskEntity>> GetUserTasksAsync(string userId)
    {
        return await _context.Tasks
            .Include(t => t.Project)
            .Include(t => t.Assignee)
            .Where(t => t.AssigneeId == userId)
            .ToListAsync();
    }
    
    public async Task<TaskEntity?> GetWithCommentsAsync(string id)
    {
        return await _context.Tasks
            .Include(t => t.Assignee)
            .Include(t => t.Comments)
                .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
}

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context) { }
    
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
    }
}

public class InvitationRepository : Repository<Invitation>, IInvitationRepository
{
    public InvitationRepository(ApplicationDbContext context) : base(context) { }
    
    public async Task<Invitation?> GetByTokenAsync(string token)
    {
        return await _context.Invitations
            .Include(i => i.Workspace)
            .Include(i => i.Project)
            .Include(i => i.InvitedBy)
            .FirstOrDefaultAsync(i => i.Token == token);
    }
    
    public async Task<Invitation?> GetPendingInvitationAsync(string email, string? workspaceId, string? projectId)
    {
        return await _context.Invitations
            .FirstOrDefaultAsync(i => 
                i.Email == email &&
                i.Status == CORE.Enums.InvitationStatus.PENDING &&
                (workspaceId == null || i.WorkspaceId == workspaceId) &&
                (projectId == null || i.ProjectId == projectId));
    }
    
    public async Task<IEnumerable<Invitation>> GetUserInvitationsAsync(string email)
    {
        return await _context.Invitations
            .Include(i => i.Workspace)
            .Include(i => i.Project)
            .Include(i => i.InvitedBy)
            .Where(i => i.Email == email)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Invitation>> GetWorkspaceInvitationsAsync(string workspaceId)
    {
        return await _context.Invitations
            .Include(i => i.InvitedBy)
            .Where(i => i.WorkspaceId == workspaceId)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Invitation>> GetProjectInvitationsAsync(string projectId)
    {
        return await _context.Invitations
            .Include(i => i.InvitedBy)
            .Where(i => i.ProjectId == projectId)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }
}
