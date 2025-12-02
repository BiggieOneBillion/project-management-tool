using AutoMapper;
using MediatR;
using Project.APPLICATION.DTOs.Comment;
using Project.CORE.Interfaces;

namespace Project.APPLICATION.Queries.Comment;

public class GetTaskCommentsQueryHandler : IRequestHandler<GetTaskCommentsQuery, List<CommentDto>>
{
    private readonly IRepository<CORE.Entities.Comment> _repository;
    private readonly IMapper _mapper;
    
    public GetTaskCommentsQueryHandler(IRepository<CORE.Entities.Comment> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<List<CommentDto>> Handle(GetTaskCommentsQuery request, CancellationToken cancellationToken)
    {
        var allComments = await _repository.GetAllAsync();
        var taskComments = allComments.Where(c => c.TaskId == request.TaskId).ToList();
        return _mapper.Map<List<CommentDto>>(taskComments);
    }
}
