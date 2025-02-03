

using CompanyTaskManager.Common.Static;

namespace CompanyTaskManager.Application.ViewModels.Shared;

public class WorkIsOverdue
{
    public string WorkStatusName { get; set; } = string.Empty;
    public DateTime EndDate { get; set; }
    public bool IsCompleted => WorkStatusName == WorkStatusesName.Completed;
    public bool IsOverdue => EndDate < DateTime.Now &&
        WorkStatusName != WorkStatusesName.Completed && WorkStatusName != WorkStatusesName.Rejected;
}
