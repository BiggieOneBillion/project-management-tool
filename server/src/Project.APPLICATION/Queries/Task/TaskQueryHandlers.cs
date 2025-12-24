using AutoMapper;
using MediatR;
using Project.APPLICATION.DTOs.Task;
using Project.APPLICATION.Results;
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


public class GetTasksForUserInWorkspaceQueryHandler : IRequestHandler<GetTasksForUserInWorkspaceQuery, Result<List<TaskDto>>>
{
    private readonly ITaskRepository _repository;

    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;
    
    public GetTasksForUserInWorkspaceQueryHandler(ITaskRepository repository, IMapper mapper, IProjectRepository projectRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _projectRepository = projectRepository;
    }

    public async Task<Result<List<TaskDto>>> Handle(GetTasksForUserInWorkspaceQuery request, CancellationToken cancellationToken)
    {
         var tasks = await _repository.GetUserTasksAsync(request.UserId);

         if(tasks == null)
         {
            return Result<List<TaskDto>>.FailureResult("No tasks found for the user.");
         }

        //  group the tasks by project id uisng dictionary
         var dict = tasks
            .GroupBy(task => task.ProjectId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var result = await _projectRepository.GetWorkspaceProjectsAsync(request.WorkspaceId);
        
        // chwck which keys(projectid) in the dictionary belong to the workspace id
       var groupedDict = dict
            .Where(pair => result.Any(p => p.Id == pair.Key))
            .ToDictionary(x => x.Key, x => x.Value);

        var valueList = groupedDict.Values.ToList();

        return Result<List<TaskDto>>.SuccessResult(data:_mapper.Map<List<TaskDto>>(valueList), message: "Tasks retrieved successfully.");

        
        // then check if the project id belongs to the workspace id
        // foreach (var group in grouped)
        // {
        //     work
        // }
        // // if it does then return the tasks
        // // else return empty list
        //  var filteredTasks = tasks.Where(t => t.WorkspaceId == request.WorkspaceId).ToList();

        // return _mapper.Map<List<TaskDto>>(tasks);
    }

}