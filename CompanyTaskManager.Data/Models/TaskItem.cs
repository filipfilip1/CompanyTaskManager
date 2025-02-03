

namespace CompanyTaskManager.Data.Models;

public class TaskItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string AssignedUserId { get; set; }
    public ApplicationUser? AssignedUser { get; set; }

    public int? ProjectId { get; set; }
    public Project? Project { get; set; }

    public int WorkStatusId { get; set; }
    public WorkStatus? WorkStatus { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string? SubmissionText { get; set; }

}
