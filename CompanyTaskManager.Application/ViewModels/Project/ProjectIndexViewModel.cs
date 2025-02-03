
using CompanyTaskManager.Application.ViewModels.Shared;

namespace CompanyTaskManager.Application.ViewModels.Project;

public class ProjectIndexViewModel : WorkIsOverdue
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LeaderName { get; set; } = string.Empty;
}
