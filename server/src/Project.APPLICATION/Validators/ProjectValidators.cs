using FluentValidation;
using Project.APPLICATION.Commands.Project;

namespace Project.APPLICATION.Validators;

public class CreateProjectValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required")
            .MaximumLength(200).WithMessage("Project name cannot exceed 200 characters");
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Project description is required")
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters");
        
        RuleFor(x => x.Priority)
            .NotEmpty().WithMessage("Priority is required")
            .Must(p => new[] { "LOW", "MEDIUM", "HIGH" }.Contains(p))
            .WithMessage("Priority must be LOW, MEDIUM, or HIGH");
        
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required")
            .Must(s => new[] { "PLANNING", "ACTIVE", "ON_HOLD", "COMPLETED" }.Contains(s))
            .WithMessage("Status must be PLANNING, ACTIVE, ON_HOLD, or COMPLETED");
        
        RuleFor(x => x.TeamLeadId)
            .NotEmpty().WithMessage("Team lead is required");
        
        RuleFor(x => x.WorkspaceId)
            .NotEmpty().WithMessage("Workspace ID is required");
        
        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after start date");
    }
}

public class UpdateProjectValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Project ID is required");
        
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required")
            .MaximumLength(200).WithMessage("Project name cannot exceed 200 characters");
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Project description is required")
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters");
        
        RuleFor(x => x.Priority)
            .NotEmpty().WithMessage("Priority is required")
            .Must(p => new[] { "LOW", "MEDIUM", "HIGH" }.Contains(p))
            .WithMessage("Priority must be LOW, MEDIUM, or HIGH");
        
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required")
            .Must(s => new[] { "PLANNING", "ACTIVE", "ON_HOLD", "COMPLETED" }.Contains(s))
            .WithMessage("Status must be PLANNING, ACTIVE, ON_HOLD, or COMPLETED");
        
        RuleFor(x => x.Progress)
            .InclusiveBetween(0, 100)
            .WithMessage("Progress must be between 0 and 100")
            .When(x => x.Progress.HasValue);
        
        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after start date")
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue);
    }
}
