using AutoMapper;
using MediatR;
using Project.APPLICATION.DTOs.Workspace;
using Project.CORE.Exceptions;
using Project.CORE.Interfaces;

namespace Project.APPLICATION.Commands.Workspace;

public class UpdateWorkspaceCommandHandler : IRequestHandler<UpdateWorkspaceCommand, WorkspaceDto>
{
    private readonly IWorkspaceRepository _repository;
    private readonly IMapper _mapper;
    
    public UpdateWorkspaceCommandHandler(IWorkspaceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<WorkspaceDto> Handle(UpdateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        var workspace = await _repository.GetByIdAsync(request.Id);
        
        if (workspace == null)
            throw new EntityNotFoundException("Workspace", request.Id);
        
        workspace.Name = request.Name;
        workspace.Description = request.Description;
        workspace.Settings = request.Settings ?? workspace.Settings;
        workspace.UpdatedAt = DateTime.UtcNow;
        
        await _repository.UpdateAsync(workspace);
        return _mapper.Map<WorkspaceDto>(workspace);
    }
}
