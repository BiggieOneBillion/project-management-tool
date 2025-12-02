using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.CORE.Entities;

namespace Project.INFRASTRUCTURE.Data.Configurations;

public class WorkspaceConfiguration : IEntityTypeConfiguration<Workspace>
{
    public void Configure(EntityTypeBuilder<Workspace> builder)
    {
        builder.ToTable("Workspaces");
        
        builder.HasKey(w => w.Id);
        
        builder.Property(w => w.Id)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(w => w.Name)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.Property(w => w.Slug)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.HasIndex(w => w.Slug)
            .IsUnique();
        
        builder.Property(w => w.Description)
            .HasMaxLength(1000);
        
        builder.Property(w => w.ImageUrl)
            .HasMaxLength(500);
        
        builder.Property(w => w.Settings)
            .HasColumnType("nvarchar(max)");
        
        builder.Property(w => w.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");
        
        builder.Property(w => w.UpdatedAt)
            .HasDefaultValueSql("GETUTCDATE()");
        
        // Relationships
        builder.HasOne(w => w.Owner)
            .WithMany(u => u.OwnedWorkspaces)
            .HasForeignKey(w => w.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(w => w.Members)
            .WithOne(wm => wm.Workspace)
            .HasForeignKey(wm => wm.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(w => w.Projects)
            .WithOne(p => p.Workspace)
            .HasForeignKey(p => p.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class WorkspaceMemberConfiguration : IEntityTypeConfiguration<WorkspaceMember>
{
    public void Configure(EntityTypeBuilder<WorkspaceMember> builder)
    {
        builder.ToTable("WorkspaceMembers");
        
        builder.HasKey(wm => wm.Id);
        
        builder.Property(wm => wm.Id)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(wm => wm.Role)
            .HasConversion<int>()
            .IsRequired();
        
        builder.Property(wm => wm.JoinedAt)
            .HasDefaultValueSql("GETUTCDATE()");
        
        builder.HasIndex(wm => new { wm.UserId, wm.WorkspaceId })
            .IsUnique();
    }
}
