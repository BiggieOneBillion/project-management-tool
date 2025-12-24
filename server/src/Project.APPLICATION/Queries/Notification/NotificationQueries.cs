using MediatR;
using Project.APPLICATION.DTOs.Notification;

namespace Project.APPLICATION.Queries.Notification;

public record GetUserNotificationsQuery(
    string UserId,
    bool? OnlyUnread = null
) : IRequest<List<NotificationDto>>;

public record GetUnreadCountQuery(string UserId) : IRequest<int>;
