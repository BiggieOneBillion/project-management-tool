namespace Project.CORE.Entities;

public class Note
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string TaskId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty; // Markdown content
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation Properties
    public TaskEntity Task { get; set; } = null!;
    public User User { get; set; } = null!;
    public ICollection<NoteMention> Mentions { get; set; } = new List<NoteMention>();
    public ICollection<NoteAttachment> Attachments { get; set; } = new List<NoteAttachment>();
}

public class NoteMention
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string NoteId { get; set; } = string.Empty;
    public string MentionedUserId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    
    // Navigation Properties
    public Note Note { get; set; } = null!;
    public User MentionedUser { get; set; } = null!;
}

public class NoteAttachment
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string NoteId { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty; // image/png, image/jpeg, etc.
    public long FileSize { get; set; }
    public DateTime UploadedAt { get; set; }
    
    // Navigation Properties
    public Note Note { get; set; } = null!;
}
