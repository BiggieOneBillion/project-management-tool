using MediatR;
using Project.APPLICATION.DTOs.Workspace;

namespace Project.APPLICATION.Commands.Workspace;

public record UpdateWorkspaceCommand(
    string Id,
    string Name,
    string? Description,
    string? Settings
) : IRequest<WorkspaceDto>;
