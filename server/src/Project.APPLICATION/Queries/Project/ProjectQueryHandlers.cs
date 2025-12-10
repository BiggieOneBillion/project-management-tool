using AutoMapper;
using MediatR;
using Project.APPLICATION.DTOs.Project;
using Project.APPLICATION.DTOs.Workspace;
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
        IEnumerable<CORE.Entities.ProjectEntity> projects;
        
        if (request.IncludeTasks)
        {
            projects = await _repository.GetWorkspaceProjectsWithTasksAsync(request.WorkspaceId);
        }
        else
        {
            projects = await _repository.GetWorkspaceProjectsAsync(request.WorkspaceId);
        }
        
        return _mapper.Map<List<ProjectDto>>(projects);
    }
}

public class GetProjectMembersQueryHandler : IRequestHandler<GetProjectMembersQuery, List<ProjectMemberDto>>
{
    private readonly IProjectRepository _repository;
    private readonly IMapper _mapper;
    
    public GetProjectMembersQueryHandler(IProjectRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<List<ProjectMemberDto>> Handle(GetProjectMembersQuery request, CancellationToken cancellationToken)
    {
        var members = await _repository.GetProjectMembersAsync(request.ProjectId);
        return _mapper.Map<List<ProjectMemberDto>>(members);
    }
}

public class GetAvailableWorkspaceMembersQueryHandler : IRequestHandler<GetAvailableWorkspaceMembersQuery, List<WorkspaceMemberDto>>
{
    private readonly IProjectRepository _repository;
    private readonly IMapper _mapper;
    
    public GetAvailableWorkspaceMembersQueryHandler(IProjectRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<List<WorkspaceMemberDto>> Handle(GetAvailableWorkspaceMembersQuery request, CancellationToken cancellationToken)
    {
        var availableMembers = await _repository.GetAvailableWorkspaceMembersAsync(request.ProjectId, request.WorkspaceId);
        return _mapper.Map<List<WorkspaceMemberDto>>(availableMembers);
    }
}
