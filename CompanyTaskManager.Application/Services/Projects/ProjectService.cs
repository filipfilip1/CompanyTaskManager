

using AutoMapper;
using CompanyTaskManager.Application.Exceptions;
using CompanyTaskManager.Application.Services.Notifications;
using CompanyTaskManager.Application.ViewModels.Project;
using CompanyTaskManager.Application.ViewModels.User;
using CompanyTaskManager.Data;
using CompanyTaskManager.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace CompanyTaskManager.Application.Services.Projects;

public class ProjectService(ApplicationDbContext _context,
    INotificationService _notificationService,
    IMapper _mapper,
    UserManager<ApplicationUser> _userManager,
    ILogger<ProjectService> _logger
    ) : IProjectService
{
    public async Task<List<ProjectIndexViewModel>> GetProjectsByUserAsync(string userId)
    {
        _logger.LogInformation("Fetching projects for user {UserId}", userId);
        
        var user = await _context.Users.Include(u => u.Team).FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            _logger.LogWarning("User {UserId} not found", userId);
            return new List<ProjectIndexViewModel>();
        }

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


        var result = _mapper.Map<List<ProjectIndexViewModel>>(projects);
        _logger.LogDebug("Found {ProjectCount} projects for user {UserId}", result.Count, userId);
        return result;
    }

    public async Task<ProjectDetailsViewModel> GetProjectByIdAsync(int projectId)
    {
        _logger.LogInformation("Fetching project details for project {ProjectId}", projectId);
        
        var project = await _context.Projects
            .Include(p => p.Leader)
            .Include(p => p.Manager)
            .Include(p => p.Team)
            .Include(p => p.WorkStatus)
            .Include(p => p.Tasks).ThenInclude(t => t.WorkStatus)
            .Include(p => p.Tasks).ThenInclude(t => t.AssignedUser)
            .Include(p => p.ProjectUsers).ThenInclude(pu => pu.User)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        if (project == null)
        {
            _logger.LogWarning("Project {ProjectId} not found", projectId);
            return null;
        }

        var vm = _mapper.Map<ProjectDetailsViewModel>(project);

        var allApproved = project.Tasks.All(t => t.WorkStatusId == 4);
        vm.AllTasksApproved = allApproved;

        _logger.LogDebug("Project {ProjectId} details retrieved successfully, all tasks approved: {AllTasksApproved}", projectId, allApproved);
        return vm;
    }

    public async Task CreateProjectAsync(CreateProjectViewModel projectViewModel)
    {
        _logger.LogInformation("Creating new project '{ProjectName}' for manager {ManagerId}", projectViewModel.Name, projectViewModel.ManagerId);
        
        var manager = await _context.Users.FindAsync(projectViewModel.ManagerId);
        if (manager == null)
        {
            _logger.LogWarning("Failed to create project: Manager with ID {ManagerId} not found", projectViewModel.ManagerId);
            throw new NotFoundException("Manager", projectViewModel.ManagerId);
        }

        var project = _mapper.Map<Project>(projectViewModel);
        project.WorkStatusId = 1;

        if (projectViewModel.SelectedMemberIds != null && projectViewModel.SelectedMemberIds.Any())
        {
            // Validate that all selected members exist in the database
            var existingUserIds = await _context.Users
                .Where(u => projectViewModel.SelectedMemberIds.Contains(u.Id))
                .Select(u => u.Id)
                .ToListAsync();

            var nonExistentUserIds = projectViewModel.SelectedMemberIds.Except(existingUserIds).ToList();
            if (nonExistentUserIds.Any())
            {
                _logger.LogWarning("Failed to create project: Some selected members do not exist. Non-existent IDs: {NonExistentIds}", 
                    string.Join(", ", nonExistentUserIds));
                throw new ValidationException($"The following user IDs do not exist: {string.Join(", ", nonExistentUserIds)}");
            }

            project.ProjectUsers = projectViewModel.SelectedMemberIds
                .Select(memberId => new ProjectUser {
                    UserId = memberId,
                    ProjectId = project.Id
                }).ToList();

            _logger.LogDebug("All {MemberCount} selected members validated successfully", projectViewModel.SelectedMemberIds.Count);
        }


        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Project '{ProjectName}' created successfully with ID {ProjectId}", project.Name, project.Id);
        _logger.LogDebug("Project created with {MemberCount} members assigned", projectViewModel.SelectedMemberIds?.Count ?? 0);
    }


    public async Task AssignProjectLeader(int projectId, string leaderId, string managerId)
    {
        _logger.LogInformation("Assigning leader {LeaderId} to project {ProjectId} by manager {ManagerId}", leaderId, projectId, managerId);
        
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId && p.ManagerId == managerId);
        if (project == null)
        {
            _logger.LogWarning("Failed to assign leader: Project {ProjectId} not found or no access for manager {ManagerId}", projectId, managerId);
            throw new Exceptions.UnauthorizedAccessException("The project was not found or you do not have access to it.");
        }

        project.LeaderId = leaderId;
        await _context.SaveChangesAsync();

        await _notificationService.CreateNotificationAsync(
            project.LeaderId,
            $"You have been designated as a project leader '{project.Name}'.",
            5 // Added As Project Leader
        );
        
        _logger.LogInformation("Successfully assigned leader {LeaderId} to project '{ProjectName}' ({ProjectId})", leaderId, project.Name, projectId);
    }

    public async Task RejectProjectCompletionAsync(int projectId)
    {
        _logger.LogInformation("Rejecting completion of project {ProjectId}", projectId);
        
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
        if (project == null)
        {
            _logger.LogWarning("Failed to reject project completion: Project {ProjectId} not found", projectId);
            throw new NotFoundException("Project", projectId);
        }

        if (project.WorkStatusId != 3)
        {
            _logger.LogWarning("Failed to reject project completion: Project {ProjectId} is not in pending state (current status: {StatusId})", projectId, project.WorkStatusId);
            throw new ValidationException("The project is not in a pending state.");
        }

        project.WorkStatusId = 5; // Rejected
        await _context.SaveChangesAsync();

        await _notificationService.CreateNotificationAsync(
            project.LeaderId,
            $"Completion of project '{project.Name}' was rejected.",
            10
        );
        
        _logger.LogInformation("Project '{ProjectName}' ({ProjectId}) completion rejected successfully", project.Name, projectId);
    }

    public async Task ApproveProjectAsync(int projectId)
    {
        _logger.LogInformation("Approving completion of project {ProjectId}", projectId);
        
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
        if (project == null)
        {
            _logger.LogWarning("Failed to approve project: Project {ProjectId} not found", projectId);
            throw new NotFoundException("Project", projectId);
        }

        if (project.WorkStatusId != 3)
        {
            _logger.LogWarning("Failed to approve project: Project {ProjectId} is not in pending state (current status: {StatusId})", projectId, project.WorkStatusId);
            throw new ValidationException("The project is not in a pending state.");
        }

        project.WorkStatusId = 4; // Completed
        await _context.SaveChangesAsync();

        await _notificationService.CreateNotificationAsync(
            project.LeaderId,
            $"Project '{project.Name}' has been accepted as completed.",
            9 // Project Approve

        );
        
        _logger.LogInformation("Project '{ProjectName}' ({ProjectId}) approved as completed successfully", project.Name, projectId);
    }

    public async Task RequestProjectCompletionAsync(int projectId)
    {
        _logger.LogInformation("Requesting completion for project {ProjectId}", projectId);
        
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
        if (project == null)
        {
            _logger.LogWarning("Failed to request project completion: Project {ProjectId} not found", projectId);
            throw new NotFoundException("Project", projectId);
        }

        var tasksAll = await _context.TaskItems
            .Where(t => t.ProjectId == project.Id)
            .ToListAsync();

        // Check if project is in correct state for completion request
        if (project.WorkStatusId != 1) // Not Active
        {
            _logger.LogWarning("Cannot request completion for project {ProjectId}: Project is not in Active state (current state: {WorkStatusId})", 
                projectId, project.WorkStatusId);
            throw new ValidationException("Project must be in Active state to request completion.");
        }

        // Check if all tasks in project are completed
        bool allTasksIsApproved = tasksAll.All(t => t.WorkStatusId == 4); // Completed
        if (!allTasksIsApproved)
        {
            _logger.LogWarning("Cannot request completion for project {ProjectId}: Not all tasks are completed ({CompletedTasks}/{TotalTasks})", 
                projectId, tasksAll.Count(t => t.WorkStatusId == 4), tasksAll.Count);
            throw new ValidationException("Not all tasks are completed yet. Cannot request project completion.");
        }


        project.WorkStatusId = 3; // Completion Pending
        await _context.SaveChangesAsync();

        await _notificationService.CreateNotificationAsync(
            project.ManagerId,
            $"Project '{project.Name}' has been submitted for completion.",
            8 // Project Waiting For Approve
        );
        
        _logger.LogInformation("Project '{ProjectName}' ({ProjectId}) completion requested successfully with {TaskCount} completed tasks", 
            project.Name, projectId, tasksAll.Count);
    }


}
