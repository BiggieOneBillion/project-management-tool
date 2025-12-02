using MediatR;

namespace Project.APPLICATION.Commands.Workspace;

public record DeleteWorkspaceCommand(string Id) : IRequest<Unit>;
