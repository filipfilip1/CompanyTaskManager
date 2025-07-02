using CompanyTaskManager.Application.Services.Notifications;
using CompanyTaskManager.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompanyTaskManager.Web.Controllers;

[Authorize]
public class NotificationController(INotificationService _notificationService,
    UserManager<ApplicationUser> _userManager,
    ILogger<NotificationController> _logger) : Controller
{
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            _logger.LogWarning("User not found when trying to access notifications");
            return Forbid();
        }

        var userName = user.UserName;
        
        try
        {
            _logger.LogInformation("User {UserName} ({UserId}) is accessing notifications", 
                userName, user.Id);
                
            var allNotifications = await _notificationService.GetAllNotificationsForUserAsync(user.Id);
            
            _logger.LogInformation("Successfully retrieved {NotificationCount} notifications for user {UserName}", 
                allNotifications.Count(), userName);
                
            return View(allNotifications);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving notifications for user {UserName} ({UserId})", 
                userName, user.Id);
            throw;
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            _logger.LogWarning("User not found when trying to mark notification {NotificationId} as read", id);
            return Forbid();
        }

        var userName = user.UserName;
        
        try
        {
            _logger.LogInformation("User {UserName} ({UserId}) is marking notification {NotificationId} as read", 
                userName, user.Id, id);
                
            await _notificationService.MarkAsReadAsync(id);
            
            _logger.LogInformation("Notification {NotificationId} successfully marked as read by user {UserName}", 
                id, userName);
                
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking notification {NotificationId} as read by user {UserName} ({UserId})", 
                id, userName, user.Id);
            throw;
        }
    }
}
