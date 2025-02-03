using CompanyTaskManager.Application.Services.Calendars;
using CompanyTaskManager.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompanyTaskManager.Web.Controllers;

public class CalendarController(ICalendarService _calendarService,
    UserManager<ApplicationUser> _userManager) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> GetCalendarEvents(DateTime? date)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Forbid();

        var events = await _calendarService.GetAllEventsAsync(user.Id);

        return Json(events);
    }
}
