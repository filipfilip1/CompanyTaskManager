
using CompanyTaskManager.Application.Services.Notifications;
using CompanyTaskManager.Application.ViewModels.User;
using CompanyTaskManager.Common.Static;
using CompanyTaskManager.Data;
using CompanyTaskManager.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CompanyTaskManager.Application.Services.Teams;

public class TeamService(ApplicationDbContext _context,
    INotificationService _notificationService) : ITeamService
{
    public async Task AddMemberAsync(string teamId, string userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            throw new Exception("User not found");

        var team = await _context.Teams
            .FirstOrDefaultAsync(t => t.Id == teamId);

        if (team == null)
            throw new Exception("Team not found");

       user.TeamId = teamId;

       var message = $"You have been added to team {team.Name}";
       await _notificationService.CreateNotificationAsync(userId, message, 3); // 3 is the notification type id for Added To Team


       await _context.SaveChangesAsync();
    }
    public async Task RemoveMemberAsync(string teamId, string userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null || user.TeamId != teamId)
            throw new Exception("User not in this team");

        var team = await _context.Teams
            .FirstOrDefaultAsync(t => t.Id == teamId);

        if (team == null)
            throw new Exception("Team not found");

        user.TeamId = null;

        var message = $"You have been removed from team {team.Name}";
        await _notificationService.CreateNotificationAsync(userId, message, 4); // 4 is the notification type id for Removed From Team

        await _context.SaveChangesAsync();
    }

    public Task CreateTeamAsync(string managerId, string teamName)
    {
        var team = new Team
        {
            Id = managerId,
            Name = teamName,
            ManagerId = managerId
        };

        _context.Teams.Add(team);
        return _context.SaveChangesAsync();
    }

    public async Task<List<UserViewModel>> GetAvailableUserAsync()
    {
        var users = await _context.Users
            .Where(u => u.TeamId == null && !u.Roles.Any(r => r.Name == Roles.Administrator &&  r.Name == Roles.Manager))
            .Select(u => new UserViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Roles = u.Roles.Select(r => r.Name).ToList(),
                IsLockedOut = u.LockoutEnd.HasValue && u.LockoutEnd > DateTimeOffset.UtcNow
            })
            .ToListAsync();

        return users;
    }

    public Task<List<UserViewModel>> GetTeamMembersAsync(string teamId)
    {
        var users = _context.Users
            .Where(u => u.TeamId == teamId)
            .Select(u => new UserViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Roles = u.Roles.Select(r => r.Name).ToList(),
                IsLockedOut = u.LockoutEnd.HasValue && u.LockoutEnd > DateTimeOffset.UtcNow
            })
            .ToListAsync();

        return users;
    }

}
