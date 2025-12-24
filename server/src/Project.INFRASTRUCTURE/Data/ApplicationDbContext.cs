using Microsoft.EntityFrameworkCore;
using Project.CORE.Entities;

namespace Project.INFRASTRUCTURE.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Workspace> Workspaces { get; set; } = null!;
    public DbSet<WorkspaceMember> WorkspaceMembers { get; set; } = null!;
    public DbSet<ProjectEntity> Projects { get; set; } = null!;
    public DbSet<ProjectMember> ProjectMembers { get; set; } = null!;
    public DbSet<TaskEntity> Tasks { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Invitation> Invitations { get; set; } = null!;
    public DbSet<Note> Notes { get; set; } = null!;
    public DbSet<NoteMention> NoteMentions { get; set; } = null!;
    public DbSet<NoteAttachment> NoteAttachments { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Apply all configurations from assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Automatically update UpdatedAt timestamp
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified);
        
        foreach (var entry in entries)
        {
            if (entry.Entity is User user)
                user.UpdatedAt = DateTime.UtcNow;
            else if (entry.Entity is Workspace workspace)
                workspace.UpdatedAt = DateTime.UtcNow;
            else if (entry.Entity is ProjectEntity project)
                project.UpdatedAt = DateTime.UtcNow;
            else if (entry.Entity is TaskEntity task)
                task.UpdatedAt = DateTime.UtcNow;
            else if (entry.Entity is Comment comment)
                comment.UpdatedAt = DateTime.UtcNow;
            else if (entry.Entity is Note note)
                note.UpdatedAt = DateTime.UtcNow;
        }
        
        return base.SaveChangesAsync(cancellationToken);
    }
}
