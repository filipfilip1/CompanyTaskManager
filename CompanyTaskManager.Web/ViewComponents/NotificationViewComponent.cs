using CompanyTaskManager.Application.Services.Notifications;
using CompanyTaskManager.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanyTaskManager.Web.ViewComponents;

public class NotificationViewComponent(INotificationService _notificationService,
    UserManager<ApplicationUser> _userManager) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = await _userManager.GetUserAsync((ClaimsPrincipal)User);
        var notifications = await _notificationService.GetUnreadNotificationsAsync(user.Id);
        var notificationCount = notifications.Count;

        return View(notificationCount);
    }
}
