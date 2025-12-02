using FluentValidation;
using Project.APPLICATION.Commands.Workspace;

namespace Project.APPLICATION.Validators;

public class CreateWorkspaceValidator : AbstractValidator<CreateWorkspaceCommand>
{
    public CreateWorkspaceValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Workspace name is required")
            .MaximumLength(200).WithMessage("Workspace name cannot exceed 200 characters");
        
        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Workspace slug is required")
            .Matches("^[a-z0-9-]+$").WithMessage("Slug must be lowercase alphanumeric with hyphens only")
            .MaximumLength(200).WithMessage("Slug cannot exceed 200 characters");
        
        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("Owner ID is required");
        
        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}

public class UpdateWorkspaceValidator : AbstractValidator<UpdateWorkspaceCommand>
{
    public UpdateWorkspaceValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Workspace ID is required");
        
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Workspace name is required")
            .MaximumLength(200).WithMessage("Workspace name cannot exceed 200 characters");
        
        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}
