namespace Project.APPLICATION.DTOs.Project;

public record CreateProjectDto(
    string Name,
    string Description,
    string Priority,
    string Status,
    DateTime StartDate,
    DateTime EndDate,
    string TeamLeadId,
    string WorkspaceId
);
