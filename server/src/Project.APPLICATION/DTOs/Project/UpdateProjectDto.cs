namespace Project.APPLICATION.DTOs.Project;

public record UpdateProjectDto(
    string Name,
    string Description,
    string Priority,
    string Status,
    DateTime? StartDate,
    DateTime? EndDate,
    int? Progress
);
