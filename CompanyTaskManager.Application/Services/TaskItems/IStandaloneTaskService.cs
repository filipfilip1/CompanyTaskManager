
using CompanyTaskManager.Application.ViewModels.TaskItem;

namespace CompanyTaskManager.Application.Services.TaskItems;

public interface IStandaloneTaskService
{
    Task<List<TaskItemViewModel>> GetTasksForEmployeeAsync(string userId);
    Task<StandaloneTaskItemViewModel?> GetEmployeeTaskDetailsAsync(int taskId, string userId);
    Task SendForApprovalAsync(int taskId, string userId);
    Task UpdateSubmissionTextAsync(int taskId, string userId, string submissionText);
    Task<List<ManagerTaskItemViewModel>> GetTasksForManagerAsync(string managerId);
    Task<ManagerTaskDetailsViewModel?> GetManagerTaskDetailsAsync(int taskId, string managerId);
    Task ApproveTaskAsync(int taskId, string managerId);
    Task RejectTaskAsync(int taskId, string managerId);
    Task CreateStandaloneTaskAsync(CreateTaskItemViewModel model);
}
