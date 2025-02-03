

using AutoMapper;
using CompanyTaskManager.Application.Services.Notifications;
using CompanyTaskManager.Application.ViewModels.Project;
using CompanyTaskManager.Application.ViewModels.User;
using CompanyTaskManager.Data;
using CompanyTaskManager.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CompanyTaskManager.Application.Services.Projects;

public class ProjectService(ApplicationDbContext _context,
    INotificationService _notificationService,
    IMapper _mapper,
    UserManager<ApplicationUser> _userManager
    ) : IProjectService
{
    public async Task<List<ProjectIndexViewModel>> GetProjectsByUserAsync(string userId)
    {
        var user = await _context.Users.Include(u => u.Team).FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            return new List<ProjectIndexViewModel>();

        var projects = await _context.Projects
            .Include(p => p.Leader)
            .Include(p => p.Manager)
            .Include(p => p.Team)
            .Include(p => p.WorkStatus)
            .Include(p => p.Tasks)
            .Where(p => p.ManagerId == userId ||
                        p.LeaderId == userId ||
                        p.ProjectUsers.Any(pu => pu.UserId == userId))
            .ToListAsync();


        return _mapper.Map<List<ProjectIndexViewModel>>(projects);
    }

    public async Task<ProjectDetailsViewModel> GetProjectByIdAsync(int projectId)
    {
        var project = await _context.Projects
            .Include(p => p.Leader)
            .Include(p => p.Manager)
            .Include(p => p.Team)
            .Include(p => p.WorkStatus)
            .Include(p => p.Tasks).ThenInclude(t => t.WorkStatus)
            .Include(p => p.Tasks).ThenInclude(t => t.AssignedUser)
            .Include(p => p.ProjectUsers).ThenInclude(pu => pu.User)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        if (project == null) return null;

        var vm = _mapper.Map<ProjectDetailsViewModel>(project);

        var allApproved = project.Tasks.All(t => t.WorkStatusId == 4);
        vm.AllTasksApproved = allApproved;

        return vm;
    }

    public async Task CreateProjectAsync(CreateProjectViewModel projectViewModel)
    {
        var manager = await _context.Users.FindAsync(projectViewModel.ManagerId);
        if (manager == null)
        {
            throw new Exception($"Manager with ID '{projectViewModel.ManagerId}' not found.");
        }

        var project = _mapper.Map<Project>(projectViewModel);
        project.WorkStatusId = 1;

        if (projectViewModel.SelectedMemberIds != null && projectViewModel.SelectedMemberIds.Any())
        {
            project.ProjectUsers = projectViewModel.SelectedMemberIds
                .Select(memberId => new ProjectUser {
                    UserId = memberId,
                    ProjectId = project.Id
                }).ToList();
        }


        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
    }


    public async Task AssignProjectLeader(int projectId, string leaderId, string managerId)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId && p.ManagerId == managerId);
        if (project == null)
            throw new Exception("The project was not found or you do not have access to it.");

        project.LeaderId = leaderId;
        await _context.SaveChangesAsync();

        await _notificationService.CreateNotificationAsync(
            project.LeaderId,
            $"You have been designated as a project leader '{project.Name}'.",
            5 // Added As Project Leader
        );
    }

    public async Task RejectProjectCompletionAsync(int projectId)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
        if (project == null)
            throw new Exception("Project was not found.");

        if (project.WorkStatusId != 3)
            throw new Exception("The project is not in a pending state.");

        project.WorkStatusId = 5; // Rejected
        await _context.SaveChangesAsync();

        await _notificationService.CreateNotificationAsync(
            project.LeaderId,
            $"Completion of project '{project.Name}' was rejected.",
            10
        );
    }

    public async Task ApproveProjectAsync(int projectId)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
        if (project == null)
            throw new Exception("Project was not found.");

        if (project.WorkStatusId != 3)
            throw new Exception("The project is not in a pending state.");

        project.WorkStatusId = 4; // Completed
        await _context.SaveChangesAsync();

        await _notificationService.CreateNotificationAsync(
            project.LeaderId,
            $"Project '{project.Name}' has been accepted as completed.",
            9 // Project Approve

        );
    }

    public async Task RequestProjectCompletionAsync(int projectId)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
        if (project == null)
            throw new Exception("Project was not found.");

        var tasksAll = await _context.TaskItems
            .Where(t => t.ProjectId == project.Id)
            .ToListAsync();

        // Check if all tasks in project are completed
        bool allTasksIsApproved = tasksAll.All(t => t.WorkStatusId == 4); // Completed
        if (!allTasksIsApproved)
            throw new Exception("Not all tasks are completed yet. Cannot request project completion.");


        project.WorkStatusId = 3; // Completion Pending
        await _context.SaveChangesAsync();

        await _notificationService.CreateNotificationAsync(
            project.ManagerId,
            $"Project '{project.Name}' has been submitted for completion.",
            8 // Project Waiting For Approve
        );
    }


}
