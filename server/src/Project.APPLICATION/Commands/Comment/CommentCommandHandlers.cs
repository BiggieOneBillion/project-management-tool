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
        var comment = new CORE.Entities.Comment
        {
            Id = Guid.NewGuid().ToString(),
            TaskId = request.TaskId,
            UserId = request.UserId,
            Content = request.Content,
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
        await _repository.DeleteAsync(request.Id);
        return Unit.Value;
    }
}
