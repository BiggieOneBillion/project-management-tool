using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.CORE.Entities;

namespace Project.INFRASTRUCTURE.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Id)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(u => u.Name)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(u => u.Email)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.HasIndex(u => u.Email)
            .IsUnique();
        
        builder.Property(u => u.ImageUrl)
            .HasMaxLength(500);
        
        builder.Property(u => u.CreatedAt)
            .HasDefaultValueSql("NOW()");
        
        builder.Property(u => u.UpdatedAt)
            .HasDefaultValueSql("NOW()");
        
        // Relationships
        builder.HasMany(u => u.OwnedWorkspaces)
            .WithOne(w => w.Owner)
            .HasForeignKey(w => w.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(u => u.WorkspaceMemberships)
            .WithOne(wm => wm.User)
            .HasForeignKey(wm => wm.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(u => u.LedProjects)
            .WithOne(p => p.TeamLead)
            .HasForeignKey(p => p.TeamLeadId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(u => u.ProjectMemberships)
            .WithOne(pm => pm.User)
            .HasForeignKey(pm => pm.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(u => u.AssignedTasks)
            .WithOne(t => t.Assignee)
            .HasForeignKey(t => t.AssigneeId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasMany(u => u.Comments)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
