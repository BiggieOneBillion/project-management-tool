using MediatR;
using Project.CORE.Interfaces;

namespace Project.APPLICATION.Commands.Workspace;

public class DeleteWorkspaceCommandHandler : IRequestHandler<DeleteWorkspaceCommand, Unit>
{
    private readonly IWorkspaceRepository _repository;
    
    public DeleteWorkspaceCommandHandler(IWorkspaceRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Unit> Handle(DeleteWorkspaceCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id);
        return Unit.Value;
    }
}
