using FluentValidation.Results;

namespace Project.APPLICATION.Exceptions;

public class ValidationException : ApplicationException
{
    public List<string> Errors { get; }
    
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : base("One or more validation failures have occurred.")
    {
        Errors = failures
            .Select(f => f.ErrorMessage)
            .ToList();
    }
    
    public ValidationException(string message) : base(message)
    {
        Errors = new List<string> { message };
    }
}
