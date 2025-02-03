

using CompanyTaskManager.Application.ViewModels.TaskItem;

namespace CompanyTaskManager.Application.Services.TaskItems;

public interface IProjectTaskService
{
    Task<ProjectTaskItemViewModel?> GetEmployeeTaskDetailsAsync(int taskId, string userId);
    Task<ManagerTaskDetailsViewModel?> GetManagerTaskDetailsAsync(int taskId, string managerId);
    Task<ProjectTaskItemViewModel> GetLeaderTaskDetailsAsync(int taskId, string leaderId);

    Task SendForApprovalAsync(int taskId, string userId);
    Task UpdateSubmissionTextAsync(int taskId, string userId, string submissionText);

    Task<int> ApproveProjectTaskAsync(int taskId, string leaderId);
    Task<int> RejectProjectTaskAsync(int taskId, string leaderId);

    Task CreateProjectTaskAsync(CreateTaskItemViewModel model, string managerId);
}
