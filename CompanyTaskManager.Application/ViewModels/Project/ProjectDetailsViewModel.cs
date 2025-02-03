

using CompanyTaskManager.Application.ViewModels.Shared;
using CompanyTaskManager.Application.ViewModels.TaskItem;
using CompanyTaskManager.Application.ViewModels.User;
using CompanyTaskManager.Common.Static;

namespace CompanyTaskManager.Application.ViewModels.Project;

public class ProjectDetailsViewModel : WorkIsOverdue
{
    public int Id { get; set; }
    public string LeaderId { get; set; } = string.Empty;
    public string ManagerId { get; set; } = string.Empty;
    public string CurrentUserId { get; set; } = string.Empty;

    public string LeaderName { get; set; } = string.Empty;
    public string ManagerName { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string TeamName { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }


    public List<ProjectTaskItemViewModel> Tasks { get; set; } = new();
    public List<UserViewModel> Members { get; set; } = new();

    // Flags for view
    public bool AllTasksApproved { get; set; }
    public bool CanRequestProjectCompletion { get; set; }
    public bool CanCompleteProject { get; set; }
    public bool CanRejectProject { get; set; }

}
