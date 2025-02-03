
using CompanyTaskManager.Application.Dto;

namespace CompanyTaskManager.Application.Services.Calendars;

public interface ICalendarService
{
    Task<List<CalendarEventDto>> GetAllEventsAsync(string userId);
}
