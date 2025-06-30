

using AutoMapper;
using CompanyTaskManager.Application.Services.Notifications;
using CompanyTaskManager.Application.ViewModels.TaskItem;
using CompanyTaskManager.Data;
using CompanyTaskManager.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace CompanyTaskManager.Application.Services.TaskItems;

public class StandaloneTaskService(ApplicationDbContext _context,
    INotificationService _notificationService,
    IMapper _mapper,
    UserManager<ApplicationUser> _userManager,
    ILogger<StandaloneTaskService> _logger) : IStandaloneTaskService
{
    // === Employee Method ===
    public async Task<List<TaskItemViewModel>> GetTasksForEmployeeAsync(string userId)
    {
        _logger.LogInformation("Fetching standalone tasks for employee {UserId}", userId);
        var tasks = await _context.TaskItems
            .Include(t => t.AssignedUser)
            .Include(t => t.WorkStatus)
            .Where(t => t.AssignedUserId == userId && t.ProjectId == null)
            .ToListAsync();

        return _mapper.Map<List<TaskItemViewModel>>(tasks);
    }

    public async Task<StandaloneTaskItemViewModel> GetEmployeeTaskDetailsAsync(int taskId, string userId)
    {
        var task = await _context.TaskItems
            .Include(t => t.AssignedUser)
            .Include(t => t.WorkStatus)
            .FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId == null);

        if (task == null)
            return null;

        if (task.AssignedUserId != userId)
            return null;

        var viewModel = _mapper.Map<StandaloneTaskItemViewModel>(task);

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
            .ThenInclude(u => u.Team) 
            .FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId == null);

        if (task == null)
            throw new Exception("Standalone task not found.");

        if (task.AssignedUserId != userId)
            throw new Exception("You are not assigned to this task.");

        task.WorkStatusId = 3; // "Completion Pending"
        await _context.SaveChangesAsync();

        // Sending notification to the manager of the team
        var managerId = task.AssignedUser.Team?.ManagerId;
        if (!string.IsNullOrEmpty(managerId))
        {
            await _notificationService.CreateNotificationAsync(
                managerId,
                $"User {task.AssignedUser.UserName} has reported the completion of standalone task '{task.Title}'.",
                11 // “Task Waiting For Approve”
            );
        }
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSubmissionTextAsync(int taskId, string userId, string submissionText)
    {
        var task = await _context.TaskItems
            .FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId == null);

        if (task == null)
            throw new Exception("Standalone task not found.");

        if (task.AssignedUserId != userId)
            throw new Exception("You are not assigned to this task.");

        task.SubmissionText = submissionText;
        await _context.SaveChangesAsync();
    }

    // === Manager method ===

    public async Task<List<ManagerTaskItemViewModel>> GetTasksForManagerAsync(string managerId)
    {
        var tasks = await _context.TaskItems
            .Include(t => t.AssignedUser).ThenInclude(u => u.Team)
            .Include(t => t.WorkStatus)
            .Where(t => t.ProjectId == null
                        && t.AssignedUser.Team != null
                        && t.AssignedUser.Team.ManagerId == managerId)
            .ToListAsync();

        return _mapper.Map<List<ManagerTaskItemViewModel>>(tasks);
    }

    public async Task<ManagerTaskDetailsViewModel?> GetManagerTaskDetailsAsync(int taskId, string managerId)
    {
        var task = await _context.TaskItems
            .Include(t => t.AssignedUser).ThenInclude(u => u.Team)
            .Include(t => t.WorkStatus)
            .FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId == null);

        if (task == null)
            return null;

        var teamManagerId = task.AssignedUser.Team?.ManagerId;
        if (teamManagerId != managerId)
            return null;

        var viewModel = _mapper.Map<ManagerTaskDetailsViewModel>(task);

        // Set CanApproveOrReject flag
        if (task.WorkStatusId == 3) // "Completion Pending"
        {
            viewModel.CanApproveOrReject = true;
        }

        return viewModel;
    }

    public async Task ApproveTaskAsync(int taskId, string managerId)
    {
        var task = await _context.TaskItems
            .Include(t => t.AssignedUser).ThenInclude(u => u.Team)
            .FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId == null);

        if (task == null)
            throw new Exception("Task not found.");

        if (task.WorkStatusId != 3)
            throw new Exception("Task is not pending approval.");

        var teamManagerId = task.AssignedUser?.Team?.ManagerId;
        if (teamManagerId != managerId)
            throw new Exception("You don't have permission to approve this standalone task.");

        // Approving the task
        task.WorkStatusId = 4; // "Completed"
        await _context.SaveChangesAsync();

        // Sending notification to the employee
        await _notificationService.CreateNotificationAsync(
            task.AssignedUserId,
            $"Your standalone task '{task.Title}' has been approved as completed.",
            12
        );
    }

    public async Task RejectTaskAsync(int taskId, string managerId)
    {
        var task = await _context.TaskItems
            .Include(t => t.AssignedUser).ThenInclude(u => u.Team)
            .FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId == null);

        if (task == null)
            throw new Exception("Task not found.");

        if (task.WorkStatusId != 3)
            throw new Exception("Task is not pending approval.");

        var teamManagerId = task.AssignedUser?.Team?.ManagerId;
        if (teamManagerId != managerId)
            throw new Exception("You don't have permission to reject this standalone task.");

        // Rejecting the task
        task.WorkStatusId = 5; // "Rejected"
        await _context.SaveChangesAsync();

        // Sending notification to the employee
        await _notificationService.CreateNotificationAsync(
            task.AssignedUserId,
            $"Request for completion of standalone task '{task.Title}' was rejected.",
            13
        );
    }

    // === Create ===

    public async Task CreateStandaloneTaskAsync(CreateTaskItemViewModel model)
    {

        var taskItem = _mapper.Map<TaskItem>(model);
        taskItem.ProjectId = null;
        taskItem.WorkStatusId = 1; // Active

        _context.TaskItems.Add(taskItem);
        await _context.SaveChangesAsync();

        // Sending notification to assigned user
        await _notificationService.CreateNotificationAsync(
            model.AssignedUserId,
            $"You have been assigned to new standalone task '{taskItem.Title}'.",
            6
        );
    }


}
