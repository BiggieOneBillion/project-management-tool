using FluentValidation;
using Project.APPLICATION.Commands.Task;

namespace Project.APPLICATION.Validators;

public class CreateTaskValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage("Project ID is required");
        
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Task title is required")
            .MaximumLength(300).WithMessage("Title cannot exceed 300 characters");
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Task description is required")
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters");
        
        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Task type is required")
            .Must(t => new[] { "FEATURE", "BUG", "TASK", "IMPROVEMENT", "OTHER" }.Contains(t))
            .WithMessage("Type must be FEATURE, BUG, TASK, IMPROVEMENT, or OTHER");
        
        RuleFor(x => x.Priority)
            .NotEmpty().WithMessage("Priority is required")
            .Must(p => new[] { "LOW", "MEDIUM", "HIGH" }.Contains(p))
            .WithMessage("Priority must be LOW, MEDIUM, or HIGH");
        
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required")
            .Must(s => new[] { "TODO", "IN_PROGRESS", "DONE", "BLOCKED" }.Contains(s))
            .WithMessage("Status must be TODO, IN_PROGRESS, DONE, or BLOCKED");
        
        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Due date must be in the future");
    }
}

public class UpdateTaskValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Task ID is required");
        
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Task title is required")
            .MaximumLength(300).WithMessage("Title cannot exceed 300 characters");
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Task description is required")
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters");
        
        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Task type is required")
            .Must(t => new[] { "FEATURE", "BUG", "TASK", "IMPROVEMENT", "OTHER" }.Contains(t))
            .WithMessage("Type must be FEATURE, BUG, TASK, IMPROVEMENT, or OTHER");
        
        RuleFor(x => x.Priority)
            .NotEmpty().WithMessage("Priority is required")
            .Must(p => new[] { "LOW", "MEDIUM", "HIGH" }.Contains(p))
            .WithMessage("Priority must be LOW, MEDIUM, or HIGH");
        
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required")
            .Must(s => new[] { "TODO", "IN_PROGRESS", "DONE", "BLOCKED" }.Contains(s))
            .WithMessage("Status must be TODO, IN_PROGRESS, DONE, or BLOCKED");
    }
}
