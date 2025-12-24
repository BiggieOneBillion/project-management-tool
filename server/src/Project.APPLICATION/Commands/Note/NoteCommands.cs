using MediatR;
using Project.APPLICATION.DTOs.Note;

namespace Project.APPLICATION.Commands.Note;

public record CreateNoteCommand(
    string TaskId = "",
    string UserId = "",
    string Title = "",
    string Content = "",
    List<string>? MentionedUserIds = null
) : IRequest<NoteDto>;

public record UpdateNoteCommand(
    string Id = "",
    string Title = "",
    string Content = "",
    List<string>? MentionedUserIds = null
) : IRequest<NoteDto>;

public record DeleteNoteCommand(string Id) : IRequest<Unit>;

public record UploadNoteAttachmentCommand(
    string NoteId,
    string FileName,
    string FileType,
    long FileSize,
    Stream FileStream
) : IRequest<NoteAttachmentDto>;

public record DeleteNoteAttachmentCommand(
    string NoteId,
    string AttachmentId
) : IRequest<Unit>;
