using Project.APPLICATION.DTOs.User;

namespace Project.APPLICATION.DTOs.Notification;

public class NotificationDto
{
    public string Id { get; init; } = "";
    public string UserId { get; init; } = "";
    public string Type { get; init; } = "";
    public string Title { get; init; } = "";
    public string Message { get; init; } = "";
    public string? RelatedEntityId { get; init; }
    public string? RelatedEntityType { get; init; }
    public bool IsRead { get; init; }
    public DateTime CreatedAt { get; init; }
}
