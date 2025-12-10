using MediatR;

namespace Project.APPLICATION.Commands.Notification;

public record MarkNotificationAsReadCommand(string NotificationId) : IRequest<Unit>;

public record MarkAllAsReadCommand(string UserId) : IRequest<Unit>;
