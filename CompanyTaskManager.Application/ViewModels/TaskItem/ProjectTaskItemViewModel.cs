

using CompanyTaskManager.Application.ViewModels.Shared;

namespace CompanyTaskManager.Application.ViewModels.TaskItem;

public class ProjectTaskItemViewModel : WorkIsOverdue
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public int ProjectId { get; set; }
    public string AssignedUserId { get; set; } = string.Empty;
    public string AssignedUserName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }

    public string? SubmissionText { get; set; }
    public bool CanSendForApproval { get; set; }
}
