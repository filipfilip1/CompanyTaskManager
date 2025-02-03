

using CompanyTaskManager.Application.Dto;
using CompanyTaskManager.Data;
using Microsoft.EntityFrameworkCore;
using CompanyTaskManager.Common.Static;

namespace CompanyTaskManager.Application.Services.Calendars;

public class CalendarService(
    ApplicationDbContext _context) : ICalendarService

{
    public async Task<List<CalendarEventDto>> GetAllEventsAsync(string userId)
    {
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
                || t.AssignedUser.Team.ManagerId == userId
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
                IsLeader = t.Project.LeaderId == userId
            })
            .ToListAsync();

        // Combine all events
        var allEvents = new List<CalendarEventDto>();
        allEvents.AddRange(projectEvents);
        allEvents.AddRange(taskItemEvents);
        allEvents.AddRange(projectTaskEvents);

        return allEvents;
    }

}
