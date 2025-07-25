﻿using CompanyTaskManager.Application.Exceptions;
using CompanyTaskManager.Application.Services.Notifications;
using CompanyTaskManager.Application.ViewModels.User;
using CompanyTaskManager.Common.Static;
using CompanyTaskManager.Data;
using CompanyTaskManager.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CompanyTaskManager.Application.Services.Teams;

public class TeamService(
    ApplicationDbContext _context,
    INotificationService _notificationService,
    UserManager<ApplicationUser> _userManager,
    ILogger<TeamService> _logger) : ITeamService
{
    public async Task AddMemberAsync(string teamId, string userId)
    {
        _logger.LogInformation("Adding user {UserId} to team {TeamId}", userId, teamId);
        
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("Failed to add user {UserId} to team {TeamId}: User not found", userId, teamId);
            throw new NotFoundException("User", userId);
        }

        var team = await _context.Teams
            .FirstOrDefaultAsync(t => t.Id == teamId);

        if (team == null)
        {
            _logger.LogWarning("Failed to add user {UserId} to team {TeamId}: Team not found", userId, teamId);
            throw new NotFoundException("Team", teamId);
        }

        user.TeamId = teamId;

        var message = $"You have been added to team {team.Name}";
        await _notificationService.CreateNotificationAsync(userId, message, 3); // 3 is the notification type id for Added To Team

        await _context.SaveChangesAsync();
        _logger.LogInformation("Successfully added user {UserId} to team {TeamId} ({TeamName})", userId, teamId, team.Name);
    }
    public async Task RemoveMemberAsync(string teamId, string userId)
    {
        _logger.LogInformation("Removing user {UserId} from team {TeamId}", userId, teamId);
        
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("Failed to remove user {UserId} from team {TeamId}: User not found", userId, teamId);
            throw new NotFoundException("User", userId);
        }
        
        if (user.TeamId != teamId)
        {
            _logger.LogWarning("Failed to remove user {UserId} from team {TeamId}: User not in this team", userId, teamId);
            throw new ValidationException("User is not a member of this team");
        }

        var team = await _context.Teams
            .FirstOrDefaultAsync(t => t.Id == teamId);

        if (team == null)
        {
            _logger.LogWarning("Failed to remove user {UserId} from team {TeamId}: Team not found", userId, teamId);
            throw new NotFoundException("Team", teamId);
        }

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
        var userWithoutTeam = await _context.Users
            .Where(u => u.TeamId == null)
            .ToListAsync();

        var result = new List<UserViewModel>();   

        foreach (var user in userWithoutTeam)
        {
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains(Roles.Administrator) || roles.Contains(Roles.Manager))
                continue;

            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles.ToList(),
                IsLockedOut = user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow
            };

            result.Add(userViewModel);
        }

        return result;
    }

    public async Task<List<UserViewModel>> GetTeamMembersAsync(string teamId)
    {
        var userInTeam = await _context.Users
            .Where(u => u.TeamId == teamId)
            .ToListAsync();

        var result = new List<UserViewModel>();

        foreach (var user in userInTeam)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles.ToList(),
                IsLockedOut = user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow
            };

            result.Add(userViewModel);
        }   

        return result;
    }

    public async Task<List<UserViewModel>> GetProjectMembersAsync(int projectId)
    {

        var projectUsers = await _context.ProjectUsers
            .Where(pu => pu.ProjectId == projectId)
            .Select(pu => pu.User)
            .ToListAsync();

        var result = new List<UserViewModel>();
        foreach (var user in projectUsers)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles.ToList(),
                IsLockedOut = user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow
            };

            result.Add(userViewModel);
        }

        return result;
    }

    public async Task<List<UserViewModel>> GetTeamMembersForCreateTaskAsync(string teamId)
    {
        var team = await _context.Teams
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == teamId);

        if (team == null)
            throw new NotFoundException("Team", teamId);

        // exclude manager from team members
        var members = team.Members
            .Where(u => u.Id != team.ManagerId)
            .ToList();

        var result = new List<UserViewModel>();

        foreach (var user in members)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            
            // exclude administrators from team members
            if (userRoles.Contains(Roles.Administrator)) continue;

            var userVm = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = userRoles.ToList(),
                IsLockedOut = user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.UtcNow
            };

            result.Add(userVm);
        }

        return result;
    }



}
