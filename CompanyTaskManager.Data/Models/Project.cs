

using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyTaskManager.Data.Models;

public class Project : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string LeaderId { get; set; }
    public ApplicationUser? Leader { get; set; }

    public string ManagerId { get; set; }
    public ApplicationUser? Manager { get; set; }

    public string TeamId { get; set; }
    public Team? Team { get; set; }

    public int WorkStatusId { get; set; }
    public WorkStatus? WorkStatus { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public ICollection<TaskItem> Tasks { get; set; } = [];
    public ICollection<ProjectUser> ProjectUsers { get; set; } = [];
    [NotMapped]
    public IEnumerable<ApplicationUser> ProjectMembers => ProjectUsers.Select(pu => pu.User);

}
