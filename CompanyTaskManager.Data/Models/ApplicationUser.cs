
using Microsoft.AspNetCore.Identity;

namespace CompanyTaskManager.Data.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public Team? Team { get; set; }
    public string? TeamId { get; set; }

    public ICollection<ApplicationRole> Roles { get; set; } = new List<ApplicationRole>();
}
