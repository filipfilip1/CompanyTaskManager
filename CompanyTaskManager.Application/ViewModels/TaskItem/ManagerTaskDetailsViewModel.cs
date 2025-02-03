

using CompanyTaskManager.Application.ViewModels.Shared;

namespace CompanyTaskManager.Application.ViewModels.TaskItem;

public class ManagerTaskDetailsViewModel : WorkIsOverdue
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

    public string AssignedUserName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string? SubmissionText { get; set; }

    public DateTime StartDate { get; set; } 

    public string ProjectName { get; set; } = string.Empty; 

    public bool CanApproveOrReject { get; set; }
}