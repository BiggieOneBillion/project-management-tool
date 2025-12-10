using AutoMapper;
using MediatR;
using Project.APPLICATION.Commands.Note;
using Project.APPLICATION.DTOs.Note;
using Project.CORE.Entities;
using Project.CORE.Interfaces;

namespace Project.APPLICATION.Commands.Note;

public class NoteCommandHandlers :
    IRequestHandler<CreateNoteCommand, NoteDto>,
    IRequestHandler<UpdateNoteCommand, NoteDto>,
    IRequestHandler<DeleteNoteCommand, Unit>,
    IRequestHandler<UploadNoteAttachmentCommand, NoteAttachmentDto>,
    IRequestHandler<DeleteNoteAttachmentCommand, Unit>
{
    private readonly IRepository<CORE.Entities.Note> _noteRepository;
    private readonly IRepository<NoteMention> _mentionRepository;
    private readonly IRepository<NoteAttachment> _attachmentRepository;
    private readonly IRepository<CORE.Entities.Notification> _notificationRepository;
    private readonly IRepository<CORE.Entities.User> _userRepository;
    private readonly IMapper _mapper;
    
    public NoteCommandHandlers(
        IRepository<CORE.Entities.Note> noteRepository,
        IRepository<NoteMention> mentionRepository,
        IRepository<NoteAttachment> attachmentRepository,
        IRepository<CORE.Entities.Notification> notificationRepository,
        IRepository<CORE.Entities.User> userRepository,
        IMapper mapper)
    {
        _noteRepository = noteRepository;
        _mentionRepository = mentionRepository;
        _attachmentRepository = attachmentRepository;
        _notificationRepository = notificationRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public async Task<NoteDto> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
    {
        var note = new CORE.Entities.Note
        {
            Id = Guid.NewGuid().ToString(),
            TaskId = request.TaskId,
            UserId = request.UserId,
            Title = request.Title,
            Content = request.Content,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        var createdNote = await _noteRepository.AddAsync(note);
        
        // Create mentions if any
        if (request.MentionedUserIds != null && request.MentionedUserIds.Any())
        {
            foreach (var mentionedUserId in request.MentionedUserIds.Distinct())
            {
                var mention = new NoteMention
                {
                    Id = Guid.NewGuid().ToString(),
                    NoteId = createdNote.Id,
                    MentionedUserId = mentionedUserId,
                    CreatedAt = DateTime.UtcNow
                };
                await _mentionRepository.AddAsync(mention);
                
                // Create notification for mentioned user
                var user = await _userRepository.GetByIdAsync(request.UserId);
                var notification = new CORE.Entities.Notification
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = mentionedUserId,
                    Type = "MENTION",
                    Title = "You were mentioned in a note",
                    Message = $"{user?.Name ?? "Someone"} mentioned you in a note: {request.Title}",
                    RelatedEntityId = createdNote.Id,
                    RelatedEntityType = "Note",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };
                await _notificationRepository.AddAsync(notification);
            }
        }
        
        return _mapper.Map<NoteDto>(createdNote);
    }
    
    public async Task<NoteDto> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
    {
        var note = await _noteRepository.GetByIdAsync(request.Id);
        if (note == null)
            throw new KeyNotFoundException($"Note with ID {request.Id} not found");
        
        note.Title = request.Title;
        note.Content = request.Content;
        note.UpdatedAt = DateTime.UtcNow;
        
        await _noteRepository.UpdateAsync(note);
        
        // Update mentions
        if (request.MentionedUserIds != null)
        {
            // Get existing mentions
            var allMentions = await _mentionRepository.GetAllAsync();
            var existingMentions = allMentions.Where(m => m.NoteId == note.Id).ToList();
            var existingUserIds = existingMentions.Select(m => m.MentionedUserId).ToHashSet();
            var newUserIds = request.MentionedUserIds.ToHashSet();
            
            // Remove mentions that are no longer present
            var toRemove = existingMentions.Where(m => !newUserIds.Contains(m.MentionedUserId));
            foreach (var mention in toRemove)
            {
                await _mentionRepository.DeleteAsync(mention.Id);
            }
            
            // Add new mentions
            var toAdd = newUserIds.Where(id => !existingUserIds.Contains(id));
            var user = await _userRepository.GetByIdAsync(note.UserId);
            
            foreach (var mentionedUserId in toAdd)
            {
                var mention = new NoteMention
                {
                    Id = Guid.NewGuid().ToString(),
                    NoteId = note.Id,
                    MentionedUserId = mentionedUserId,
                    CreatedAt = DateTime.UtcNow
                };
                await _mentionRepository.AddAsync(mention);
                
                // Create notification for newly mentioned user
                var notification = new CORE.Entities.Notification
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = mentionedUserId,
                    Type = "MENTION",
                    Title = "You were mentioned in a note",
                    Message = $"{user?.Name ?? "Someone"} mentioned you in a note: {request.Title}",
                    RelatedEntityId = note.Id,
                    RelatedEntityType = "Note",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };
                await _notificationRepository.AddAsync(notification);
            }
        }
        
        return _mapper.Map<NoteDto>(note);
    }
    
    public async Task<Unit> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
    {
        await _noteRepository.DeleteAsync(request.Id);
        return Unit.Value;
    }
    
    public async Task<NoteAttachmentDto> Handle(UploadNoteAttachmentCommand request, CancellationToken cancellationToken)
    {
        // Create uploads directory if it doesn't exist
        var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "notes", request.NoteId);
        Directory.CreateDirectory(uploadsPath);
        
        // Generate unique filename
        var timestamp = DateTime.UtcNow.Ticks;
        var extension = Path.GetExtension(request.FileName);
        var uniqueFileName = $"{timestamp}_{Path.GetFileNameWithoutExtension(request.FileName)}{extension}";
        var filePath = Path.Combine(uploadsPath, uniqueFileName);
        
        // Save file
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await request.FileStream.CopyToAsync(fileStream, cancellationToken);
        }
        
        // Create attachment record
        var attachment = new NoteAttachment
        {
            Id = Guid.NewGuid().ToString(),
            NoteId = request.NoteId,
            FileName = request.FileName,
            FilePath = $"/uploads/notes/{request.NoteId}/{uniqueFileName}",
            FileType = request.FileType,
            FileSize = request.FileSize,
            UploadedAt = DateTime.UtcNow
        };
        
        var createdAttachment = await _attachmentRepository.AddAsync(attachment);
        return _mapper.Map<NoteAttachmentDto>(createdAttachment);
    }
    
    public async Task<Unit> Handle(DeleteNoteAttachmentCommand request, CancellationToken cancellationToken)
    {
        var attachment = await _attachmentRepository.GetByIdAsync(request.AttachmentId);
        if (attachment == null)
            throw new KeyNotFoundException($"Attachment with ID {request.AttachmentId} not found");
        
        // Delete physical file
        var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", attachment.FilePath.TrimStart('/'));
        if (File.Exists(physicalPath))
        {
            File.Delete(physicalPath);
        }
        
        // Delete database record
        await _attachmentRepository.DeleteAsync(request.AttachmentId);
        return Unit.Value;
    }
}
