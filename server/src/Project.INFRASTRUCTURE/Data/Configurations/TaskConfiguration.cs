using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.CORE.Entities;

namespace Project.INFRASTRUCTURE.Data.Configurations;

public class TaskConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.ToTable("Tasks");
        
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Id)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(t => t.Title)
            .HasMaxLength(300)
            .IsRequired();
        
        builder.Property(t => t.Description)
            .HasMaxLength(5000);
        
        builder.Property(t => t.Status)
            .HasConversion<int>()
            .IsRequired();
        
        builder.Property(t => t.Type)
            .HasConversion<int>()
            .IsRequired();
        
        builder.Property(t => t.Priority)
            .HasConversion<int>()
            .IsRequired();
        
        builder.Property(t => t.CreatedAt)
            .HasDefaultValueSql("NOW()");
        
        builder.Property(t => t.UpdatedAt)
            .HasDefaultValueSql("NOW()");
        
        // Relationships
        builder.HasOne(t => t.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(t => t.Assignee)
            .WithMany(u => u.AssignedTasks)
            .HasForeignKey(t => t.AssigneeId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasMany(t => t.Comments)
            .WithOne(c => c.Task)
            .HasForeignKey(c => c.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Indexes
        builder.HasIndex(t => t.ProjectId);
        builder.HasIndex(t => t.AssigneeId);
        builder.HasIndex(t => t.Status);
        builder.HasIndex(t => t.DueDate);
    }
}

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");
        
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Id)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(c => c.Content)
            .HasMaxLength(5000)
            .IsRequired();
        
        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("NOW()");
        
        builder.Property(c => c.UpdatedAt)
            .HasDefaultValueSql("NOW()");
        
        // Indexes
        builder.HasIndex(c => c.TaskId);
        builder.HasIndex(c => c.UserId);
        builder.HasIndex(c => c.CreatedAt);
    }
}
