namespace Project.APPLICATION.DTOs.Workspace;

public record CreateWorkspaceDto(
    string Name,
    string Slug,
    string? Description,
    string OwnerId
);
