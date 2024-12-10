

namespace CompanyTaskManager.Data.Models;

public class Team
{
    public string Id { get; set; }
    public string Name { get; set; }

    public string ManagerId { get; set; }
    public ApplicationUser Manager { get; set; }

    public ICollection<ApplicationUser> Members { get; set; } = new List<ApplicationUser>();
}
