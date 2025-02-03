using CompanyTaskManager.Application.Services.Projects;
using CompanyTaskManager.Application.ViewModels.Project;
using CompanyTaskManager.Data.Models;
using CompanyTaskManager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CompanyTaskManager.Common.Static;
using CompanyTaskManager.Application.Services.WorkStatuses;

namespace CompanyTaskManager.Web.Controllers;

[Authorize]
public class ProjectController(IProjectService _projectService,
    IWorkStatusService _workStatusService,
    UserManager<ApplicationUser> _userManager,
    ApplicationDbContext _context) : Controller
{ 
    public async Task<IActionResult> Index(string status = null, string leaderName = null, bool showOnlyOverdue = false)
    {
        var user = await _userManager.GetUserAsync(User);
        var projects = await _projectService.GetProjectsByUserAsync(user.Id);

        var allStatuses = await _workStatusService.GetAllWorkStatusesAsync();
        ViewBag.Statuses = allStatuses;

        if (!string.IsNullOrEmpty(status))
            projects = projects
                .Where(p => p.WorkStatusName == status)
                .ToList();

        if (!string.IsNullOrEmpty(leaderName))
            projects = projects
                .Where(p => p.LeaderName.Contains(leaderName, StringComparison.OrdinalIgnoreCase))
                .ToList();

        if (showOnlyOverdue)
        {
            projects = projects
                .Where(t => t.IsOverdue)
                .ToList();

            projects = projects.OrderByDescending(t => t.EndDate).ToList();
        }
        else
        {

            projects = projects
                .OrderBy(t => t.IsOverdue).ThenBy(t => t.IsCompleted)  
                .ThenByDescending(t => t.EndDate)
                .ToList();
        }


        return View(projects);
    }

    [Authorize(Roles = Roles.Manager)]
    public async Task<IActionResult> Create()
    {
        var manager = await _userManager.GetUserAsync(User);
        var team = await _context.Teams.Include(t => t.Members).FirstOrDefaultAsync(t => t.ManagerId == manager.Id);

        var model = new CreateProjectViewModel();

        if (team != null)
        {
            model.ManagerId = manager.Id;
            model.TeamId = manager.Id;
            model.TeamMembers = team.Members
                .Select(u => new SelectListItem(u.UserName, u.Id))
                .ToList();
        }

        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Manager)]
    public async Task<IActionResult> Create(CreateProjectViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var manager = await _userManager.GetUserAsync(User);
            var team = await _context.Teams.Include(t => t.Members).FirstOrDefaultAsync(t => t.ManagerId == manager.Id);
            if (team != null)
            {
                model.ManagerId = manager.Id;
                model.TeamId = manager.Id;
                model.TeamMembers = team.Members
                    .Select(u => new SelectListItem(u.UserName, u.Id))
                    .ToList();
            }
            return View(model);
        }

        await _projectService.CreateProjectAsync(model);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Details(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Forbid();

        var projectVm = await _projectService.GetProjectByIdAsync(id);
        if (projectVm == null) return NotFound();

        // set current user id
        projectVm.CurrentUserId = user.Id;

        // logic to determine if user can request project completion, approve project, reject project
        bool isLeader = (projectVm.LeaderId == user.Id);
        bool isManager = (projectVm.ManagerId == user.Id);
        bool isActive = (projectVm.WorkStatusName == WorkStatusesName.Active);
        bool isPending = (projectVm.WorkStatusName == WorkStatusesName.CompletionPending);

        // set flags for view model
        projectVm.CanRequestProjectCompletion = isLeader && isActive && projectVm.AllTasksApproved;
        projectVm.CanCompleteProject = isManager && isPending;
        projectVm.CanRejectProject = isManager && isPending;

        return View("Details", projectVm);
    }



    [HttpPost]
    [Authorize(Roles = Roles.Manager)]
    public async Task<IActionResult> AssignLeader(int projectId, string leaderId)
    {
        var user = await _userManager.GetUserAsync(User);
        await _projectService.AssignProjectLeader(projectId, leaderId, user.Id);
        return RedirectToAction("Details", new { id = projectId });
    }

    [HttpPost]
    [Authorize(Roles = Roles.Employee)]
    public async Task<IActionResult> RequestCompletion(int projectId)
    {
        var user = await _userManager.GetUserAsync(User);

        var project = await _context.Projects.Include(p => p.Leader).FirstOrDefaultAsync(p => p.Id == projectId);
        if (project == null)
        {
            return NotFound();
        }

        if (project.LeaderId != user.Id)
        {
            return Forbid(); 
        }

        await _projectService.RequestProjectCompletionAsync(projectId);
        return RedirectToAction("Details", new { id = projectId });
    }

    [HttpPost]
    [Authorize(Roles = Roles.Manager)]
    public async Task<IActionResult> ApproveProject(int projectId)
    {
        await _projectService.ApproveProjectAsync(projectId);
        return RedirectToAction("Details", new { id = projectId });
    }

    [HttpPost]
    [Authorize(Roles = Roles.Manager)]
    public async Task<IActionResult> RejectCompletion(int projectId)
    {
        await _projectService.RejectProjectCompletionAsync(projectId);
        return RedirectToAction("Details", new { id = projectId });
    }
}
