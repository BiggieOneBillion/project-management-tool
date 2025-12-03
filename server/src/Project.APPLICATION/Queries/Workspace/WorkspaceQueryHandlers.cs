using AutoMapper;
using MediatR;
using Project.APPLICATION.DTOs.Workspace;
using Project.CORE.Interfaces;

namespace Project.APPLICATION.Queries.Workspace;

public class GetWorkspaceByIdQueryHandler : IRequestHandler<GetWorkspaceByIdQuery, WorkspaceDto?>
{
    private readonly IWorkspaceRepository _repository;
    private readonly IMapper _mapper;
    
    public GetWorkspaceByIdQueryHandler(IWorkspaceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<WorkspaceDto?> Handle(GetWorkspaceByIdQuery request, CancellationToken cancellationToken)
    {
        CORE.Entities.Workspace? workspace;
        
        if (request.IncludeMembers)
        {
            workspace = await _repository.GetWithMembersAsync(request.Id);
        }
        else if (request.IncludeProjects)
        {
            workspace = await _repository.GetWithProjectsAsync(request.Id);
        }
        else
        {
            // Always load Members and Projects for accurate counts
            workspace = await _repository.GetWithCountsAsync(request.Id);
        }
        
        return workspace == null ? null : _mapper.Map<WorkspaceDto>(workspace);
    }
}

public class GetAllWorkspacesQueryHandler : IRequestHandler<GetAllWorkspacesQuery, List<WorkspaceDetailDto>>
{
    private readonly IWorkspaceRepository _repository;
    private readonly IMapper _mapper;
    
    public GetAllWorkspacesQueryHandler(IWorkspaceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<List<WorkspaceDetailDto>> Handle(GetAllWorkspacesQuery request, CancellationToken cancellationToken)
    {
        var workspaces = await _repository.GetAllAsync();
        return _mapper.Map<List<WorkspaceDetailDto>>(workspaces);
    }
}

public class GetUserWorkspacesQueryHandler : IRequestHandler<GetUserWorkspacesQuery, List<WorkspaceDto>>
{
    private readonly IWorkspaceRepository _repository;
    private readonly IMapper _mapper;
    
    public GetUserWorkspacesQueryHandler(IWorkspaceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<List<WorkspaceDto>> Handle(GetUserWorkspacesQuery request, CancellationToken cancellationToken)
    {
        var workspaces = await _repository.GetUserWorkspacesAsync(request.UserId);
        return _mapper.Map<List<WorkspaceDto>>(workspaces);
    }
}
