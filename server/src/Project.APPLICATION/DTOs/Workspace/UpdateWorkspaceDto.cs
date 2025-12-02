namespace Project.APPLICATION.DTOs.Workspace;

public record UpdateWorkspaceDto(
    string Name,
    string? Description,
    string? Settings
);
