
using CompanyTaskManager.Application.ViewModels.Shared;

namespace CompanyTaskManager.Application.ViewModels.TaskItem;

public class TaskItemViewModel : WorkIsOverdue
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

}
