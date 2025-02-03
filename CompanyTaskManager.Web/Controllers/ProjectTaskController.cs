using CompanyTaskManager.Application.Services.TaskItems;
using CompanyTaskManager.Application.Services.Teams;
using CompanyTaskManager.Application.ViewModels.TaskItem;
using CompanyTaskManager.Common.Static;
using CompanyTaskManager.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompanyTaskManager.Web.Controllers;

public class ProjectTaskController(IProjectTaskService _projectTaskService,
    ITeamService _teamService,
    UserManager<ApplicationUser> _userManager) : Controller
{

    [Authorize(Roles = Roles.Employee)]
    public async Task<IActionResult> EmployeeTaskDetails(int taskId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Forbid();

        var task = await _projectTaskService.GetEmployeeTaskDetailsAsync(taskId, user.Id);
        if (task == null)
            return NotFound();

        return View("EmployeeTaskDetails", task);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Employee)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendForApproval(int taskId, string submissionText)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Forbid();

        // Update submission text
        await _projectTaskService.UpdateSubmissionTextAsync(taskId, user.Id, submissionText);

        // Send for approval
        await _projectTaskService.SendForApprovalAsync(taskId, user.Id);


        return RedirectToAction(nameof(EmployeeTaskDetails), new { taskId });
    }

    [Authorize(Roles = Roles.Manager)]
    public async Task<IActionResult> ManagerDetails(int taskId)
    {
        var manager = await _userManager.GetUserAsync(User);
        if (manager == null)
            return Forbid();

        var vm = await _projectTaskService.GetManagerTaskDetailsAsync(taskId, manager.Id);
        if (vm == null)
            return NotFound();

        return View("ManagerDetails", vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int taskId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Forbid();

        var id = await _projectTaskService.ApproveProjectTaskAsync(taskId, user.Id);

        return RedirectToAction("Details", "Project", new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(int taskId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Forbid();

        var projectId = await _projectTaskService.RejectProjectTaskAsync(taskId, user.Id);

        return RedirectToAction("Details", "Project", new { projectId });
    }

    [Authorize(Roles = Roles.Manager)]
    public async Task<IActionResult> Create(int projectId)
    {
        var manager = await _userManager.GetUserAsync(User);
        if (manager == null)
            return Forbid();

        var members = await _teamService.GetProjectMembersAsync(projectId);

        var model = new CreateTaskItemViewModel
        {
            ProjectId = projectId,
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(1),
            TeamMembers = members.Select(m => new SelectListItem
            {
                Text = m.UserName,
                Value = m.Id
            }).ToList()
        };

        return View("Create", model);
    }


    [HttpPost]
    [Authorize(Roles = Roles.Manager)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTaskItemViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var manager = await _userManager.GetUserAsync(User);
            if (manager == null)
                return Forbid();

            var members = await _teamService.GetProjectMembersAsync(model.ProjectId.Value);

            model.TeamMembers = members.Select(m => new SelectListItem
            {
                Text = m.UserName,
                Value = m.Id
            }).ToList();

            return View("Create", model);
        }

        var managerUser = await _userManager.GetUserAsync(User);
        if (managerUser == null)
            return Forbid();

        await _projectTaskService.CreateProjectTaskAsync(model, managerUser.Id);

        return RedirectToAction(nameof(ProjectController.Index), "Project", new { projectId = model.ProjectId });
    }

    [Authorize(Roles = Roles.Employee)]
    public async Task<IActionResult> LeaderTaskDetails(int taskId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Forbid();

        var task = await _projectTaskService.GetLeaderTaskDetailsAsync(taskId, user.Id);
        if (task == null)
            return NotFound();

        return View("LeaderTaskDetails", task);
    }


}
