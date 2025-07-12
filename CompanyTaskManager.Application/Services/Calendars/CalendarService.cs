using CompanyTaskManager.Application.Dto;
using CompanyTaskManager.Application.Exceptions;
using CompanyTaskManager.Data;
using Microsoft.EntityFrameworkCore;
using CompanyTaskManager.Common.Static;
using Microsoft.Extensions.Logging;

namespace CompanyTaskManager.Application.Services.Calendars;

public class CalendarService(
    ApplicationDbContext _context,
    ILogger<CalendarService> _logger) : ICalendarService

{
    public async Task<List<CalendarEventDto>> GetAllEventsAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            _logger.LogWarning("GetAllEventsAsync called with null or empty userId");
            throw new ValidationException("User ID cannot be null or empty");
        }
        
        _logger.LogInformation("Fetching all calendar events for user {UserId}", userId);
        // fetch projects related to manager, leader and employee
        var projectEvents = await _context.Projects
            .Include(p => p.WorkStatus)
            .Where(p => p.ManagerId == userId
                 || p.LeaderId == userId
                 || p.ProjectUsers.Any(pu => pu.UserId == userId))
             .Where( p => p.WorkStatus.Name != WorkStatusesName.Completed
                && p.WorkStatus.Name != WorkStatusesName.Rejected)
            .Select(p => new CalendarEventDto
            {
                Id = "P" + p.Id,
                Title = p.Name,
                Start = p.EndDate,
                End = p.EndDate,
                Type = "project"
            })
            .ToListAsync();
        
        // fetch tasks related to user and user's team manager
        var taskItemEvents = await _context.TaskItems
            .Include(t => t.WorkStatus)
            .Include(t => t.AssignedUser).ThenInclude(u => u.Team)
            .Where(t =>
                 t.AssignedUserId == userId
                || (t.AssignedUser != null && t.AssignedUser.Team != null && t.AssignedUser.Team.ManagerId == userId)
            )
            .Where(t =>
                t.WorkStatus.Name != WorkStatusesName.Completed
                && t.WorkStatus.Name != WorkStatusesName.Rejected
            )
            .Select(t => new CalendarEventDto
            {
                Id = "T" + t.Id,
                Title = t.Title,
                Start = t.EndDate,
                End = t.EndDate,
                Type = "task"
            })
            .ToListAsync();

        // fetch project tasks related to user and leader
        var projectTaskEvents = await _context.TaskItems
            .Include(t => t.WorkStatus)
            .Include(t => t.AssignedUser).ThenInclude(u => u.Team)
            .Include(t => t.Project)
            .Where(t =>
                 t.ProjectId != null && 
                 (t.AssignedUserId == userId || t.Project.LeaderId == userId)
            )
            .Where(t =>
                t.WorkStatus.Name != WorkStatusesName.Completed
                && t.WorkStatus.Name != WorkStatusesName.Rejected
            )
            .Select(t => new CalendarEventDto
            {
                Id = "PT" + t.Id,
                Title = t.Title,
                Start = t.EndDate,
                End = t.EndDate,
                Type = "projectTask",
                IsLeader = t.Project != null && t.Project.LeaderId == userId
            })
            .ToListAsync();

        // Combine all events
        var allEvents = new List<CalendarEventDto>();
        allEvents.AddRange(projectEvents);
        allEvents.AddRange(taskItemEvents);
        allEvents.AddRange(projectTaskEvents);

        _logger.LogDebug("Found {ProjectCount} project events, {TaskCount} task events, {ProjectTaskCount} project task events for user {UserId}", 
            projectEvents.Count, taskItemEvents.Count, projectTaskEvents.Count, userId);
        _logger.LogInformation("Successfully retrieved {TotalCount} calendar events for user {UserId}", allEvents.Count, userId);

        return allEvents;
    }

}
