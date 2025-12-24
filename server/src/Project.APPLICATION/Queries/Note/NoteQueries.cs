using MediatR;
using Project.APPLICATION.DTOs.Note;

namespace Project.APPLICATION.Queries.Note;

public record GetTaskNotesQuery(string TaskId) : IRequest<List<NoteDto>>;

public record GetNoteByIdQuery(string NoteId) : IRequest<NoteDto>;
