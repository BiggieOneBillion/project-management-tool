using AutoMapper;
using MediatR;
using Project.APPLICATION.DTOs.Task;
using Project.CORE.Entities;
using Project.CORE.Exceptions;
using Project.CORE.Interfaces;
using Project.CORE.ValueObjects;

namespace Project.APPLICATION.Commands.Task;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
{
    private readonly ITaskRepository _repository;
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;
    
    public CreateTaskCommandHandler(
        ITaskRepository repository,
        IProjectRepository projectRepository,
        IMapper mapper)
    {
        _repository = repository;
        _projectRepository = projectRepository;
        _mapper = mapper;
    }
    
    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        // Check if user is workspace owner
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project == null)
            throw new EntityNotFoundException("Project", request.ProjectId);
        
        if (project.Workspace?.OwnerId != request.UserId)
            throw new UnauthorizedAccessException("Only workspace owners can create tasks");
        
        var task = new TaskEntity
        {
            Id = Guid.NewGuid().ToString(),
            ProjectId = request.ProjectId,
            Title = request.Title,
            Description = request.Description,
            Status = Enum.Parse<CORE.ValueObjects.TaskStatus>(request.Status),
            Type = Enum.Parse<TaskType>(request.Type),
            Priority = Enum.Parse<Priority>(request.Priority),
            AssigneeId = request.AssigneeId,
            DueDate = request.DueDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        var created = await _repository.AddAsync(task);
        return _mapper.Map<TaskDto>(created);
    }
}

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskDto>
{
    private readonly ITaskRepository _repository;
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;
    
    public UpdateTaskCommandHandler(
        ITaskRepository repository,
        IProjectRepository projectRepository,
        IMapper mapper)
    {
        _repository = repository;
        _projectRepository = projectRepository;
        _mapper = mapper;
    }
    
    public async Task<TaskDto> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _repository.GetByIdAsync(request.Id);
        
        if (task == null)
            throw new EntityNotFoundException("Task", request.Id);
        
        // Check if user is workspace owner
        var project = await _projectRepository.GetByIdAsync(task.ProjectId);
        if (project == null)
            throw new EntityNotFoundException("Project", task.ProjectId);
        
        if (project.Workspace?.OwnerId != request.UserId)
            throw new UnauthorizedAccessException("Only workspace owners can update tasks");
        
        task.Title = request.Title;
        task.Description = request.Description;
        task.Status = Enum.Parse<CORE.ValueObjects.TaskStatus>(request.Status);
        task.Type = Enum.Parse<TaskType>(request.Type);
        task.Priority = Enum.Parse<Priority>(request.Priority);
        task.AssigneeId = request.AssigneeId;
        
        if (request.DueDate.HasValue)
            task.DueDate = request.DueDate.Value;
        
        task.UpdatedAt = DateTime.UtcNow;
        
        await _repository.UpdateAsync(task);
        return _mapper.Map<TaskDto>(task);
    }
}

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Unit>
{
    private readonly ITaskRepository _repository;
    private readonly IProjectRepository _projectRepository;
    
    public DeleteTaskCommandHandler(
        ITaskRepository repository,
        IProjectRepository projectRepository)
    {
        _repository = repository;
        _projectRepository = projectRepository;
    }
    
    public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        // Get task to find its project
        var task = await _repository.GetByIdAsync(request.Id);
        if (task == null)
            throw new EntityNotFoundException("Task", request.Id);
        
        // Check if user is workspace owner
        var project = await _projectRepository.GetByIdAsync(task.ProjectId);
        if (project == null)
            throw new EntityNotFoundException("Project", task.ProjectId);
        
        if (project.Workspace?.OwnerId != request.UserId)
            throw new UnauthorizedAccessException("Only workspace owners can delete tasks");
        
        await _repository.DeleteAsync(request.Id);
        return Unit.Value;
    }
}

public class BulkDeleteTasksCommandHandler : IRequestHandler<BulkDeleteTasksCommand, int>
{
    private readonly ITaskRepository _repository;
    
    public BulkDeleteTasksCommandHandler(ITaskRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<int> Handle(BulkDeleteTasksCommand request, CancellationToken cancellationToken)
    {
        foreach (var taskId in request.TaskIds)
        {
            await _repository.DeleteAsync(taskId);
        }
        
        return request.TaskIds.Count;
    }
}
