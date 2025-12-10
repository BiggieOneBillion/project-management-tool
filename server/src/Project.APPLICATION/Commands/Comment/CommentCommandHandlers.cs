using AutoMapper;
using MediatR;
using Project.APPLICATION.DTOs.Comment;
using Project.CORE.Entities;
using Project.CORE.Exceptions;
using Project.CORE.Interfaces;

namespace Project.APPLICATION.Commands.Comment;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CommentDto>
{
    private readonly IRepository<CORE.Entities.Comment> _repository;
    private readonly IMapper _mapper;
    
    public CreateCommentCommandHandler(IRepository<CORE.Entities.Comment> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<CommentDto> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        int level = 0;
        
        // If this is a reply, validate parent and calculate level
        if (!string.IsNullOrEmpty(request.ParentId))
        {
            var parent = await _repository.GetByIdAsync(request.ParentId);
            if (parent == null)
                throw new EntityNotFoundException("Parent Comment", request.ParentId);
            
            level = parent.Level + 1;
            
            // Enforce max nesting level of 4
            if (level > 4)
                throw new BusinessRuleViolationException("Maximum comment nesting level (4) exceeded");
        }
        
        var comment = new CORE.Entities.Comment
        {
            Id = Guid.NewGuid().ToString(),
            TaskId = request.TaskId,
            UserId = request.UserId,
            Content = request.Content,
            ParentId = request.ParentId,
            Level = level,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        var created = await _repository.AddAsync(comment);
        return _mapper.Map<CommentDto>(created);
    }
}

public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, CommentDto>
{
    private readonly IRepository<CORE.Entities.Comment> _repository;
    private readonly IMapper _mapper;
    
    public UpdateCommentCommandHandler(IRepository<CORE.Entities.Comment> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<CommentDto> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _repository.GetByIdAsync(request.Id);
        
        if (comment == null)
            throw new EntityNotFoundException("Comment", request.Id);
        
        // Note: Ownership validation should be done in the controller/API layer
        // where we have access to the current user context
        
        comment.Content = request.Content;
        comment.UpdatedAt = DateTime.UtcNow;
        
        await _repository.UpdateAsync(comment);
        return _mapper.Map<CommentDto>(comment);
    }
}

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Unit>
{
    private readonly IRepository<CORE.Entities.Comment> _repository;
    
    public DeleteCommentCommandHandler(IRepository<CORE.Entities.Comment> repository)
    {
        _repository = repository;
    }
    
    public async Task<Unit> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _repository.GetByIdAsync(request.Id);
        
        if (comment == null)
            throw new EntityNotFoundException("Comment", request.Id);
        
        // Note: Ownership validation should be done in the controller/API layer
        // Cascade delete will automatically remove all replies due to DB configuration
        
        await _repository.DeleteAsync(request.Id);
        return Unit.Value;
    }
}
