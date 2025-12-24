using AutoMapper;
using MediatR;
using Project.APPLICATION.DTOs.Note;
using Project.APPLICATION.Queries.Note;
using Project.CORE.Interfaces;

namespace Project.APPLICATION.Queries.Note;

public class NoteQueryHandlers :
    IRequestHandler<GetTaskNotesQuery, List<NoteDto>>,
    IRequestHandler<GetNoteByIdQuery, NoteDto>
{
    private readonly IRepository<CORE.Entities.Note> _noteRepository;
    private readonly IMapper _mapper;
    
    public NoteQueryHandlers(IRepository<CORE.Entities.Note> noteRepository, IMapper mapper)
    {
        _noteRepository = noteRepository;
        _mapper = mapper;
    }
    
    public async Task<List<NoteDto>> Handle(GetTaskNotesQuery request, CancellationToken cancellationToken)
    {
        var allNotes = await _noteRepository.GetAllAsync();
        var taskNotes = allNotes
            .Where(n => n.TaskId == request.TaskId)
            .OrderByDescending(n => n.CreatedAt)
            .ToList();
        
        return _mapper.Map<List<NoteDto>>(taskNotes);
    }
    
    public async Task<NoteDto> Handle(GetNoteByIdQuery request, CancellationToken cancellationToken)
    {
        var note = await _noteRepository.GetByIdAsync(request.NoteId);
        if (note == null)
            throw new KeyNotFoundException($"Note with ID {request.NoteId} not found");
        
        return _mapper.Map<NoteDto>(note);
    }
}
