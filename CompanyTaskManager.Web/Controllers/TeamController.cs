using CompanyTaskManager.Application.Services.Teams;
using CompanyTaskManager.Application.ViewModels.Team;
using CompanyTaskManager.Common.Static;
using CompanyTaskManager.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompanyTaskManager.Web.Controllers;

[Authorize(Roles = Roles.Manager)]
public class TeamController(
    ITeamService _teamService,
    UserManager<ApplicationUser> _userManager,
    ILogger<TeamController> _logger) : Controller
{

    public async Task<IActionResult> ManageTeam()
    {
        var manager = await _userManager.GetUserAsync(User);
        if (manager == null)
        {
            _logger.LogWarning("Manager not found when trying to access ManageTeam");
            return Forbid();
        }

        var teamId = manager.Id;
        var teamMembers = await _teamService.GetTeamMembersAsync(teamId);

        var model = new ManageTeamViewModel
        {
            TeamMembers = teamMembers
        };

        return View(model);
    }

    public async Task<IActionResult> AddTeamMember()
    {
        var availableUsers = await _teamService.GetAvailableUserAsync();

        var model = new AddTeamMemberViewModel
        {
            AvailableUsers = availableUsers
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddMember(string userId)
    {
        var manager = await _userManager.GetUserAsync(User);
        var managerName = manager?.UserName ?? "Unknown";
        
        _logger.LogInformation("Manager {ManagerName} ({ManagerId}) is adding user {UserId} to their team", 
            managerName, manager?.Id, userId);
            
        await _teamService.AddMemberAsync(manager.Id, userId);
        
        _logger.LogInformation("User {UserId} successfully added to team by manager {ManagerName}", 
            userId, managerName);
            
        return RedirectToAction("AddTeamMember");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveMember(string userId)
    {
        var manager = await _userManager.GetUserAsync(User);
        var managerName = manager?.UserName ?? "Unknown";
        
        _logger.LogInformation("Manager {ManagerName} ({ManagerId}) is removing user {UserId} from their team", 
            managerName, manager?.Id, userId);
            
        await _teamService.RemoveMemberAsync(manager.Id, userId);
        
        _logger.LogInformation("User {UserId} successfully removed from team by manager {ManagerName}", 
            userId, managerName);
            
        return RedirectToAction("ManageTeam");
    }

    public async Task<IActionResult> GetFilteredTeamMembers(string leaderId)
    {
        var manager = await _userManager.GetUserAsync(User);
        if (manager == null)
        {
            _logger.LogWarning("Manager not found when trying to access GetFilteredTeamMembers");
            return Forbid();
        }

        var teamMembers = await _teamService.GetTeamMembersAsync(manager.Id);

        var filteredMembers = teamMembers
            .Where(m => m.Id != leaderId)
            .Select(m => new SelectListItem(m.UserName, m.Id))
            .ToList();

        return PartialView("_TeamMembersPartial", filteredMembers);
    }
}
