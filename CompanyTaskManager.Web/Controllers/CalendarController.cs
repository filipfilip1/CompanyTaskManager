using CompanyTaskManager.Application.Services.Calendars;
using CompanyTaskManager.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompanyTaskManager.Web.Controllers;

public class CalendarController(ICalendarService _calendarService,
    UserManager<ApplicationUser> _userManager,
    ILogger<CalendarController> _logger) : Controller
{
    public IActionResult Index()
    {
        var userName = User?.Identity?.Name ?? "Anonymous";
        _logger.LogInformation("User {UserName} accessed calendar index page", userName);
        return View();
    }

    public async Task<IActionResult> GetCalendarEvents(DateTime? date)
    {
        var user = await _userManager.GetUserAsync(User);
        var userName = user?.UserName ?? "Unknown";
        
        if (user == null)
        {
            _logger.LogWarning("Unauthorized access attempt to calendar events");
            return Forbid();
        }

        try
        {
            _logger.LogInformation("User {UserName} ({UserId}) is fetching calendar events for date {Date}", 
                userName, user.Id, date?.ToString("yyyy-MM-dd") ?? "all dates");
                
            var events = await _calendarService.GetAllEventsAsync(user.Id);
            
            _logger.LogInformation("Successfully retrieved {EventCount} calendar events for user {UserName}", 
                events.Count(), userName);

            return Json(events);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving calendar events for user {UserName} ({UserId})", 
                userName, user.Id);
            throw;
        }
    }
}
