
namespace CompanyTaskManager.Data.Models;

public class ProjectUser : BaseEntity
{
    public int ProjectId { get; set; }
    public Project Project { get; set; }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}
