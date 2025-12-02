using AutoMapper;
using MediatR;
using Project.APPLICATION.DTOs.Project;
using Project.CORE.Interfaces;

namespace Project.APPLICATION.Queries.Project;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto?>
{
    private readonly IProjectRepository _repository;
    private readonly IMapper _mapper;
    
    public GetProjectByIdQueryHandler(IProjectRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<ProjectDto?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        CORE.Entities.ProjectEntity? project;
        
        if (request.IncludeTasks)
        {
            project = await _repository.GetWithTasksAsync(request.Id);
        }
        else if (request.IncludeMembers)
        {
            project = await _repository.GetWithMembersAsync(request.Id);
        }
        else
        {
            project = await _repository.GetByIdAsync(request.Id);
        }
        
        return project == null ? null :
            (request.IncludeTasks || request.IncludeMembers)
                ? _mapper.Map<ProjectDto>(project)
                : _mapper.Map<ProjectDto>(project);
    }
}

public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, List<ProjectDto>>
{
    private readonly IProjectRepository _repository;
    private readonly IMapper _mapper;
    
    public GetAllProjectsQueryHandler(IProjectRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<List<ProjectDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await _repository.GetAllAsync();
        return _mapper.Map<List<ProjectDto>>(projects);
    }
}

public class GetWorkspaceProjectsQueryHandler : IRequestHandler<GetWorkspaceProjectsQuery, List<ProjectDto>>
{
    private readonly IProjectRepository _repository;
    private readonly IMapper _mapper;
    
    public GetWorkspaceProjectsQueryHandler(IProjectRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<List<ProjectDto>> Handle(GetWorkspaceProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await _repository.GetWorkspaceProjectsAsync(request.WorkspaceId);
        return _mapper.Map<List<ProjectDto>>(projects);
    }
}
