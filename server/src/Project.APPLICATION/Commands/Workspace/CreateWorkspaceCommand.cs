using MediatR;
using Project.APPLICATION.DTOs.Workspace;

namespace Project.APPLICATION.Commands.Workspace;

public record CreateWorkspaceCommand(
    string Name,
    string Slug,
    string? Description,
    string OwnerId
) : IRequest<WorkspaceDto>;
