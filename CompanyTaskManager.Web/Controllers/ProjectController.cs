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
    ApplicationDbContext _context,
    ILogger<ProjectController> _logger) : Controller
{ 
    public async Task<IActionResult> Index(string status = null, string leaderName = null, bool showOnlyOverdue = false)
    {
        var user = await _userManager.GetUserAsync(User);
        var userName = user?.UserName ?? "Unknown";
        
        _logger.LogInformation("User {UserName} ({UserId}) is accessing projects index with filters: Status={Status}, LeaderName={LeaderName}, ShowOnlyOverdue={ShowOnlyOverdue}", 
            userName, user?.Id, status ?? "All", leaderName ?? "All", showOnlyOverdue);
            
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

        _logger.LogInformation("Successfully retrieved {ProjectCount} projects for user {UserName}", 
            projects.Count, userName);

        return View(projects);
    }

    [Authorize(Roles = Roles.Manager)]
    public async Task<IActionResult> Create()
    {
        var manager = await _userManager.GetUserAsync(User);
        var managerName = manager?.UserName ?? "Unknown";
        
        _logger.LogInformation("Manager {ManagerName} ({ManagerId}) is accessing project creation form", 
            managerName, manager?.Id);
            
        var team = await _context.Teams.Include(t => t.Members).FirstOrDefaultAsync(t => t.ManagerId == manager.Id);

        var model = new CreateProjectViewModel();

        if (team != null)
        {
            model.ManagerId = manager.Id;
            model.TeamId = manager.Id;
            model.TeamMembers = team.Members
                .Select(u => new SelectListItem(u.UserName, u.Id))
                .ToList();
                
            _logger.LogInformation("Team with {MemberCount} members loaded for manager {ManagerName}", 
                team.Members.Count, managerName);
        }
        else
        {
            _logger.LogWarning("No team found for manager {ManagerName} ({ManagerId})", 
                managerName, manager?.Id);
        }

        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Manager)]
    public async Task<IActionResult> Create(CreateProjectViewModel model)
    {
        var manager = await _userManager.GetUserAsync(User);
        var managerName = manager?.UserName ?? "Unknown";
        
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for project creation by manager {ManagerName} ({ManagerId})", 
                managerName, manager?.Id);
                
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

        _logger.LogInformation("Manager {ManagerName} ({ManagerId}) is creating project '{ProjectName}' with leader {LeaderId}", 
            managerName, manager?.Id, model.Name, model.LeaderId);
            
        await _projectService.CreateProjectAsync(model);
        
        _logger.LogInformation("Project '{ProjectName}' successfully created by manager {ManagerName}", 
            model.Name, managerName);
            
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Details(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Forbid();

        var projectVm = await _projectService.GetProjectByIdAsync(id);
        if (projectVm == null) return NotFound();

        // Check if user has access to this project
        bool isManager = projectVm.ManagerId == user.Id;
        bool isLeader = projectVm.LeaderId == user.Id;
        bool isProjectMember = projectVm.Members?.Any(m => m.Id == user.Id) ?? false;
        
        if (!isManager && !isLeader && !isProjectMember)
        {
            _logger.LogWarning("User {UserId} ({UserName}) attempted to access project {ProjectId} without permissions", 
                user.Id, user.UserName, id);
            return Forbid();
        }

        // set current user id
        projectVm.CurrentUserId = user.Id;

        // logic to determine if user can request project completion, approve project, reject project
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
        var userName = user?.UserName ?? "Unknown";
        
        _logger.LogInformation("Manager {UserName} ({UserId}) is assigning leader {LeaderId} to project {ProjectId}", 
            userName, user?.Id, leaderId, projectId);
            
        await _projectService.AssignProjectLeader(projectId, leaderId, user.Id);
        
        _logger.LogInformation("Leader {LeaderId} successfully assigned to project {ProjectId} by manager {UserName}", 
            leaderId, projectId, userName);
            
        return RedirectToAction("Details", new { id = projectId });
    }

    [HttpPost]
    [Authorize(Roles = Roles.Employee)]
    public async Task<IActionResult> RequestCompletion(int projectId)
    {
        var user = await _userManager.GetUserAsync(User);
        var userName = user?.UserName ?? "Unknown";
        
        _logger.LogInformation("User {UserName} ({UserId}) is requesting completion for project {ProjectId}", 
            userName, user?.Id, projectId);

        var project = await _context.Projects.Include(p => p.Leader).FirstOrDefaultAsync(p => p.Id == projectId);
        if (project == null)
        {
            _logger.LogWarning("Project {ProjectId} not found when user {UserName} requested completion", 
                projectId, userName);
            return NotFound();
        }

        if (project.LeaderId != user.Id)
        {
            _logger.LogWarning("User {UserName} ({UserId}) attempted to request completion for project {ProjectId} but is not the leader", 
                userName, user?.Id, projectId);
            return Forbid(); 
        }

        await _projectService.RequestProjectCompletionAsync(projectId);
        
        _logger.LogInformation("Project {ProjectId} completion successfully requested by leader {UserName}", 
            projectId, userName);
            
        return RedirectToAction("Details", new { id = projectId });
    }

    [HttpPost]
    [Authorize(Roles = Roles.Manager)]
    public async Task<IActionResult> ApproveProject(int projectId)
    {
        var user = await _userManager.GetUserAsync(User);
        var userName = user?.UserName ?? "Unknown";
        
        _logger.LogInformation("Manager {UserName} ({UserId}) is approving project {ProjectId}", 
            userName, user?.Id, projectId);
            
        await _projectService.ApproveProjectAsync(projectId);
        
        _logger.LogInformation("Project {ProjectId} successfully approved by manager {UserName}", 
            projectId, userName);
            
        return RedirectToAction("Details", new { id = projectId });
    }

    [HttpPost]
    [Authorize(Roles = Roles.Manager)]
    public async Task<IActionResult> RejectCompletion(int projectId)
    {
        var user = await _userManager.GetUserAsync(User);
        var userName = user?.UserName ?? "Unknown";
        
        _logger.LogInformation("Manager {UserName} ({UserId}) is rejecting completion for project {ProjectId}", 
            userName, user?.Id, projectId);
            
        await _projectService.RejectProjectCompletionAsync(projectId);
        
        _logger.LogInformation("Project {ProjectId} completion successfully rejected by manager {UserName}", 
            projectId, userName);
            
        return RedirectToAction("Details", new { id = projectId });
    }
}
