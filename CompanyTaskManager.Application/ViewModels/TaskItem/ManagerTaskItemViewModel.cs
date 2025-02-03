

using CompanyTaskManager.Application.ViewModels.Shared;
using CompanyTaskManager.Common.Static;

namespace CompanyTaskManager.Application.ViewModels.TaskItem;

public class ManagerTaskItemViewModel : WorkIsOverdue
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string AssignedUserName { get; set; } = string.Empty;

}
