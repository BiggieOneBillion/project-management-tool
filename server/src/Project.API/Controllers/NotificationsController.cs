using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.APPLICATION.Commands.Notification;
using Project.APPLICATION.Queries.Notification;
using System.Security.Claims;

namespace Project.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public NotificationsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUserNotifications([FromQuery] bool? onlyUnread = null)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { success = false, message = "User not authenticated" });
            
            var query = new GetUserNotificationsQuery(userId, onlyUnread);
            var notifications = await _mediator.Send(query);
            return Ok(new { success = true, data = notifications });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { success = false, message = "User not authenticated" });
            
            var query = new GetUnreadCountQuery(userId);
            var count = await _mediator.Send(query);
            return Ok(new { success = true, data = count });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpPut("{notificationId}/mark-read")]
    public async Task<IActionResult> MarkAsRead(string notificationId)
    {
        try
        {
            var command = new MarkNotificationAsReadCommand(notificationId);
            await _mediator.Send(command);
            return Ok(new { success = true, message = "Notification marked as read" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpPut("mark-all-read")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { success = false, message = "User not authenticated" });
            
            var command = new MarkAllAsReadCommand(userId);
            await _mediator.Send(command);
            return Ok(new { success = true, message = "All notifications marked as read" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
}
