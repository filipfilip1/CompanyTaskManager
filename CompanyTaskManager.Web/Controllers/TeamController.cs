using CompanyTaskManager.Application.Services.Teams;
using CompanyTaskManager.Application.ViewModels.Team;
using CompanyTaskManager.Application.ViewModels.User;
using CompanyTaskManager.Common.Static;
using CompanyTaskManager.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompanyTaskManager.Web.Controllers;

[Authorize(Roles = Roles.Manager)]
public class TeamController(ITeamService _teamService,
    UserManager<ApplicationUser> _userManager) : Controller
{

    public async Task<IActionResult> ManageTeam()
    {
        var manager = await _userManager.GetUserAsync(User);
        var teamMembers = await _teamService.GetTeamMembersAsync(manager.Id);

        var model = new ManageTeamViewModel
        {
            TeamMembers = teamMembers
        };

        return View(model);
    }

    public async Task<IActionResult> AddTeamMember()
    {
        var manager = await _userManager.GetUserAsync(User);
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
        await _teamService.AddMemberAsync(manager.Id, userId);
        return RedirectToAction("AddTeamMember");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveMember(string userId)
    {
        var manager = await _userManager.GetUserAsync(User);
        await _teamService.RemoveMemberAsync(manager.Id, userId);
        return RedirectToAction("ManageTeam");
    }
}
