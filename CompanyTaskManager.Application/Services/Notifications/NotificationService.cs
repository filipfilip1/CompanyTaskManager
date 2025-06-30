using AutoMapper;
using CompanyTaskManager.Application.ViewModels.Notification;
using CompanyTaskManager.Data;
using CompanyTaskManager.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CompanyTaskManager.Application.Services.Notifications;

public class NotificationService(
    ApplicationDbContext _context,
    IMapper _mapper,
    ILogger<NotificationService> _logger) : INotificationService
{

    public async Task<List<NotificationViewModel>> GetUnreadNotificationsAsync(string userId)
    {
        _logger.LogInformation("Fetching unread notifications for user {UserId}", userId);
        
        var notifications = await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .Include(n => n.NotificationType)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        _logger.LogDebug("Found {Count} unread notifications for user {UserId}", notifications.Count, userId);
        return _mapper.Map<List<NotificationViewModel>>(notifications);
    }

    public async Task MarkAsReadAsync(int notificationId)
    {
        _logger.LogInformation("Marking notification {NotificationId} as read", notificationId);
        
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification != null)
        {
            notification.IsRead = true;
            await _context.SaveChangesAsync();
            _logger.LogDebug("Notification {NotificationId} marked as read for user {UserId}", notificationId, notification.UserId);
        }
        else
        {
            _logger.LogWarning("Notification {NotificationId} not found", notificationId);
        }
    }


    public async Task CreateNotificationAsync(string userId, string message, int notificationTypeId)
    {
        _logger.LogInformation("Creating notification for user {UserId} with type {NotificationTypeId}", userId, notificationTypeId);
        
        var notification = new Notification
        {
            UserId = userId,
            Message = message,
            NotificationTypeId = notificationTypeId
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
        
        _logger.LogDebug("Notification created with ID {NotificationId} for user {UserId}", notification.Id, userId);
    }

    public async Task<List<NotificationViewModel>> GetAllNotificationsForUserAsync(string userId)
    {
        _logger.LogInformation("Fetching all notifications for user {UserId}", userId);
        
        var notifications = await _context.Notifications
            .Where(n => n.UserId == userId)
            .Include(n => n.NotificationType)
            .OrderBy(n => n.IsRead)
            .ThenByDescending(n => n.CreatedAt)
            .ToListAsync();

        _logger.LogDebug("Found {Count} total notifications for user {UserId}", notifications.Count, userId);
        return _mapper.Map<List<NotificationViewModel>>(notifications);
    }

}
