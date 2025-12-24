namespace Project.APPLICATION.DTOs.Note;

public record CreateNoteDto(
    string TaskId = "",
    string UserId = "",
    string Title = "",
    string Content = "",
    List<string>? MentionedUserIds = null
);

public record UpdateNoteDto(
    string Title = "",
    string Content = "",
    List<string>? MentionedUserIds = null
);
