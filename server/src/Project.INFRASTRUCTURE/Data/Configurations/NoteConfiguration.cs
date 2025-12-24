using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.CORE.Entities;

namespace Project.INFRASTRUCTURE.Data.Configurations;

public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.ToTable("Notes");
        
        builder.HasKey(n => n.Id);
        
        builder.Property(n => n.Id)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(n => n.Title)
            .HasMaxLength(500)
            .IsRequired();
        
        builder.Property(n => n.Content)
            .HasMaxLength(50000) // Support large markdown content
            .IsRequired();
        
        builder.Property(n => n.CreatedAt)
            .HasDefaultValueSql("NOW()");
        
        builder.Property(n => n.UpdatedAt)
            .HasDefaultValueSql("NOW()");
        
        // Relationships
        builder.HasOne(n => n.Task)
            .WithMany(t => t.Notes)
            .HasForeignKey(n => n.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(n => n.User)
            .WithMany(u => u.Notes)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(n => n.Mentions)
            .WithOne(m => m.Note)
            .HasForeignKey(m => m.NoteId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(n => n.Attachments)
            .WithOne(a => a.Note)
            .HasForeignKey(a => a.NoteId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Indexes
        builder.HasIndex(n => n.TaskId);
        builder.HasIndex(n => n.UserId);
        builder.HasIndex(n => n.CreatedAt);
    }
}

public class NoteMentionConfiguration : IEntityTypeConfiguration<NoteMention>
{
    public void Configure(EntityTypeBuilder<NoteMention> builder)
    {
        builder.ToTable("NoteMentions");
        
        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.Id)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(m => m.CreatedAt)
            .HasDefaultValueSql("NOW()");
        
        // Relationships
        builder.HasOne(m => m.MentionedUser)
            .WithMany()
            .HasForeignKey(m => m.MentionedUserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Indexes
        builder.HasIndex(m => m.NoteId);
        builder.HasIndex(m => m.MentionedUserId);
    }
}

public class NoteAttachmentConfiguration : IEntityTypeConfiguration<NoteAttachment>
{
    public void Configure(EntityTypeBuilder<NoteAttachment> builder)
    {
        builder.ToTable("NoteAttachments");
        
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.Id)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(a => a.FileName)
            .HasMaxLength(500)
            .IsRequired();
        
        builder.Property(a => a.FilePath)
            .HasMaxLength(1000)
            .IsRequired();
        
        builder.Property(a => a.FileType)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(a => a.UploadedAt)
            .HasDefaultValueSql("NOW()");
        
        // Indexes
        builder.HasIndex(a => a.NoteId);
    }
}

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");
        
        builder.HasKey(n => n.Id);
        
        builder.Property(n => n.Id)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(n => n.Type)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(n => n.Title)
            .HasMaxLength(500)
            .IsRequired();
        
        builder.Property(n => n.Message)
            .HasMaxLength(2000)
            .IsRequired();
        
        builder.Property(n => n.RelatedEntityId)
            .HasMaxLength(50);
        
        builder.Property(n => n.RelatedEntityType)
            .HasMaxLength(50);
        
        builder.Property(n => n.IsRead)
            .HasDefaultValue(false);
        
        builder.Property(n => n.CreatedAt)
            .HasDefaultValueSql("NOW()");
        
        // Relationships
        builder.HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Indexes
        builder.HasIndex(n => n.UserId);
        builder.HasIndex(n => n.IsRead);
        builder.HasIndex(n => n.CreatedAt);
        builder.HasIndex(n => new { n.UserId, n.IsRead }); // Composite index for unread notifications
    }
}
