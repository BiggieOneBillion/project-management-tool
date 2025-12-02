using AutoMapper;
using MediatR;
using Project.APPLICATION.DTOs.Task;
using Project.CORE.Interfaces;

namespace Project.APPLICATION.Queries.Task;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto?>
{
    private readonly ITaskRepository _repository;
    private readonly IMapper _mapper;
    
    public GetTaskByIdQueryHandler(ITaskRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<TaskDto?> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        CORE.Entities.TaskEntity? task;
        
        if (request.IncludeComments)
        {
            task = await _repository.GetWithCommentsAsync(request.Id);
        }
        else
        {
            task = await _repository.GetByIdAsync(request.Id);
        }
        
        return task == null ? null :
            request.IncludeComments
                ? _mapper.Map<TaskDto>(task)
                : _mapper.Map<TaskDto>(task);
    }
}

public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, List<TaskDto>>
{
    private readonly ITaskRepository _repository;
    private readonly IMapper _mapper;
    
    public GetAllTasksQueryHandler(ITaskRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<List<TaskDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _repository.GetAllAsync();
        return _mapper.Map<List<TaskDto>>(tasks);
    }
}

public class GetProjectTasksQueryHandler : IRequestHandler<GetProjectTasksQuery, List<TaskDto>>
{
    private readonly ITaskRepository _repository;
    private readonly IMapper _mapper;
    
    public GetProjectTasksQueryHandler(ITaskRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<List<TaskDto>> Handle(GetProjectTasksQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _repository.GetProjectTasksAsync(request.ProjectId);
        return _mapper.Map<List<TaskDto>>(tasks);
    }
}

public class GetUserTasksQueryHandler : IRequestHandler<GetUserTasksQuery, List<TaskDto>>
{
    private readonly ITaskRepository _repository;
    private readonly IMapper _mapper;
    
    public GetUserTasksQueryHandler(ITaskRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<List<TaskDto>> Handle(GetUserTasksQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _repository.GetUserTasksAsync(request.UserId);
        return _mapper.Map<List<TaskDto>>(tasks);
    }
}
