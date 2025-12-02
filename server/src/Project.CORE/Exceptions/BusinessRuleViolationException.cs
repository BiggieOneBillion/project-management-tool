namespace Project.CORE.Exceptions;

public class BusinessRuleViolationException : DomainException
{
    public BusinessRuleViolationException(string message) : base(message)
    {
    }
    
    public BusinessRuleViolationException(string rule, string details)
        : base($"Business rule '{rule}' violated: {details}")
    {
        Rule = rule;
        Details = details;
    }
    
    public string? Rule { get; }
    public string? Details { get; }
}
