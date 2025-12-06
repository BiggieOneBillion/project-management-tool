using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.CORE.Entities;

namespace Project.INFRASTRUCTURE.Data.Configurations;

public class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.ToTable("Invitations");
        
        builder.HasKey(i => i.Id);
        
        builder.Property(i => i.Email)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(i => i.Token)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(i => i.Type)
            .IsRequired()
            .HasConversion<string>();
        
        builder.Property(i => i.Status)
            .IsRequired()
            .HasConversion<string>();
        
        builder.Property(i => i.InvitedById)
            .IsRequired();
        
        builder.Property(i => i.ExpiresAt)
            .IsRequired();
        
        builder.Property(i => i.CreatedAt)
            .IsRequired();
        
        // Relationships
        builder.HasOne(i => i.Workspace)
            .WithMany()
            .HasForeignKey(i => i.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        builder.HasOne(i => i.Project)
            .WithMany()
            .HasForeignKey(i => i.ProjectId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        builder.HasOne(i => i.InvitedBy)
            .WithMany()
            .HasForeignKey(i => i.InvitedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Indexes
        builder.HasIndex(i => i.Token).IsUnique();
        builder.HasIndex(i => i.Email);
        builder.HasIndex(i => i.Status);
    }
}
