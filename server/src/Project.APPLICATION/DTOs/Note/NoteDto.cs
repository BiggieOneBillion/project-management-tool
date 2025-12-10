using Project.APPLICATION.DTOs.User;

namespace Project.APPLICATION.DTOs.Note;

public class NoteDto
{
    public string Id { get; init; } = "";
    public string TaskId { get; init; } = "";
    public string UserId { get; init; } = "";
    public string Title { get; init; } = "";
    public string Content { get; init; } = "";
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public UserDto? User { get; init; }
    public List<NoteMentionDto> Mentions { get; init; } = new();
    public List<NoteAttachmentDto> Attachments { get; init; } = new();
    public bool CanEdit { get; init; }
    public bool CanDelete { get; init; }
}

public class NoteMentionDto
{
    public string Id { get; init; } = "";
    public string NoteId { get; init; } = "";
    public string MentionedUserId { get; init; } = "";
    public DateTime CreatedAt { get; init; }
    public UserDto? MentionedUser { get; init; }
}

public class NoteAttachmentDto
{
    public string Id { get; init; } = "";
    public string NoteId { get; init; } = "";
    public string FileName { get; init; } = "";
    public string FilePath { get; init; } = "";
    public string FileType { get; init; } = "";
    public long FileSize { get; init; }
    public DateTime UploadedAt { get; init; }
}
