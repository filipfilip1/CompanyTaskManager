using CompanyTaskManager.Application.Services.Notifications;
using CompanyTaskManager.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompanyTaskManager.Web.Controllers;

[Authorize]
public class NotificationController(INotificationService _notificationService,
    UserManager<ApplicationUser> _userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var allNotifications = await _notificationService.GetAllNotificationsForUserAsync(user.Id);
        return View(allNotifications);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        await _notificationService.MarkAsReadAsync(id);
        return RedirectToAction("Index");
    }
}
