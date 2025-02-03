
using Microsoft.AspNetCore.Identity;

namespace CompanyTaskManager.Data.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public Team? Team { get; set; }
    public string? TeamId { get; set; }

    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();
}
