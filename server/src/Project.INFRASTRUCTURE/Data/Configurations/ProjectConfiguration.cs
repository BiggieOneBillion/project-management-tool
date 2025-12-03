using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.CORE.Entities;

namespace Project.INFRASTRUCTURE.Data.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<ProjectEntity>
{
    public void Configure(EntityTypeBuilder<ProjectEntity> builder)
    {
        builder.ToTable("Projects");
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(p => p.Name)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.Property(p => p.Description)
            .HasMaxLength(2000);
        
        builder.Property(p => p.Priority)
            .HasConversion<int>()
            .IsRequired();
        
        builder.Property(p => p.Status)
            .HasConversion<int>()
            .IsRequired();
        
        builder.Property(p => p.Progress)
            .HasDefaultValue(0);
        
        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("NOW()");
        
        builder.Property(p => p.UpdatedAt)
            .HasDefaultValueSql("NOW()");
        
        // Relationships
        builder.HasOne(p => p.Workspace)
            .WithMany(w => w.Projects)
            .HasForeignKey(p => p.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(p => p.TeamLead)
            .WithMany(u => u.LedProjects)
            .HasForeignKey(p => p.TeamLeadId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
        
        builder.HasMany(p => p.Members)
            .WithOne(pm => pm.Project)
            .HasForeignKey(pm => pm.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(p => p.Tasks)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Indexes
        builder.HasIndex(p => p.WorkspaceId);
        builder.HasIndex(p => p.TeamLeadId);
        builder.HasIndex(p => p.Status);
    }
}

public class ProjectMemberConfiguration : IEntityTypeConfiguration<ProjectMember>
{
    public void Configure(EntityTypeBuilder<ProjectMember> builder)
    {
        builder.ToTable("ProjectMembers");
        
        builder.HasKey(pm => pm.Id);
        
        builder.Property(pm => pm.Id)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(pm => pm.AddedAt)
            .HasDefaultValueSql("NOW()");
        
        builder.HasIndex(pm => new { pm.UserId, pm.ProjectId })
            .IsUnique();
    }
}
