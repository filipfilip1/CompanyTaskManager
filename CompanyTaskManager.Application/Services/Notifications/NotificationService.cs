

using AutoMapper;
using CompanyTaskManager.Application.ViewModels.Notification;
using CompanyTaskManager.Data;
using CompanyTaskManager.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyTaskManager.Application.Services.Notifications;

public class NotificationService(ApplicationDbContext _context,
    IMapper _mapper) : INotificationService
{

    public async Task<List<NotificationViewModel>> GetUnreadNotificationsAsync(string userId)
    {
        var notifications = await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .Include(n => n.NotificationType)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        return _mapper.Map<List<NotificationViewModel>>(notifications);
    }

    public async Task MarkAsReadAsync(int notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification != null)
        {
            notification.IsRead = true;
            await _context.SaveChangesAsync();
        }
    }


    public Task CreateNotificationAsync(string userId, string message, int notificationTypeId)
    {
        var notification = new Notification
        {
            UserId = userId,
            Message = message,
            NotificationTypeId = notificationTypeId
        };

        _context.Notifications.Add(notification);
        return _context.SaveChangesAsync();
    }

    public async Task<List<NotificationViewModel>> GetAllNotificationsForUserAsync(string userId)
    {
        var notifications = await _context.Notifications
            .Where(n => n.UserId == userId)
            .Include(n => n.NotificationType)
            .OrderBy(n => n.IsRead)
            .ThenByDescending(n => n.CreatedAt)
            .ToListAsync();

        return _mapper.Map<List<NotificationViewModel>>(notifications);
    }

}
