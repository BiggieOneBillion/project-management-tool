namespace Project.CORE.Exceptions;

public class EntityNotFoundException : DomainException
{
    public EntityNotFoundException(string entityName, string entityId)
        : base($"{entityName} with ID '{entityId}' was not found.")
    {
        EntityName = entityName;
        EntityId = entityId;
    }
    
    public string EntityName { get; }
    public string EntityId { get; }
}
