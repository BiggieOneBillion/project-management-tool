using AutoMapper;
using MediatR;
using Project.APPLICATION.DTOs.Workspace;
using Project.CORE.Interfaces;

namespace Project.APPLICATION.Commands.Workspace;

public class CreateWorkspaceCommandHandler : IRequestHandler<CreateWorkspaceCommand, WorkspaceDto>
{
    private readonly IWorkspaceRepository _repository;
    private readonly IMapper _mapper;
    
    public CreateWorkspaceCommandHandler(IWorkspaceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<WorkspaceDto> Handle(CreateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        var workspace = new CORE.Entities.Workspace
        {
            Id = Guid.NewGuid().ToString(),
            Name = request.Name,
            Slug = request.Slug,
            Description = request.Description,
            OwnerId = request.OwnerId,
            Settings = "{}",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        var created = await _repository.AddAsync(workspace);
        return _mapper.Map<WorkspaceDto>(created);
    }
}
