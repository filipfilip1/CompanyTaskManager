
using AutoMapper;
using CompanyTaskManager.Application.Services.Notifications;
using CompanyTaskManager.Application.ViewModels.TaskItem;
using CompanyTaskManager.Data;
using CompanyTaskManager.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CompanyTaskManager.Application.Services.TaskItems;

public class ProjectTaskService(ApplicationDbContext _context,
    INotificationService _notificationService,
    IMapper _mapper,
    UserManager<ApplicationUser> _userManager) : IProjectTaskService
{
    // === Employee Method ===

    public async Task<ProjectTaskItemViewModel?> GetEmployeeTaskDetailsAsync(int taskId, string userId)
    {
        var task = await _context.TaskItems
            .Include(t => t.AssignedUser)
            .Include(t => t.WorkStatus)
            .Include(t => t.Project).ThenInclude(p => p.Leader)
            .FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId != null);

        if (task == null)
            return null;

        if (task.AssignedUserId != userId)
            return null;

        var viewModel = _mapper.Map<ProjectTaskItemViewModel>(task);

        // Set CanSendForApproval flag
        if (task.WorkStatusId == 1 && !(task.EndDate < DateTime.Now)) // Active and not overdue
        {
            viewModel.CanSendForApproval = true;
        }

        return viewModel;
    }

    public async Task SendForApprovalAsync(int taskId, string userId)
    {
        var task = await _context.TaskItems
            .Include(t => t.AssignedUser)
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId != null);

        if (task == null)
            throw new Exception("Project task not found.");

        if (task.AssignedUserId != userId)
            throw new Exception("You are not assigned to this project task.");

        // Set task status to 'Completion Pending'
        task.WorkStatusId = 3; // Completion Pending
        await _context.SaveChangesAsync();

        var leaderId = task.Project?.LeaderId;
        if (!string.IsNullOrEmpty(leaderId))
        {
            await _notificationService.CreateNotificationAsync(
                leaderId,
                $"User {task.AssignedUser.UserName} has reported the completion of project task '{task.Title}'.",
                11
            );
        }
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSubmissionTextAsync(int taskId, string userId, string submissionText)
    {
        var task = await _context.TaskItems
            .FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId != null);

        if (task == null)
            throw new Exception("Project task not found.");

        if (task.AssignedUserId != userId)
            throw new Exception("You are not assigned to this project task.");

        task.SubmissionText = submissionText;
        await _context.SaveChangesAsync();
    }

    // === Manager / Leader Method ===


    public async Task<ProjectTaskItemViewModel> GetLeaderTaskDetailsAsync(int taskId, string leaderId)
    {
        var task = await _context.TaskItems
            .Include(t => t.AssignedUser)
            .Include(t => t.WorkStatus)
            .Include(t => t.Project).ThenInclude(p => p.Leader)
            .FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId != null);

        if (task == null)
            return null;

        if (task.Project.LeaderId != leaderId)
            return null;

        var viewModel = _mapper.Map<ProjectTaskItemViewModel>(task);


        return viewModel;
    }

    public async Task<ManagerTaskDetailsViewModel?> GetManagerTaskDetailsAsync(int taskId, string managerId)
    {
        var task = await _context.TaskItems
            .Include(t => t.AssignedUser)
            .Include(t => t.Project)
            .Include(t => t.WorkStatus)
            .FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId != null);

        if (task == null)
            return null;

        if (task.Project?.ManagerId != managerId)
            return null;

        return _mapper.Map<ManagerTaskDetailsViewModel>(task);
    }

    public async Task<int> ApproveProjectTaskAsync(int taskId, string leaderId)
    {
        var task = await _context.TaskItems
            .Include(t => t.AssignedUser)
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId != null);

        if (task == null)
            throw new Exception("Project task not found.");

        if (task.WorkStatusId != 3) // 3 = "Completion Pending"
            throw new Exception("Task is not pending approval.");

        // Only Leader can approve
        if (task.Project == null)
            throw new Exception("Project not found for this task.");

        if (task.Project.LeaderId != leaderId)
            throw new Exception("You don't have permission to approve this project task (Leader only).");

        task.WorkStatusId = 4;


        await _notificationService.CreateNotificationAsync(
            task.AssignedUserId,
            $"Your project task '{task.Title}' has been approved as completed by the Leader.",
            12 //Task Completed Approve
        );
        await _context.SaveChangesAsync();

        return task.ProjectId.Value;
    }

    public async Task<int> RejectProjectTaskAsync(int taskId, string leaderId)
    {
        var task = await _context.TaskItems
            .Include(t => t.AssignedUser)
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId != null);

        if (task == null)
            throw new Exception("Project task not found.");

        if (task.WorkStatusId != 3) // 3 = "Completion Pending"
            throw new Exception("Task is not pending approval.");

        if (task.Project == null)
            throw new Exception("Project not found for this task.");

        if (task.Project.LeaderId != leaderId)
            throw new Exception("You don't have permission to reject this project task (Leader only).");

        task.WorkStatusId = 5; // 5 = Rejected
        await _context.SaveChangesAsync();

        await _notificationService.CreateNotificationAsync(
            task.AssignedUserId,
            $"Request for completion of project task '{task.Title}' was rejected by the Leader.",
            13 //Task Completion Rejected
        );

        return task.ProjectId.Value;
    }

    // === Create ===

    public async Task CreateProjectTaskAsync(CreateTaskItemViewModel model, string managerId)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == model.ProjectId && p.ManagerId == managerId);

        if (project == null)
            throw new Exception("Project not found or no access.");

        var taskItem = _mapper.Map<TaskItem>(model);
        taskItem.WorkStatusId = 1; // Active

        _context.TaskItems.Add(taskItem);
        await _context.SaveChangesAsync();

        await _notificationService.CreateNotificationAsync(
            model.AssignedUserId,
            $"You have been assigned to task '{taskItem.Title}' in project '{project.Name}'.",
            6 // Added To Task
        );
    }

}
