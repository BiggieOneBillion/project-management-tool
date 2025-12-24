using AutoMapper;
using MediatR;
using Project.APPLICATION.DTOs.Notification;
using Project.APPLICATION.Queries.Notification;
using Project.CORE.Interfaces;

namespace Project.APPLICATION.Queries.Notification;

public class NotificationQueryHandlers :
    IRequestHandler<GetUserNotificationsQuery, List<NotificationDto>>,
    IRequestHandler<GetUnreadCountQuery, int>
{
    private readonly IRepository<CORE.Entities.Notification> _notificationRepository;
    private readonly IMapper _mapper;
    
    public NotificationQueryHandlers(IRepository<CORE.Entities.Notification> notificationRepository, IMapper mapper)
    {
        _notificationRepository = notificationRepository;
        _mapper = mapper;
    }
    
    public async Task<List<NotificationDto>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
    {
        var allNotifications = await _notificationRepository.GetAllAsync();
        var userNotifications = allNotifications
            .Where(n => n.UserId == request.UserId);
        
        if (request.OnlyUnread == true)
        {
            userNotifications = userNotifications.Where(n => !n.IsRead);
        }
        
        var notifications = userNotifications
            .OrderByDescending(n => n.CreatedAt)
            .ToList();
        
        return _mapper.Map<List<NotificationDto>>(notifications);
    }
    
    public async Task<int> Handle(GetUnreadCountQuery request, CancellationToken cancellationToken)
    {
        var allNotifications = await _notificationRepository.GetAllAsync();
        return allNotifications.Count(n => n.UserId == request.UserId && !n.IsRead);
    }
}
