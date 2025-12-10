using MediatR;
using Project.APPLICATION.Commands.Notification;
using Project.CORE.Interfaces;

namespace Project.APPLICATION.Commands.Notification;

public class NotificationCommandHandlers :
    IRequestHandler<MarkNotificationAsReadCommand, Unit>,
    IRequestHandler<MarkAllAsReadCommand, Unit>
{
    private readonly IRepository<CORE.Entities.Notification> _notificationRepository;
    
    public NotificationCommandHandlers(IRepository<CORE.Entities.Notification> notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }
    
    public async Task<Unit> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
    {
        var notification = await _notificationRepository.GetByIdAsync(request.NotificationId);
        if (notification == null)
            throw new KeyNotFoundException($"Notification with ID {request.NotificationId} not found");
        
        notification.IsRead = true;
        await _notificationRepository.UpdateAsync(notification);
        
        return Unit.Value;
    }
    
    public async Task<Unit> Handle(MarkAllAsReadCommand request, CancellationToken cancellationToken)
    {
        var allNotifications = await _notificationRepository.GetAllAsync();
        var userNotifications = allNotifications
            .Where(n => n.UserId == request.UserId && !n.IsRead)
            .ToList();
        
        foreach (var notification in userNotifications)
        {
            notification.IsRead = true;
            await _notificationRepository.UpdateAsync(notification);
        }
        
        return Unit.Value;
    }
}
