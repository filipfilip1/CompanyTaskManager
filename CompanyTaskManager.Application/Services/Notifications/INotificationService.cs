using CompanyTaskManager.Application.ViewModels.Notification;

namespace CompanyTaskManager.Application.Services.Notifications;

public interface INotificationService
{
    Task<List<NotificationViewModel>> GetUnreadNotificationsAsync(string userId);
    Task MarkAsReadAsync(int notificationId);
    Task CreateNotificationAsync(string userId, string message, int notificationTypeId);
    Task<List<NotificationViewModel>> GetAllNotificationsForUserAsync(string userId);
}
