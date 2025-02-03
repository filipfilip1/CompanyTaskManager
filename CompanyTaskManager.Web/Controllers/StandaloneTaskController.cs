using CompanyTaskManager.Application.Services.TaskItems;
using CompanyTaskManager.Application.Services.Teams;
using CompanyTaskManager.Application.Services.WorkStatuses;
using CompanyTaskManager.Application.ViewModels.TaskItem;
using CompanyTaskManager.Common.Static;
using CompanyTaskManager.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompanyTaskManager.Web.Controllers;

[Authorize]
public class StandaloneTaskController(
    IStandaloneTaskService _standaloneTaskService,
    IWorkStatusService _workStatusService,
    UserManager<ApplicationUser> _userManager,
    ITeamService _teamService) : Controller

{

    [Authorize(Roles = Roles.Employee)]
    public async Task<IActionResult> MyTasks(string statusFilter, bool showOnlyOverdue)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Forbid();

        var tasks = await _standaloneTaskService.GetTasksForEmployeeAsync(user.Id);

        var allStatuses = await _workStatusService.GetAllWorkStatusesAsync();
        ViewBag.Statuses = allStatuses;

        if (!string.IsNullOrEmpty(statusFilter))
        {
            tasks = tasks
                .Where(t => t.WorkStatusName == statusFilter)
                .ToList();
        }

        if (showOnlyOverdue)
        {
            tasks = tasks
                .Where(t => t.IsOverdue)
                .ToList();

            tasks = tasks.OrderByDescending(t => t.EndDate).ToList();
        }
        else
        {

            tasks = tasks
                .OrderBy(t => t.IsOverdue).ThenBy(t => t.IsCompleted)            
                .ThenByDescending(t => t.EndDate)
                .ToList();
        }


        return View("MyTasks", tasks);
    }

    [Authorize(Roles = Roles.Manager)]
    public async Task<IActionResult> ManagerTasks(string statusFilter, string assignedUser, bool showOnlyOverdue = false)
    {
        var manager = await _userManager.GetUserAsync(User);
        if (manager == null)
            return Forbid();

        var tasks = await _standaloneTaskService.GetTasksForManagerAsync(manager.Id);

        var allStatuses = await _workStatusService.GetAllWorkStatusesAsync();
        ViewBag.Statuses = allStatuses;

        if (!string.IsNullOrEmpty(statusFilter))
        {
            tasks = tasks
                .Where(t => t.WorkStatusName == statusFilter)
                .ToList();
        }

        if (!string.IsNullOrEmpty(assignedUser))
        {
            tasks = tasks
                .Where(t => t.AssignedUserName.Contains(assignedUser, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (showOnlyOverdue)
        {
            tasks = tasks
                .Where(t => t.IsOverdue)
                .ToList();
            
            tasks = tasks.OrderByDescending(t => t.EndDate).ToList();
        } else
        {
            tasks = tasks
                .OrderBy(t => t.IsOverdue).ThenBy(t => t.IsCompleted)             
                .ThenByDescending(t => t.EndDate)       
                .ToList();
        }

        return View("ManagerTasks", tasks);
    }

    [Authorize(Roles = Roles.Employee)]
    public async Task<IActionResult> EmployeeTaskDetails(int taskId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Forbid();

        var task = await _standaloneTaskService.GetEmployeeTaskDetailsAsync(taskId, user.Id);
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

        // Save submission text
        await _standaloneTaskService.UpdateSubmissionTextAsync(taskId, user.Id, submissionText);

        // Send for approval
        await _standaloneTaskService.SendForApprovalAsync(taskId, user.Id);

        return RedirectToAction(nameof(MyTasks));
    }

    [Authorize(Roles = Roles.Manager)]
    public async Task<IActionResult> ManagerTaskDetails(int taskId)
    {
        var manager = await _userManager.GetUserAsync(User);
        if (manager == null)
            return Forbid();

        var task = await _standaloneTaskService.GetManagerTaskDetailsAsync(taskId, manager.Id);
        if (task == null)
            return NotFound();

        return View("ManagerTaskDetails", task);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Manager)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int taskId)
    {
        var manager = await _userManager.GetUserAsync(User);
        if (manager == null)
            return Forbid();

        await _standaloneTaskService.ApproveTaskAsync(taskId, manager.Id);

        return RedirectToAction(nameof(ManagerTasks));
    }

    [HttpPost]
    [Authorize(Roles = Roles.Manager)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(int taskId)
    {
        var manager = await _userManager.GetUserAsync(User);
        if (manager == null)
            return Forbid();

        await _standaloneTaskService.RejectTaskAsync(taskId, manager.Id);
        return RedirectToAction(nameof(ManagerTasks));
    }


    [Authorize(Roles = Roles.Manager)]
    public async Task<IActionResult> Create()
    {
        var manager = await _userManager.GetUserAsync(User);
        if (manager == null)
            return Forbid();

        var members = await _teamService.GetTeamMembersForCreateTaskAsync(manager.Id);

        var model = new CreateTaskItemViewModel
        {
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

            var teamId = manager.TeamId;
            var employees = await _teamService.GetTeamMembersAsync(teamId);

            model.TeamMembers = employees.Select(e => new SelectListItem
            {
                Text = e.UserName,
                Value = e.Id
            }).ToList();

            return View("Create", model);
        }

        var managerUser = await _userManager.GetUserAsync(User);
        if (managerUser == null)
            return Forbid();

        await _standaloneTaskService.CreateStandaloneTaskAsync(model);

        return RedirectToAction(nameof(ManagerTasks));
    }
}
